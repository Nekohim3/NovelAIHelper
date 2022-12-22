using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using Avalonia.Controls;
using NovelAIHelper.DataBase;
using NovelAIHelper.DataBase.Entities.DataBase;
using NovelAIHelper.DataBase.Entities.ViewModels;
using NovelAIHelper.Utils;
using NovelAIHelper.Utils.Collections;
using NovelAIHelper.Views;
using System.Collections;
using System.Reactive.Linq;
using System.Runtime.Intrinsics.X86;
using NovelAIHelper.DataBase.Services;
using NovelAIHelper.Utils.DragHelpers;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia;

namespace NovelAIHelper.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private Window _wnd;

    public ReactiveCommand<Unit, Unit> TagEditorShowCmd { get; }

    public ReactiveCommand<Unit, Unit> GroupAddCmd    { get; }
    public ReactiveCommand<Unit, Unit> GroupSaveCmd   { get; }
    public ReactiveCommand<Unit, Unit> GroupCancelCmd { get; }

    public ReactiveCommand<Unit, Unit> SessionAddCmd    { get; }
    public ReactiveCommand<Unit, Unit> SessionEditCmd   { get; }
    public ReactiveCommand<Unit, Unit> SessionRemoveCmd { get; }
    public ReactiveCommand<Unit, Unit> SessionSaveCmd   { get; }
    public ReactiveCommand<Unit, Unit> SessionCancelCmd { get; }

    private bool _sessionsOpen;
    public bool SessionsOpen
    {
        get => _sessionsOpen;
        set => this.RaiseAndSetIfChanged(ref _sessionsOpen, value);
    }

    private bool _groupEditMode;
    public bool GroupEditMode
    {
        get => _groupEditMode;
        set => this.RaiseAndSetIfChanged(ref _groupEditMode, value);
    }

    private UI_Group? _currentGroup;
    public UI_Group? CurrentGroup
    {
        get => _currentGroup;
        set => this.RaiseAndSetIfChanged(ref _currentGroup, value);
    }

    private UI_Session? _currentSession;
    public UI_Session? CurrentSession
    {
        get => _currentSession;
        set => this.RaiseAndSetIfChanged(ref _currentSession, value);
    }

    private bool _sessionEditMode;
    public bool SessionEditMode
    {
        get => _sessionEditMode;
        set => this.RaiseAndSetIfChanged(ref _sessionEditMode, value);
    }

    public MainWindowViewModel()
    {
    }

    public MainWindowViewModel(Window wnd)
    {
        _wnd = wnd;

        GroupAddCmd      = ReactiveCommand.Create(OnGroupAdd);
        GroupSaveCmd     = ReactiveCommand.Create(OnGroupSave);
        GroupCancelCmd   = ReactiveCommand.Create(OnGroupCancel);
        TagEditorShowCmd = ReactiveCommand.Create(OnTagEditorShow);

        SessionAddCmd    = ReactiveCommand.Create(OnSessionAdd);
        SessionEditCmd   = ReactiveCommand.Create(OnSessionEdit);
        SessionRemoveCmd = ReactiveCommand.Create(OnSessionRemove);
        SessionSaveCmd   = ReactiveCommand.Create(OnSessionSave);
        SessionCancelCmd = ReactiveCommand.Create(OnSessionCancel);
    }

    private void OnTagEditorShow()
    {
        var f = new TagEditorView();
        f.DataContext = new TagEditorViewModel(f);
        f.Show(_wnd);
        f.Closed += (_, _) =>
                    {
                        f.DataContext = null;
                        g.TagTree     = new TagTree();
                        GC.Collect();
                        GC.Collect();
                        GC.Collect();
                        GC.Collect();
                    };
    }

    private void OnGroupAdd()
    {
        CurrentGroup  = new UI_Group();
        GroupEditMode = true;
    }

    public void OnGroupEdit(UI_Group group)
    {
    }

    public void OnGroupDelete(UI_Group group)
    {
    }

    private void OnGroupSave()
    {
        if (!GroupEditMode)
            return;
        if (CurrentGroup == null)
            return;
        if (string.IsNullOrEmpty(CurrentGroup.Name))
            return;
        bool saved = false;
        if (CurrentGroup.Id == 0)
        {
            g.TagTree.Sessions.SelectedItem.UI_SessionGroups.Add(CurrentGroup);
            saved = g.TagTree.Sessions.SelectedItem.Save();
        }
        else
        {
            saved = CurrentGroup.Save();
        }
        if (!saved)
            MessageBoxManager.GetMessageBoxStandardWindow("", "Add/Save group failed", ButtonEnum.Ok, Icon.Error).ShowDialog(_wnd);

        CurrentGroup  = null;
        GroupEditMode = false;
    }

    private void OnGroupCancel()
    {
        CurrentGroup  = null;
        GroupEditMode = false;
    }

    #region SessionCmdExec

    private void OnSessionAdd()
    {
        SessionEditMode = true;
        CurrentSession  = new UI_Session();
    }

    private void OnSessionEdit()
    {
        if(g.TagTree.Sessions.SelectedItem == null) return;
        SessionEditMode = true;
        CurrentSession  = g.TagTree.Sessions.SelectedItem.GetCopy();
    }

    private void OnSessionRemove()
    {
    }

    private void OnSessionSave()
    {
        if(CurrentSession == null) return;
        if(string.IsNullOrEmpty(CurrentSession.Name)) return;
        var saved = false;
        if (CurrentSession.Id == 0)
        {
            saved = CurrentSession.Save();
        }
        else
        {
            CurrentSession.CopyTo(g.TagTree.Sessions.SelectedItem);
            if(g.TagTree.Sessions.SelectedItem)
            saved = g.TagTree.Sessions.SelectedItem.Save();
        }
        if(!saved)
            MessageBoxManager.GetMessageBoxStandardWindow("", "Add/Save session failed", ButtonEnum.Ok, Icon.Error).ShowDialog(_wnd);
        CurrentSession  = null;
        SessionEditMode = false;
    }

    private void OnSessionCancel()
    {
        CurrentSession  = null;
        SessionEditMode = false;
    }

    #endregion
}