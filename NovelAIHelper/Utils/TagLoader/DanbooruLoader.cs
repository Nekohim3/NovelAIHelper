using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NovelAIHelper.DataBase.Entities.DataBase;
using NovelAIHelper.DataBase.Entities.ViewModels;
using NovelAIHelper.DataBase.Services;

namespace NovelAIHelper.Utils.TagDownloader
{
    internal class DanbooruLoader
    {
        public static string       BaseAddress = "https://danbooru.donmai.us";
        public        List<UI_Dir> DirTree     = new List<UI_Dir>();

        public DanbooruLoader()
        {

        }

        public bool SaveDirs(IList<UI_Dir> dirsTree)
        {
            var service = new DirService();
            return service.AddRange(dirsTree);
        }

        public void DownloadAll()
        {
            var loader   = new DanbooruLoader();
            DirTree = loader.DownloadDirs().ToList();
            LoadTagsForDirTree();
            var saved    = loader.SaveDirs(DirTree);
        }

        private HtmlNode GetWikiBody(string path)
        {
            var client = new HttpClient();
            var html   = client.GetStringAsync(CombineUrl(path)).Result;
            
            //var html   = new WebClient().DownloadString(CombineUrl(path));
            //var html = Http.GetHtml($"/{path.TrimStart('/')}");
            var doc  = new HtmlDocument();
            doc.LoadHtml(html);
            var wikibody = doc.GetElementbyId("wiki-page-body");
            return wikibody;
        }

        private IEnumerable<HtmlNode> DownloadUls(HtmlNode wikibody)
        {
            return wikibody.ChildNodes.Where(x => x.Name.ToLower() == "ul");
        }

        public IList<UI_Dir> DownloadDirs()
        {
            var uls      = DownloadUls(GetWikiBody("wiki_pages/tag_groups"));
            var dirsTree = new List<UI_Dir>();
            foreach (var ul in uls)
            {
                if (ul.PreviousSibling.InnerText.ToLower().Contains("see also")) continue;
                var dir = new UI_Dir(ul.PreviousSibling.InnerText);
                if (dir.Name.ToLower().Contains("tag group:"))
                    dir.Name = dir.Name.Remove(0, "tag group:".Length);
                LoadDirChilds(dir, ul);
                dirsTree.Add(dir);
            }

            return dirsTree;
        }

        private void LoadDirChilds(Dir dir, HtmlNode node)
        {
            foreach (var child in node.ChildNodes)
            {
                if (child.Name.ToLower() == "li")
                {
                    var aNode = child.ChildNodes.FirstOrDefault(x => x.Name.ToLower() == "a");
                    if (aNode != null)
                    {
                        var newDir = new UI_Dir();
                        if (aNode.InnerText.ToLower().Contains("tag group:"))
                            newDir.Name = aNode.InnerText.Remove(0, "tag group:".Length);
                        else if (aNode.InnerText.ToLower().Contains("list of "))
                            newDir.Name = aNode.InnerText.Remove(0, "list of ".Length);
                        else
                        {
                            newDir.Name = aNode.InnerText;
                            dir.Tags.Add(new UI_Tag(aNode.InnerText, aNode.GetAttributeValue("href", null)));
                        }
                        newDir.Link = aNode.GetAttributeValue("href", null);
                        dir.ChildDirs.Add(newDir);
                    }
                }
                else if (child.Name.ToLower() == "ul")
                {
                    LoadDirChilds(dir.ChildDirs.Last(), child);
                }
                else
                {
                    
                }
            }
        }

        private void LoadTagsForDirTree()
        {
            foreach (var dir in DirTree)//.Where(x => x.Name.ToLower() == "body"))
            {
                LoadTagsForDirTree(dir);
            }
        }

