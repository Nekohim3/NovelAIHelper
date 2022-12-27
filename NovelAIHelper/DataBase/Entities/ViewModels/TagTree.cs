using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.Execution;
using Avalonia.Threading;
using AvaloniaEdit.Utils;
using NovelAIHelper.DataBase.Services;
using NovelAIHelper.Utils.Collections;
using NovelAIHelper.Utils.DragHelpers;
using NovelAIHelper.ViewModels;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.ViewModels;

public class TagTree : ViewModelBase
{
    private ObservableCollectionWithSelectedItem<UI_Dir> _rootDirs = new();

    public ObservableCollectionWithSelectedItem<UI_Dir> RootDirs
    {
        get => _rootDirs;
        set => this.RaiseAndSetIfChanged(ref _rootDirs, value);
    }

    private ObservableCollectionWithSelectedItem<UI_Tag> _tags = new();

    public ObservableCollectionWithSelectedItem<UI_Tag> Tags
    {
        get => _tags;
        set => this.RaiseAndSetIfChanged(ref _tags, value);
    }

    private ObservableCollectionWithSelectedItem<UI_Tag> _searchedTags = new();

    public ObservableCollectionWithSelectedItem<UI_Tag> SearchedTags
    {
        get => _searchedTags;
        set => this.RaiseAndSetIfChanged(ref _searchedTags, value);
    }

    private ObservableCollectionWithSelectedItem<UI_Session> _sessions = new();

    public ObservableCollectionWithSelectedItem<UI_Session> Sessions
    {
        get => _sessions;
        set => this.RaiseAndSetIfChanged(ref _sessions, value);
    }

    private UI_Group? _sourceDragGroup;

    public UI_Group? SourceDragGroup
    {
        get => _sourceDragGroup;
        set => this.RaiseAndSetIfChanged(ref _sourceDragGroup, value);
    }

    private UI_Group? _lastGroup;

    public UI_Group? LastGroup
    {
        get => _lastGroup;
        set => this.RaiseAndSetIfChanged(ref _lastGroup, value);
    }

    private UI_Tag? _draggedTag;

    public UI_Tag? DraggedTag
    {
        get => _draggedTag;
        set => this.RaiseAndSetIfChanged(ref _draggedTag, value);
    }

    private UI_GroupTag? _draggedGroupTag;
    public UI_GroupTag? DraggedGroupTag
    {
        get => _draggedGroupTag;
        set => this.RaiseAndSetIfChanged(ref _draggedGroupTag, value);
    }

    private string _searchString = "";

    public string SearchString
    {
        get => _searchString;
        set
        {
            this.RaiseAndSetIfChanged(ref _searchString, value);
            Search(_searchString);
        }
    }

    private List<UI_Dir>      _dirList     = new();
    private List<UI_Tag>      _tagList     = new();
    private List<UI_Session>  _sessionList = new();
    private List<UI_Group>    _groupList   = new();
    private List<UI_GroupTag> _groupTagList    = new();
    private List<int>         _expandedIds = new();
    private int               _selectedId;
    private int _selectedSessionId;
    private DragObject        _dragObject;

    public TagTree()
    {
        LoadTree();
    }

    #region Loader

    public void LoadDirs(bool remember = false)
    {
        if (remember) RememberExpandedAndSelectedDirsTags();
        RootDirs.Clear();
        SearchedTags.Clear();
        Tags.Clear();
        _tagList = new TagService().GetAll();
        _dirList = new DirService().GetAll();
        AssignTagsToDirs(_dirList, _tagList);
        BuildTree(_dirList);
        if (remember) RestoreExpandedAndSelectedDirsTags();
    }

    public void LoadSessions(bool remember = false)
    {
        AdjustOrders();
        if (remember) RememberSelectedSession();
        Sessions.Clear();
        _sessionList = new SessionService().GetAll();
        _groupList   = new GroupService().GetAll().OrderBy(x => x.Order).ToList();
        _groupTagList    = new GroupTagService().GetAll().OrderBy(x => x.Order).ToList();
        BuildSessions(_sessionList);
        if (remember) RestoreSelectedSession();
    }

    public void LoadTree(bool remember = false)
    {
        LoadDirs(remember);
        LoadSessions(remember);
    }

    public void RememberExpandedAndSelectedDirsTags()
    {
        _expandedIds = _dirList.Where(x => x.IsExpanded && x.Id > 0).Select(x => x.Id).ToList();
        _selectedId  = RootDirs.SelectedItem?.Id ?? 0;
    }

