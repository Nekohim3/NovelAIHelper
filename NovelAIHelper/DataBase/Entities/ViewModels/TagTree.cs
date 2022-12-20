using System.Collections.Generic;
using System.Linq;
using Avalonia.Threading;
using AvaloniaEdit.Utils;
using NovelAIHelper.DataBase.Services;
using NovelAIHelper.Utils.Collections;
using NovelAIHelper.ViewModels;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.ViewModels;

public class TagTree : ViewModelBase
{
    public TagTree() { LoadTree(); }

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

    private string _searchString;

    public string SearchString
    {
        get => _searchString;
        set
        {
            this.RaiseAndSetIfChanged(ref _searchString, value);
            Search(_searchString);
        }
    }

    private List<UI_Dir>         _dirList         = new();
    private List<UI_Tag>         _tagList         = new();
    private List<UI_Session>     _sessionList     = new();
    private List<UI_SessionPart> _sessionPartList = new();
    private List<UI_PartTag>     _partTags        = new();
    private List<int>            _expandedIds     = new();
    private int                  _selectedId;

    public void LoadTree(bool remember = false)
    {
        if (remember) RememberExpandedAndSelected();
        RootDirs.Clear();
        Sessions.Clear();
        _dirList         = new DirService().GetAll().ToList();
        _tagList         = new TagService().GetAll().ToList();
        _sessionList     = new SessionService().GetAll().ToList();
        _sessionPartList = new SessionPartService().GetAll().ToList();
        _partTags        = new PartTagService().GetAll().ToList();

        AssignTagsToDirs(_dirList, _tagList);
        BuildTree(_dirList);
        BuildSessions(_sessionList);
        if (remember) RestoreExpandedAndSelected();
    }

    public void RememberExpandedAndSelected()
    {
        _expandedIds = _dirList.Where(x => x.IsExpanded && x.Id > 0).Select(x => x.Id).ToList();
        _selectedId  = RootDirs.SelectedItem?.Id ?? 0;
    }

    public void RestoreExpandedAndSelected()
    {
        foreach (var x in _dirList.Where(x => _expandedIds.Contains(x.Id))) x.IsExpanded = true;
        RootDirs.SelectedItem = _dirList.FirstOrDefault(x => x.Id == _selectedId);
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
        foreach (var x in RootDirs) FillChilds(dirs, x);
    }

    private void FillChilds(List<UI_Dir> dirs, UI_Dir dir)
    {
        dir.UI_Childs.AddRange(dirs.Where(x => x.IdParent.GetValueOrDefault() == dir.Id));
        foreach (var x in dir.UI_Childs) FillChilds(dirs, x);
    }

    private void BuildSessions(List<UI_Session> sessionList)
    {
        Sessions.AddRange(sessionList);
        foreach (var x in Sessions)
        {
            x.UI_SessionParts.AddRange(_sessionPartList.Where(c => c.IdSession == x.Id));
            foreach (var c in x.UI_SessionParts)
            {
                c.UI_PartTags.AddRange(_partTags.Where(v => v.IdPart == c.Id));
                foreach (var v in c.UI_PartTags)
                {
                    v.UI_Tag = _tagList.FirstOrDefault(b => b.Id == v.IdTag);
                }
            }
        }
    }

    public void Search(string name)
    {
        SearchedTags.Clear();
        if (!string.IsNullOrEmpty(name.Trim(' '))) SearchedTags.AddRange(_tagList.Where(x => x.Name.ToLower().Contains(name.Trim(' ').ToLower())));
    }

    public List<UI_Tag> GetRange(int startIndex, int length) => _tagList.Skip(startIndex).Take(length).ToList();
}