        private void LoadTagsForDirTree(UI_Dir dir)
        {
            if (!string.IsNullOrEmpty(dir.Link))
            {
                var wikibody     = GetWikiBody(dir.Link);
                var nodes        = wikibody.ChildNodes.Where(x => (x.Name.ToLower().StartsWith("h") && x.Name.Length == 2) || x.Name.ToLower() == "ul").ToList();
                var excludeNode = GetExcludeNode(nodes);
                while (excludeNode != null)
                {
                    var pos    = nodes.IndexOf(excludeNode);
                    var endpos = pos + 1;
                    while (endpos < nodes.Count && nodes[endpos].Name.ToLower().StartsWith("h") && nodes[endpos].Name.Length == 2)
                        endpos++;
                    while (endpos < nodes.Count && nodes[endpos].Name.ToLower() == "ul")
                        endpos++;
                    var cnt = endpos - pos;
                    for (var i = 0; i < cnt; i++)
                        nodes.RemoveAt(pos);
                    excludeNode = GetExcludeNode(nodes);
                }

                (UI_Dir dir, int level) currentParent = (dir, 0);
                var parentList    = new List<(UI_Dir dir, int level)> { currentParent };
                foreach (var node in nodes)
                {
                    if (node.Name.ToLower().StartsWith("h"))
                    {
                        var level = int.Parse(node.Name.Substring(1, 1));
                        if (level > currentParent.level)
                        {
                            var newDir = new UI_Dir(node.InnerText);
                            currentParent.dir.ChildDirs.Add(newDir);
                            parentList.Add((newDir, level));
                            currentParent = parentList.Last();
                        }
                        else if (level == currentParent.level)
                        {
                            parentList.Remove(currentParent);
                            currentParent = parentList.Last();
                            var newDir = new UI_Dir(node.InnerText);
                            currentParent.dir.ChildDirs.Add(newDir);
                            parentList.Add((newDir, level));
                            currentParent = parentList.Last();
                        }
                        else
                        {
                            while (level < currentParent.level)
                            {
                                parentList.Remove(currentParent);
                                currentParent = parentList.Last();
                            }
                            var newDir = new UI_Dir(node.InnerText);
                            currentParent.dir.ChildDirs.Add(newDir);
                            parentList.Add((newDir, level));
                            currentParent = parentList.Last();
                        }
                    }
                    else if (node.Name.ToLower() == "ul")
                    {
                        var lis = node.Descendants("li").Where(x => x.ChildNodes.Any(c => c.Name.ToLower() == "a")).ToList();
                        foreach (var x in lis)
                        {
                            foreach (var c in x.ChildNodes.Where(c => c.Name.ToLower() == "a"))
                            {
                                if (!x.InnerText.ToLower().StartsWith("tag group:"))
                                {
                                    currentParent.dir.Tags.Add(new Tag(x.InnerText, c.GetAttributeValue("href", null)));
                                }
                            }
                        }

                    }
                    else throw new Exception();
                    //if (node.Name.ToLower() == "h4")
                    //{
                    //    if (parentList.Count > 1)
                    //    {
                    //        parentList    = parentList.Take(1).ToList();
                    //        currentParent = parentList.Last();
                    //    }

                    //    var newDir = new UI_Dir(node.InnerText);
                    //    currentParent.ChildDirs.Add(newDir);
                    //    currentParent = newDir;
                    //    parentList.Add(currentParent);
                    //}
                    //else if (node.Name.ToLower() == "h6" || node.Name.ToLower() == "h5")
                    //{
                    //    if (parentList.Count > 2)
                    //    {
                    //        parentList    = parentList.Take(2).ToList();
                    //        currentParent = parentList.Last();
                    //    }

                    //    var newDir = new UI_Dir(node.InnerText);
                    //    currentParent.ChildDirs.Add(newDir);
                    //    currentParent = newDir;
                    //    parentList.Add(currentParent);
                    //}
                    //else if (node.Name.ToLower() == "ul")
                    //{
                    //    var lis = node.Descendants("li").Where(x => x.ChildNodes.Any(c => c.Name.ToLower() == "a")).ToList();
                    //    foreach (var x in lis)
                    //    {
                    //        if (x.InnerText.ToLower().StartsWith("tag group:"))
                    //        {
                    //            //var movedDir = FindDirByName(x.InnerText.Remove(0, "tag group:".Length));
                    //            //if (movedDir != null)
                    //            //{
                    //            //    currentParent.dir.ChildDirs.Add(movedDir);
                    //            //    if (movedDir.ParentDir != null)
                    //            //        movedDir.ParentDir.ChildDirs.Remove(movedDir);
                    //            //    movedDir.ParentDir = currentParent.dir;
                    //            //}
                    //            //else
                    //            //{
                    //            //    var newDir = new UI_Dir(x.InnerText.Remove(0, "tag group:".Length));
                    //            //    LoadTagsForDirTree(newDir);
                    //            //    currentParent.dir.ChildDirs.Add(newDir);
                    //            //}
                    //        }
                    //        else
                    //        {
                    //            currentParent.Tags.Add(new Tag(x.InnerText));
                    //        }
                    //    }
                    //}
                    //else throw new Exception();
                }

                foreach (UI_Dir child in dir.ChildDirs)
                {
                    LoadTagsForDirTree(child);
                }
            }
            else
            {
                foreach (UI_Dir child in dir.ChildDirs)
                {
                    LoadTagsForDirTree(child);
                }
            }
        }

        public Dir? FindDirByName(string name)
        {
            return FindDirByName(name.ToLower(), DirTree.OfType<Dir>().ToList());
        }

        private Dir? FindDirByName(string name, List<Dir> dirs)
        {
            foreach (var dir in dirs)
            {
                if (dir.Name.ToLower() == name) return dir;
                var ele = FindDirByName(name, dir.ChildDirs.ToList());
                if (ele != null) return ele;
            }

            return null;
        }

        public static string CombineUrl(string path) => $"{BaseAddress.TrimEnd('/')}/{path.TrimStart('/')}";
        public static HtmlNode? GetExcludeNode(IList<HtmlNode> nodes) => nodes.FirstOrDefault(x => x.Name.ToLower().StartsWith("h") && x.Name.Length == 2 &&
                                                                                                   (x.InnerText.ToLower() == "related tags" || x.InnerText.ToLower() == "Related tag groups".ToLower() || x.InnerText.ToLower() == "see also" || x.InnerText.ToLower() == "External links".ToLower()));
    }
}