    public void RestoreExpandedAndSelectedDirsTags()
    {
        foreach (var x in _dirList.Where(x => _expandedIds.Contains(x.Id))) x.IsExpanded = true;
        RootDirs.SelectedItem = _dirList.FirstOrDefault(x => x.Id     == _selectedId);
    }

    public void RememberSelectedSession()
    {
        _selectedSessionId = Sessions.SelectedItem?.Id ?? 0;
    }

    public void RestoreSelectedSession()
    {
        Sessions.SelectedItem = _sessionList.FirstOrDefault(x => x.Id == _selectedSessionId);
    }

    private void AssignTagsToDirs(List<UI_Dir> dirs, List<UI_Tag> tags)
    {
        foreach (var tag in tags)
        {
            var dir = dirs.FirstOrDefault(x => x.Id == tag.IdDir);
            if (dir != null)
            {
                tag.UI_Dir = dir;
                dir.UI_Tags.Add(tag);
            }
        }
    }

    private void BuildTree(List<UI_Dir> dirs)
    {
        RootDirs.AddRange(dirs.Where(x => !x.IdParent.HasValue));
        foreach (var x in RootDirs)
            FillChilds(dirs, x);
    }

    private void FillChilds(List<UI_Dir> dirs, UI_Dir dir)
    {
        dir.UI_Childs.AddRange(dirs.Where(x => x.IdParent.GetValueOrDefault() == dir.Id));
        foreach (var x in dir.UI_Childs)
        {
            x.UI_Parent = dir;
            FillChilds(dirs, x);
        }
    }

    private void BuildSessions(List<UI_Session> sessionList)
    {
        Sessions.AddRange(sessionList);
        foreach (var x in Sessions)
        {
            x.UI_SessionGroups.AddRange(_groupList.Where(c => c.IdSession == x.Id));
            foreach (var c in x.UI_SessionGroups)
            {
                c.UI_GroupTags.AddRange(_groupTagList.Where(v => v.IdGroup == c.Id));
                foreach (var v in c.UI_GroupTags) v.UI_Tag = _tagList.FirstOrDefault(b => b.Id == v.IdTag);
            }
        }
    }

    #endregion

    #region Drag

    public void DragStart(UI_Group? group, UI_Tag tag, DragObject dragObject)
    {
        if (g.TagTree.Sessions.SelectedItem == null)
            return;
        SourceDragGroup = group != null ? g.TagTree._groupList.FirstOrDefault(x => x.Id == group.Id) : null;
        DraggedTag      = g.TagTree._tagList.FirstOrDefault(x => x.Id == tag.Id);
        var dgt = g.TagTree._groupTagList.FirstOrDefault(_ => _.IdTag == DraggedTag.Id);
        DraggedGroupTag   = dgt == null ? new UI_GroupTag(tag) : dgt;
        LastGroup         = SourceDragGroup;
        DraggedTag.IsDrag = true;
        _dragObject       = dragObject;
    }

    public void DragStart(UI_Group group, DragObject dragObject)
    {
        SourceDragGroup = group != null ? g.TagTree._groupList.FirstOrDefault(x => x.Id == group.Id) : null;
        _dragObject     = dragObject;
    }

    public void DragEnd()
    {
        if (LastGroup != null && DraggedGroupTag != null && DraggedGroupTag.Id != 0)
        {
            DraggedGroupTag.IdGroup = LastGroup.Id;
            DraggedGroupTag.Group = LastGroup;
            DraggedGroupTag.Save();
        }

        AdjustOrders();
        if (g.TagTree._groupTagList.Count(_ => _.Id == DraggedGroupTag.Id) == 0)
            g.TagTree._groupTagList.Add(DraggedGroupTag);
        SourceDragGroup = null;
        if (DraggedTag == null) return;
        DraggedTag.IsDrag = false;
        DraggedTag        = null;
        
        //todo: save
    }

