using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Noma.ApplicationL.Categories.Queries.GetCategories;
using Noma.ApplicationL.Common.Repositories;
using Noma.ApplicationL.Notes.Commands.CreateNote;
using Noma.ApplicationL.Notes.Commands.DeleteNote;
using Noma.ApplicationL.Notes.Commands.UpdateNote;
using Noma.ApplicationL.Notes.Queries.GetNotes;
using Noma.WindowsUI.Common;
using Noma.WindowsUI.Common.Base;
using Noma.WindowsUI.Common.CustomEventArguments;
using Noma.WindowsUI.Common.Interfaces;
using Noma.WindowsUI.Enums;
using Noma.WindowsUI.Model;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Noma.WindowsUI.ViewModel
{
    public class NotesTrashViewModel : ViewModelBase
    {
        #region Commands
        public RelayCommand LoadedCommand { get; private set; }
        public RelayCommand<Note> RestoreNoteCommand { get; private set; }
        public RelayCommand<Note> DeleteNoteCommand { get; private set; }
        public RelayCommand DeleteAllNotesPermanentlyCommand { get; private set; }
        #endregion

        #region Initialization
        public NotesTrashViewModel(IMediator mediator, ISettings userSettings) : base(mediator, userSettings)
        {
            categories = new List<Category>();
            filteredNotes = new ObservableCollection<Note>();

            LoadedCommand = new RelayCommand(OnLoaded);
            RestoreNoteCommand = new RelayCommand<Note>(OnRestoreNote);
            DeleteNoteCommand = new RelayCommand<Note>(OnDeleteNote);
            DeleteAllNotesPermanentlyCommand = new RelayCommand(OnDeleteAllNotesPermanently);
        }

        private async void OnLoaded()
        {
        }

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
            BindNotes();
        }
        #endregion

        #region Notes & Categories
        private List<Note> notes;
        public List<Note> Notes
        {
            get => notes;
            set
            {
                notes = value;
                if (notes != null)
                    BindNotes();
            }
        }

        private void BindNotes()
        {
            FilteredNotes = new ObservableCollection<Note>(Notes.Where(n =>
            (n.Content.ToLower().Contains(searchInput.ToLower()))
            && n.InTrash
            && n.State != ModelState.Deleted)
                .OrderByDescending(n => n.LastModifiedAt));
        }

        private ObservableCollection<Note> filteredNotes;
        public ObservableCollection<Note> FilteredNotes
        {
            get => filteredNotes;
            set
            {
                SetProperty(ref filteredNotes, value);
            }
        }


        private void OnDeleteNote(Note note)
        {
            note.InTrash = true;
            note.State = ModelState.Deleted;

            BindNotes();
        }

        private void OnRestoreNote(Note note)
        {
            note.InTrash = false;

            BindNotes();
        }

        private void OnDeleteAllNotesPermanently()
        {
            if (FilteredNotes.Count == 0)
                return;

            var messageBoxResult = MessageBox.Show("Are you sure you want to delete all notes permanently?",
                                                "Delete notes permanently",
                                                MessageBoxButton.YesNo,
                                                MessageBoxImage.Question);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                foreach (var note in filteredNotes)
                {
                    note.State = ModelState.Deleted;
                }

                BindNotes();
            }
        }

        private List<Category> categories;
        public List<Category> Categories
        {
            get => categories;
            set
            {
                categories = value;
            }
        }


        #endregion

        #region Common
        public override Task Focused()
        {
            BindNotes();

            return base.Focused();
        }
        #endregion
    }
}
