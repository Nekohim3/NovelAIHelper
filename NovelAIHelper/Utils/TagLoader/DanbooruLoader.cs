using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public        List<UI_Tag> TagList     = new List<UI_Tag>();
        public        List<UI_Dir> DirList     = new List<UI_Dir>();

        public DanbooruLoader()
        {

        }

        public bool SaveDirs(List<UI_Dir> dirsTree)
        {
            var service = new DirService();
            return service.AddRange(dirsTree);
        }

        private IEnumerable<HtmlNode> DownloadUls()
        {
            var html = new WebClient().DownloadString(CombineUrl("wiki_pages/tag_groups"));
            var doc  = new HtmlDocument();
            doc.LoadHtml(html);
            var wikibody = doc.GetElementbyId("wiki-page-body");
            return wikibody.ChildNodes.Where(x => x.Name.ToLower() == "ul");
        }

        public List<UI_Dir> DownloadDirs()
        {
            var uls  = DownloadUls();
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

        //private UI_Dir? DownloadDirTree(HtmlNode dirNode)
        //{
        //    //    else if (child.Name.ToLower() == "ul")
        //    //    {
        //    //        dir.ChildDirs.Last().ChildDirs ??= new List<Dir>();
        //    //        foreach (var x in DownloadDirTree(child).ChildDirs)
        //    //            dir.ChildDirs.Last().ChildDirs.Add(x);
        //    //    }
        //    //    else
        //    //    {
        //    //        throw new Exception();
        //    //    }
        //    //}

        //    return dir;
        //}

        public static string CombineUrl(string path) => $"{BaseAddress.TrimEnd('/')}/{path.TrimStart('/')}";
    }
}
