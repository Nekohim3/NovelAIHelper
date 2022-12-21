//using System.Collections.Generic;
//using NovelAIHelper.DataBase.Entities.ViewModels;
//using NovelAIHelper.Utils.Collections;
//using NovelAIHelper.ViewModels;
//using ReactiveUI;

//namespace NovelAIHelper.Utils;

//public class TagGroup : ViewModelBase
//{
//    private ObservableCollectionWithSelectedItem<UI_Tag> _tagList;

//    public ObservableCollectionWithSelectedItem<UI_Tag> TagList
//    {
//        get => _tagList;
//        set => this.RaiseAndSetIfChanged(ref _tagList, value);
//    }

//    private string _name;

//    public string Name
//    {
//        get => _name;
//        set => this.RaiseAndSetIfChanged(ref _name, value);
//    }

//    public TagGroup(ObservableCollectionWithSelectedItem<UI_Tag> tagList, string name)
//    {
//        _tagList = tagList;
//        _name    = name;
//    }
//}