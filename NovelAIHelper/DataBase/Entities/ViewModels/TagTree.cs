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
    public TagTree()
    {
        LoadTree();
    }

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

    private List<UI_Dir> _dirList     = new();
    private List<UI_Tag> _tagList     = new();
    private List<int>    _expandedIds = new();
    private int          _selectedId;

    public void LoadTree(bool remember = false)
    {
        if (remember)
            RememberExpandedAndSelected();
        RootDirs.Clear();
        var dirs = new DirService().GetAllDirs().ToList();
        _dirList = dirs;
        var tags = new TagService().GetAllTags().ToList();
        _tagList = tags;
        AssignTagsToDirs(dirs, tags);
        BuildTree(dirs);
        if(remember)
            RestoreExpandedAndSelected();
    }

    public void RememberExpandedAndSelected()
    {
        _expandedIds = _dirList.Where(x => x.IsExpanded && x.Id > 0).Select(x => x.Id).ToList();
        _selectedId  = RootDirs.SelectedItem?.Id ?? 0;
    }

    public void RestoreExpandedAndSelected()
    {
        foreach (var x in _dirList.Where(x => _expandedIds.Contains(x.Id)))
            x.IsExpanded = true;
        RootDirs.SelectedItem = _dirList.FirstOrDefault(x => x.Id == _selectedId);
    }

    private void AssignTagsToDirs(List<UI_Dir> dirs, List<UI_Tag> tags)
    {
        foreach (var tag in tags)
        foreach (var id in tag.Dirs.Select(x => x.Id))
        {
            var dir = dirs.FirstOrDefault(x => x.Id == id);
            if (dir != null)
            {
                dir.UI_Tags.Add(tag);
                tag.UI_Dirs.Add(dir);
            }
        }
    }

    private void BuildTree(List<UI_Dir> dirs)
    {
        RootDirs.AddRange(dirs.Where(x => !x.ParentId.HasValue));
        foreach (var x in RootDirs)
            FillChilds(dirs, x);
    }

    private void FillChilds(List<UI_Dir> dirs, UI_Dir dir)
    {
        dir.UI_Childs.AddRange(dirs.Where(x => x.ParentId.GetValueOrDefault() == dir.Id));
        foreach (var x in dir.UI_Childs)
            FillChilds(dirs, x);
    }

    public void Search(string name)
    {
        SearchedTags.Clear();
        if(!string.IsNullOrEmpty(name.Trim(' ')))
            SearchedTags.AddRange(_tagList.Where(x => x.Name.ToLower().Contains(name.Trim(' ').ToLower())));
    }
}
