using MediatR;
using Noma.ApplicationL.Categories.Commands.CreateCategory;
using Noma.ApplicationL.Categories.Commands.DeleteCategory;
using Noma.ApplicationL.Categories.Commands.UpdateCategory;
using Noma.ApplicationL.Categories.Queries.GetCategories;
using Noma.WindowsUI.Common;
using Noma.WindowsUI.Common.Base;
using Noma.WindowsUI.Common.CustomEventArguments;
using Noma.WindowsUI.Common.Interfaces;
using Noma.WindowsUI.Enums;
using Noma.WindowsUI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Noma.WindowsUI.ViewModel
{
    public class CategoriesViewModel: ViewModelBase
    {
        #region Commands
        public RelayCommand LoadedCommand { get; private set; }
        public RelayCommand CreateCategoryCommand { get; set; }
        public RelayCommand<Category> DeleteCategoryCommand { get; set; }
        #endregion

        #region Search
        private string searchInput = string.Empty;
        private MainViewModel parentViewModel;
        public MainViewModel ParentViewModel
        {
            get => parentViewModel;
            set
            {
                if (parentViewModel != value)
                {
                    if (parentViewModel != null)
                        parentViewModel.OnSearchInputChanged -= ParentViewModel_OnSearchInputChanged;

                    parentViewModel = value;
                    parentViewModel.OnSearchInputChanged += ParentViewModel_OnSearchInputChanged;
                }
            }
        }

        private void ParentViewModel_OnSearchInputChanged(object sender, FilterCriteriaChangedEventArgs e)
        {
            searchInput = e.SearchInput;
            BindCategories();
        }

        #endregion

        #region Initialization
        public CategoriesViewModel(IMediator mediator, ISettings userSettings) : base(mediator, userSettings)
        {
            categories = new List<Category>();
            filteredCategories = new ObservableCollection<Category>();

            LoadedCommand = new RelayCommand(OnLoaded);
            CreateCategoryCommand = new RelayCommand(OnCreateCategory);
            DeleteCategoryCommand = new RelayCommand<Category>(OnDeleteCategory);
        }

        private async void OnLoaded()
        {

        }
        #endregion

        #region Categories

        private List<Category> categories;
        public List<Category> Categories
        {
            get => categories;
            set
            {
                categories = value;

                if (categories != null)
                    BindCategories();
            }
        }

        private ObservableCollection<Category> filteredCategories;
        public ObservableCollection<Category> FilteredCategories
        {
            get => filteredCategories;
            set
            {
                SetProperty(ref filteredCategories, value);
            }
        }

        private void BindCategories()
        {
            FilteredCategories = new ObservableCollection<Category>(categories
                .Where(c => c.State != ModelState.Deleted && c.Title
                        .ToLower()
                        .Contains(searchInput.ToLower()))
                        .OrderByDescending(n => n.LastModifiedAt));
        }

        private void OnCreateCategory()
        {
            Category category = new Category()
            {
                Title = "New Category",
                Color = userSettings.DefaultNoteColor32BitArgb,
                State = ModelState.New
            };

            categories.Add(category);

            BindCategories();
        }

        private void OnDeleteCategory(Category category)
        {
            var messageBoxResult = MessageBox.Show("Are you sure you want to delete this category permanently?",
                                                 "Delete category permanently",
                                                 MessageBoxButton.YesNo,
                                                 MessageBoxImage.Question);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                category.State = ModelState.Deleted;

                BindCategories();
            }
        }
        #endregion
    }
}