    public void DragOver(UI_Group? group, UI_Tag? tag = null)
    {
        if (_dragObject is DragObject.SearchedTag or DragObject.Tag)
        {
            if(DraggedTag == null || group == null) return;
            if (tag == null)
            {
                if (LastGroup == null)
                {
                    LastGroup = group;
                    if (group.UI_GroupTags.All(x => x.IdTag != DraggedTag.Id))
                        group.UI_GroupTags.Add(DraggedGroupTag);
                }
                else
                {
                    if (LastGroup != group)
                    {
                        foreach (var x in Sessions.SelectedItem.UI_SessionGroups)
                            x.UI_GroupTags.Remove(DraggedGroupTag);
                        LastGroup = group;
                        group.UI_GroupTags.Add(DraggedGroupTag);
                    }
                }
            }
            else
            {
                if (DraggedTag == tag) return;
                if (LastGroup == null)
                {
                    foreach (var x in Sessions.SelectedItem.UI_SessionGroups)
                        x.UI_GroupTags.Remove(DraggedGroupTag);
                    LastGroup = group; 
                    if (group.UI_GroupTags.All(x => x.IdTag != DraggedTag.Id))
                        group.UI_GroupTags.Add(DraggedGroupTag);
                }
                else if(LastGroup == group)
                {
                    if (group.UI_GroupTags.All(x => x.IdTag != DraggedTag.Id))
                        group.UI_GroupTags.Insert(group.UI_GroupTags.IndexOf(group.UI_GroupTags.FirstOrDefault(x => x.IdTag == tag.Id)), DraggedGroupTag);
                    else
                        group.UI_GroupTags.Move(group.UI_GroupTags.IndexOf(group.UI_GroupTags.FirstOrDefault(x => x.IdTag == DraggedTag.Id)), group.UI_GroupTags.IndexOf(group.UI_GroupTags.FirstOrDefault(x => x.IdTag == tag.Id)));
                }
                else
                {
                    foreach (var x in Sessions.SelectedItem.UI_SessionGroups)
                        x.UI_GroupTags.Remove(x.UI_GroupTags.FirstOrDefault(c => c.IdTag == DraggedTag.Id));
                    LastGroup = group; 
                    group.UI_GroupTags.Insert(group.UI_GroupTags.IndexOf(group.UI_GroupTags.FirstOrDefault(x => x.IdTag == tag.Id)), DraggedGroupTag);
                }
            }
        }
        else if (_dragObject == DragObject.Group)
        {
            if (group == null || SourceDragGroup == null) return;
            if (SourceDragGroup != group)
                Sessions.SelectedItem.UI_SessionGroups.Move(Sessions.SelectedItem.UI_SessionGroups.IndexOf(SourceDragGroup), Sessions.SelectedItem.UI_SessionGroups.IndexOf(group));
        }
    }

    #endregion

    public void AdjustOrders()
    {
        if (Sessions.SelectedItem != null)
        {
            var lstGroups    = new List<UI_Group>();
            var lstGroupTags = new List<UI_GroupTag>();
            for (var i = 0; i < Sessions.SelectedItem.UI_SessionGroups.Count; i++)
            {
                Sessions.SelectedItem.UI_SessionGroups[i].Order = i;
                lstGroups.Add(Sessions.SelectedItem.UI_SessionGroups[i]);
                for (var j = 0; j < Sessions.SelectedItem.UI_SessionGroups[i].UI_GroupTags.Count; j++)
                {
                    Sessions.SelectedItem.UI_SessionGroups[i].UI_GroupTags[j].Order = j;
                    lstGroupTags.Add(Sessions.SelectedItem.UI_SessionGroups[i].UI_GroupTags[j]);
                }
            }
            new GroupService().SaveRange(lstGroups);
            new GroupTagService().SaveRange(lstGroupTags);

        }
        else
        {
            foreach (var x in Sessions)
            {
                var lstGroups    = new List<UI_Group>();
                var lstGroupTags = new List<UI_GroupTag>();
                for (var i = 0; i < x.UI_SessionGroups.Count; i++)
                {
                    x.UI_SessionGroups[i].Order = i;
                    lstGroups.Add(x.UI_SessionGroups[i]);
                    for (var j = 0; j < x.UI_SessionGroups[i].UI_GroupTags.Count; j++)
                    {
                        x.UI_SessionGroups[i].UI_GroupTags[j].Order = j;
                        lstGroupTags.Add(x.UI_SessionGroups[i].UI_GroupTags[j]);
                    }
                }
                new GroupService().SaveRange(lstGroups);
                new GroupTagService().SaveRange(lstGroupTags);
            }
        }
    }

    public void Search(string name)
    {
        SearchedTags.Clear();
        if (!string.IsNullOrEmpty(name.Trim(' '))) SearchedTags.AddRange(_tagList.Where(x => x.Name.ToLower().Contains(name.Trim(' ').ToLower())));
    }

    public List<UI_Tag> GetRange(int startIndex, int length)
    {
        return _tagList.Skip(startIndex).Take(length).ToList();
    }
}
