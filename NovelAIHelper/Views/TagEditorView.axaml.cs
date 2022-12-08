using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using NovelAIHelper.DataBase.Entities.ViewModels;
using NovelAIHelper.ViewModels;

namespace NovelAIHelper.Views
{
    public partial class TagEditorView : Window
    {
        public TagEditorView()
        {
            InitializeComponent();
        }

        private void ListBoxTags_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var vm = DataContext as TagEditorViewModel;
            vm.TagsOnSelectionChanged((e.AddedItems as object[]).OfType<UI_Tag>().FirstOrDefault(), (e.RemovedItems as object[]).OfType<UI_Tag>().FirstOrDefault());
        }
    }
}
