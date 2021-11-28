using MediatR;
using Noma.WindowsUI.Common;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Noma.ApplicationL.Notes.Commands.CreateNote;
using Noma.ApplicationL.Notes.Commands.UpdateNote;
using Noma.ApplicationL.Notes.Commands.DeleteNote;
using Noma.ApplicationL.Notes.Queries.GetNotes;
using Noma.WindowsUI.Model;
using Noma.WindowsUI.Enums;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Noma.WindowsUI.Common.CustomEventArguments;
using Microsoft.Extensions.DependencyInjection;
using Noma.WindowsUI.View;
using Noma.WindowsUI.Common.Interfaces;
using Noma.WindowsUI.Common.Base;
using Noma.ApplicationL.Categories.Queries.GetCategories;
using System.Windows.Media;
using Noma.WindowsUI.Common.Helpers;
using Noma.ApplicationL.Common.Repositories;

namespace Noma.WindowsUI.ViewModel
{
    public class NotesViewModel: ViewModelBase
    {
        #region Commands
        public RelayCommand LoadedCommand { get; private set; }
        public RelayCommand CreateNoteCommand { get; private set; }
        public RelayCommand<Note> DeleteNoteCommand { get; private set; }
        public RelayCommand<Note> MoveNoteToTrashCommand { get; private set; }
        public RelayCommand<Note> ExpandNoteCommand { get; private set; }
        #endregion

        #region Initialization
        public NotesViewModel(IMediator mediator, ISettings userSettings) : base(mediator, userSettings)
        {
            categories = new List<Category>();
            //notes = new List<Note>();
            filteredNotes = new ObservableCollection<Note>();

            LoadedCommand = new RelayCommand(OnLoaded);
            CreateNoteCommand = new RelayCommand(OnCreateNote);
            DeleteNoteCommand = new RelayCommand<Note>(OnDeleteNote);
            MoveNoteToTrashCommand = new RelayCommand<Note>(OnMoveNoteToTrash);
            ExpandNoteCommand = new RelayCommand<Note>(OnExpandNote);
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
                if(parentViewModel != value)
                {
                    if(parentViewModel != null)
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

                if(notes != null)
                    BindNotes();
            }
        }

        private ObservableCollection<Note> filteredNotes;
        public ObservableCollection<Note> FilteredNotes
        {
            get => filteredNotes;
            private set
            {
                SetProperty(ref filteredNotes, value);
            }
        }


        private void BindNotes()
        {
            FilteredNotes = new ObservableCollection<Note>(Notes.Where(n =>
            (n.Content.ToLower().Contains(searchInput.ToLower()))
            && !n.InTrash
            && n.State != ModelState.Deleted)
                .OrderByDescending(n => n.LastModifiedAt));
        } 

        private void OnCreateNote()
        {
            Category defaultCategory = Categories.FirstOrDefault(c => c.IsDefault);

            Note note = new Note(categories)
            {
                Content = string.Empty,
                InTrash = false,
                CategoryId = defaultCategory?.Id ?? null,
                BackgroundColor = defaultCategory?.Color ?? userSettings.DefaultNoteColor32BitArgb,
                State = ModelState.New
            };

            this.Notes.Add(note);

            BindNotes();
        }

        private void OnMoveNoteToTrash(Note note)
        {
            note.InTrash = true;

            CloseNote(note);
            BindNotes();
        }

        private void OnDeleteNote(Note note)
        {
            var messageBoxResult = MessageBox.Show("Are you sure you want to delete this note permanently?",
                                                "Delete note permanently",
                                                MessageBoxButton.YesNo,
                                                MessageBoxImage.Question);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                note.InTrash = true;
                note.State = ModelState.Deleted;

                CloseNote(note);
                BindNotes();
            }
        }

        private void OnExpandNote(Note note)
        {
            ExpandNote(note);
        }

        readonly List<Window> noteWindows = new List<Window>();
        private void ExpandNote(Note note)
        {
            foreach (var window in noteWindows)
            {
                if (window?.DataContext is NoteViewModel _noteViewModel && _noteViewModel.Note == note)
                {
                    window.Activate();
                    return;
                }
            }

            var noteViewModel = Services.GetRequiredService<NoteViewModel>();
            noteViewModel.Note = note;
            noteViewModel.Categories = categories;

            noteViewModel.OnNoteDeleted += (o, e) => BindNotes();
            noteViewModel.OnNoteMovedToTrash += (o, e) => BindNotes();

            var noteWindow = new NoteView(noteViewModel)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            noteWindow.Closing += (o, e) => noteWindows.Remove(o as Window);
            noteWindows.Add(noteWindow);
            noteWindow.Show();
        }

        private void CloseNote(Note note)
        {
            var noteWindow = noteWindows.FirstOrDefault(nw => (nw.DataContext as NoteViewModel)?.Note == note);

            if (noteWindow != null)
            {
                noteWindow.Close();
                noteWindows.Remove(noteWindow);
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

        /*private async Task LoadCategories()
        {
            GetCategoriesQuery query = new GetCategoriesQuery();
            var categoryDTOS = await mediator.Send(query);

            categories.Clear();

            foreach (var categoryDTO in categoryDTOS)
            {
                categories.Add(new Category()
                {
                    Id = categoryDTO.Id,
                    Title = categoryDTO.Title,
                    Color = categoryDTO.Color,
                    IsDefault = categoryDTO.IsDefault
                });
            }
        }*/

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


