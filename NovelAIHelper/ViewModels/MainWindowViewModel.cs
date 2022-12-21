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
using System.Runtime.Intrinsics.X86;
using NovelAIHelper.DataBase.Services;
using NovelAIHelper.Utils.DragHelpers;

namespace NovelAIHelper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Window                      _wnd;
        public  ReactiveCommand<Unit, Unit> TagEditorShowCmd { get; }

        //private TagTree _tagTree = new();

        //public TagTree TagTree
        //{
        //    get => _tagTree;
        //    set => this.RaiseAndSetIfChanged(ref _tagTree, value);
        //}

        //private ObservableCollectionWithSelectedItem<UI_Tag> _firstList;

        //public ObservableCollectionWithSelectedItem<UI_Tag> FirstList
        //{
        //    get => _firstList;
        //    set => this.RaiseAndSetIfChanged(ref _firstList, value);
        //}

        //private ObservableCollectionWithSelectedItem<UI_Tag> _secondList;

        //public ObservableCollectionWithSelectedItem<UI_Tag> SecondList
        //{
        //    get => _secondList;
        //    set => this.RaiseAndSetIfChanged(ref _secondList, value);
        //}

        //private ObservableCollectionWithSelectedItem<UI_Tag> _thirdList;

        //public ObservableCollectionWithSelectedItem<UI_Tag> ThirdList
        //{
        //    get => _thirdList;
        //    set => this.RaiseAndSetIfChanged(ref _thirdList, value);
        //}
        
        //private UI_Tag _draggedTag;

        //public UI_Tag DraggedTag
        //{
        //    get => _draggedTag;
        //    set => this.RaiseAndSetIfChanged(ref _draggedTag, value);
        //}

        //private ObservableCollectionWithSelectedItem<UI_Tag> _sourceDragList;

        //public ObservableCollectionWithSelectedItem<UI_Tag> SourceDragList
        //{
        //    get => _sourceDragList;
        //    set => this.RaiseAndSetIfChanged(ref _sourceDragList, value);
        //}

        //private UI_SessionPart? _sourceDragGroup;

        //public UI_SessionPart? SourceDragGroup
        //{
        //    get => _sourceDragGroup;
        //    set => this.RaiseAndSetIfChanged(ref _sourceDragGroup, value);
        //}

        //private UI_SessionPart? _lastGroup;

        //public UI_SessionPart? LastGroup
        //{
        //    get => _lastGroup;
        //    set => this.RaiseAndSetIfChanged(ref _lastGroup, value);
        //}


        //private DragObject _dragObject;

        //private TagGridViewModel _tagGroupVM;

        //public TagGridViewModel TagGroupVM
        //{
        //    get => _tagGroupVM;
        //    set => this.RaiseAndSetIfChanged(ref _tagGroupVM, value);
        //}

        public ReactiveCommand<Unit, Unit> AddGroupCmd { get; }
        public ReactiveCommand<Unit, Unit> CancelGroupCmd        { get; }
        
        public MainWindowViewModel()
        {
        }

        public MainWindowViewModel(Window wnd)
        {
            //var sess = new UI_Session();
            //sess.Name        = "qwe";
            //sess.Comment     = "asd";
            //sess.Save();
            //sess.Name        = "123";
            //sess.Save();
            AddGroupCmd      = ReactiveCommand.Create(OnAddGroup);
            CancelGroupCmd   = ReactiveCommand.Create(OnCancelGroup);
            _wnd             = wnd;
            TagEditorShowCmd = ReactiveCommand.Create(OnTagEditorShow);
            //_firstList       = new ObservableCollectionWithSelectedItem<UI_Tag>(TagTree.GetRange(10000, 10));
            //_secondList      = new ObservableCollectionWithSelectedItem<UI_Tag>(TagTree.GetRange(20000, 10));
            //_thirdList       = new ObservableCollectionWithSelectedItem<UI_Tag>(TagTree.GetRange(30000, 10));
            //TagGroupVM = new TagGridViewModel(new ObservableCollectionWithSelectedItem<TagGroup>(
            //                                                                                     new List<TagGroup>
            //                                                                                     {
            //                                                                                         //new(_firstList, "Тело"),
            //                                                                                         //new(_secondList, "Одежда"),
            //                                                                                         //new(_thirdList, "Поза"),

            //                                                                                     }
            //                                                                                    ));
        }

        private void OnTagEditorShow()
        {
            var f = new TagEditorView();
            f.DataContext = new TagEditorViewModel(f);
            f.Show(_wnd);
            f.Closed += (_, _) =>
                        {
                            f.DataContext = null;
                            g.TagTree       = new TagTree();
                            GC.Collect();
                        };
        }

        private void OnAddGroup()
        {

        }

        private void OnCancelGroup()
        {

        }

        //public void DragStart(UI_SessionPart? group, UI_Tag tag, DragObject dragObject)
        //{
        //    SourceDragGroup = group;
        //    DraggedTag = tag;
        //    LastGroup = SourceDragGroup;
        //    DraggedTag.IsDrag = true;
        //    _dragObject = dragObject;
        //}

        //public void DragStart(UI_SessionPart group, DragObject dragObject)
        //{
        //    SourceDragGroup = group;
        //    _dragObject = dragObject;
        //}

        //public void DragEnd()
        //{
        //    SourceDragGroup = null;
        //    if (DraggedTag == null) return;
        //    DraggedTag.IsDrag = false;
        //    DraggedTag = null;
        //}

        //public void DragOver(UI_SessionPart? group, UI_Tag? tag = null)
        //{
        //    if (_dragObject is DragObject.Tag or DragObject.SearchedTag)
        //    {
        //        if (DraggedTag == null) return;
        //        if (group == null) return;
        //        if (tag == null)
        //        {
        //            if (LastGroup == null)
        //            {
        //                LastGroup = group;
        //                if (group.UI_PartTags.Count(x => x.UI_Tag == DraggedTag) == 0)
        //                    group.UI_PartTags.Add(new UI_PartTag(DraggedTag, group.UI_PartTags.Count + 1));
        //            }
        //            else
        //            {
        //                if (LastGroup != group)
        //                {
        //                    foreach (var x in TagTree.Sessions.SelectedItem.UI_SessionParts)
        //                        x.UI_PartTags.Remove(x.UI_PartTags.FirstOrDefault(c => c.IdTag == DraggedTag.Id));
        //                    LastGroup = group;
        //                    group.UI_PartTags.Add(new UI_PartTag(DraggedTag, group.UI_PartTags.Count + 1));
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (DraggedTag == tag) return;
        //            if (LastGroup == null)
        //            {
        //                foreach (var x in TagTree.Sessions.SelectedItem.UI_SessionParts)
        //                    x.UI_PartTags.Remove(x.UI_PartTags.FirstOrDefault(c => c.IdTag == DraggedTag.Id));
        //                LastGroup = group;
        //                if (group.UI_PartTags.Count(x => x.UI_Tag == DraggedTag) == 0)
        //                    group.UI_PartTags.Add(new UI_PartTag(DraggedTag, group.UI_PartTags.Count + 1));
        //            }
        //            else if (LastGroup == group)
        //            {
        //                if (group.UI_PartTags.Count(x => x.UI_Tag == DraggedTag) == 0)
        //                    group.UI_PartTags.Insert(group.UI_PartTags.IndexOf(group.UI_PartTags.FirstOrDefault(x => x.IdTag == tag.Id)), new UI_PartTag(DraggedTag, group.UI_PartTags.Count + 1));
        //                else
        //                    group.UI_PartTags.Move(group.UI_PartTags.IndexOf(group.UI_PartTags.FirstOrDefault(x => x.IdTag == DraggedTag.Id)), group.UI_PartTags.IndexOf(group.UI_PartTags.FirstOrDefault(x => x.IdTag == tag.Id)));
        //            }
        //            else
        //            {
        //                foreach (var x in TagTree.Sessions.SelectedItem.UI_SessionParts)
        //                    x.UI_PartTags.Remove(x.UI_PartTags.FirstOrDefault(c => c.IdTag == DraggedTag.Id));
        //                LastGroup = group;
        //                group.UI_PartTags.Insert(group.UI_PartTags.IndexOf(group.UI_PartTags.FirstOrDefault(x => x.IdTag == tag.Id)), group.UI_PartTags.FirstOrDefault(x => x.IdTag == DraggedTag.Id));
        //            }
        //        }
        //    }
        //    else if (_dragObject == DragObject.Group)
        //    {
        //        if (group == null || SourceDragGroup == null) return;
        //        if (SourceDragGroup != group)
        //            TagTree.Sessions.SelectedItem.UI_SessionParts.Move(TagTree.Sessions.SelectedItem.UI_SessionParts.IndexOf(SourceDragGroup), TagTree.Sessions.SelectedItem.UI_SessionParts.IndexOf(group));
        //    }
        //}
    }
}

