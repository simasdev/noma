using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Noma.WindowsUI.Common;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Noma.WindowsUI.Common.CustomEventArguments;
using Noma.WindowsUI.View;
using Noma.WindowsUI.Common.Interfaces;
using Noma.WindowsUI.Common.Base;
using Noma.ApplicationL.Notes.Queries.GetNotes;
using Noma.WindowsUI.Model;
using Noma.ApplicationL.Common.Repositories;
using Noma.WindowsUI.Enums;
using System.Linq;
using Noma.ApplicationL.Notes.Commands.CreateNote;
using Noma.ApplicationL.Notes.Commands.UpdateNote;
using Noma.ApplicationL.Notes.Commands.DeleteNote;
using System.Collections.ObjectModel;
using Noma.ApplicationL.Categories.Queries.GetCategories;
using Noma.ApplicationL.Categories.Commands.CreateCategory;
using Noma.ApplicationL.Categories.Commands.UpdateCategory;
using Noma.ApplicationL.Categories.Commands.DeleteCategory;
using AutoMapper;
using System.Net;
using System.Threading;
using System.IO;
using Noma.ApplicationL.Common.Interfaces;

namespace Noma.WindowsUI.ViewModel
{
    public class MainViewModel: ViewModelBase
    {
        #region View Models
        private readonly NotesViewModel notesViewModel;
        private readonly NotesTrashViewModel notesTrashViewModel;
        private readonly SettingsViewModel settingsViewModel;
        private readonly CategoriesViewModel categoriesViewModel;
        #endregion

        #region Commands
        public RelayCommand WindowLoadedCommand { get; private set; }
        public RelayCommand WindowClosingCommand { get; private set; }
        #endregion

        #region Public Properties
        private string searchInput;
        public string SearchInput
        {
            get => searchInput;
            set
            {
                if (searchInput != value)
                {
                    SetProperty(ref searchInput, value);
                    OnSearchInputChanged?.Invoke(this, new FilterCriteriaChangedEventArgs() { SearchInput = searchInput });
                }
            }
        }

        #endregion

        #region Initialization
        private readonly IMapper mapper;
        private readonly IRequestsReceiver requestsReceiver;

        public MainViewModel(
            IMediator mediator, 
            ISettings userSettings, 
            IMapper mapper,
            IRequestsReceiver requestsReceiver) : base(mediator, userSettings)
        {
            notesViewModel = Services.GetRequiredService<NotesViewModel>();
            notesViewModel.ParentViewModel = this;

            notesTrashViewModel = Services.GetRequiredService<NotesTrashViewModel>();
            notesTrashViewModel.ParentViewModel = this;

            categoriesViewModel = Services.GetRequiredService<CategoriesViewModel>();
            categoriesViewModel.ParentViewModel = this;

            settingsViewModel = Services.GetRequiredService<SettingsViewModel>();

            WindowLoadedCommand = new RelayCommand(OnWindowLoaded);
            WindowClosingCommand = new RelayCommand(OnWindowClosing);

            this.mapper = mapper;
            this.requestsReceiver = requestsReceiver;

            CurrentViewModel = notesViewModel;

        }

        private async void OnWindowLoaded()
        {
            await LoadNotes();
            await LoadCategories();

            StartRequestsHandling();
        }

        private async void OnWindowClosing()
        {
            await currentViewModel.Save();
            await SaveNotes();
            await SaveCategories();

            StopRequestsHandling();
        }

        #endregion

        #region Events
        public event EventHandler<FilterCriteriaChangedEventArgs> OnSearchInputChanged;

        #endregion

        #region Notes

        private List<Note> notes = new List<Note>();
        private async Task LoadNotes()
        {
            notes.Clear();

            GetNotesQuery query = new GetNotesQuery();
            var noteDTOS = await mediator.Send(query);

            foreach (var noteDTO in noteDTOS)
            {
                notes.Add(new Note(categories)
                {
                    Id = noteDTO.Id,
                    Content = noteDTO.Content,
                    InTrash = noteDTO.InTrash,
                    CategoryId = noteDTO.CategoryId,
                    BackgroundColor = noteDTO.BackgroundColor,
                    LastModifiedAt = noteDTO.LastModifiedAt,
                    State = ModelState.Unmodified
                });
            }

            notesViewModel.Notes = notes;
            notesTrashViewModel.Notes = notes;
        }

        private async Task SaveNotes()
        {
            foreach (var note in notes.Where(n => n.State != ModelState.Unmodified))
            {
                if (note.State == ModelState.New)
                {
                    CreateNoteCommand cmd = new CreateNoteCommand()
                    {
                        Content = note.Content,
                        InTrash = note.InTrash,
                        CategoryId = note.CategoryId,
                        BackgroundColor = note.BackgroundColor
                    };

                    note.Id = await mediator.Send(cmd);
                    note.State = ModelState.Unmodified;
                }
                else if (note.State == ModelState.Modified)
                {
                    UpdateNoteCommand cmd = new UpdateNoteCommand()
                    {
                        Id = note.Id,
                        Content = note.Content,
                        InTrash = note.InTrash,
                        CategoryId = note.CategoryId,
                        BackgroundColor = note.BackgroundColor
                    };

                    await mediator.Send(cmd);

                    note.State = ModelState.Unmodified;
                }
                else if (note.State == ModelState.Deleted)
                {
                    DeleteNoteCommand cmd = new DeleteNoteCommand()
                    {
                        Id = note.Id
                    };

                    await mediator.Send(cmd);
                }
            }
        }

        #endregion

        #region Categories
        private List<Category> categories = new List<Category>();
        private async Task LoadCategories()
        {
            categories.Clear();

            GetCategoriesQuery query = new GetCategoriesQuery();
            var categoryDTOS = await mediator.Send(query);
            
            foreach (var categoryDTO in categoryDTOS)
            {
                categories.Add(new Category()
                {
                    Id = categoryDTO.Id,
                    Title = categoryDTO.Title,
                    Color = categoryDTO.Color,
                    IsDefault = categoryDTO.IsDefault,
                    LastModifiedAt = categoryDTO.LastModifiedAt
                });
            }

            notesViewModel.Categories = categories;
            notesTrashViewModel.Categories = categories;
            categoriesViewModel.Categories = categories;
        }

        private async Task SaveCategories()
        {
            foreach (var category in categories.Where(c => c.State != ModelState.Unmodified))
            {
                if (category.State == ModelState.New)
                {
                    CreateCategoryCommand cmd = new CreateCategoryCommand()
                    {
                        Title = category.Title,
                        Color = category.Color,
                        IsDefault = category.IsDefault
                    };

                    category.Id = await mediator.Send(cmd);
                    category.State = ModelState.Unmodified;
                }
                else if (category.State == ModelState.Modified)
                {
                    UpdateCategoryCommand cmd = new UpdateCategoryCommand()
                    {
                        Id = category.Id,
                        Title = category.Title,
                        Color = category.Color,
                        IsDefault = category.IsDefault
                    };

                    await mediator.Send(cmd);
                }
                else if (category.State == ModelState.Deleted)
                {
                    DeleteCategoryCommand cmd = new DeleteCategoryCommand()
                    {
                        Id = category.Id
                    };

                    await mediator.Send(cmd);
                }
            }
        }

        #endregion

        #region Navigation

        private ViewModelBase currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => currentViewModel;
            set => SetProperty(ref currentViewModel, value);
        }

        public async void OnNavSelectedItemChanged(string selectedItemTag)
        {
            await currentViewModel?.Save();

            switch(selectedItemTag)
            {
                case "notes": CurrentViewModel = notesViewModel; break;
                case "trash": CurrentViewModel = notesTrashViewModel; break;
                case "settings": CurrentViewModel = settingsViewModel; break;
                case "categories": CurrentViewModel = categoriesViewModel; break;
            }

            await CurrentViewModel?.Focused();
        }

        #endregion

        #region Requests Handling

        private void StartRequestsHandling()
        {
            requestsReceiver.NoteCreationRequested += RequestsReceiver_NoteCreationRequested;

            requestsReceiver.StartReceiving();
        }

        private void StopRequestsHandling()
        {
            requestsReceiver.NoteCreationRequested -= RequestsReceiver_NoteCreationRequested;

            requestsReceiver.StopReceiving();
        }

        private void RequestsReceiver_NoteCreationRequested(object sender, ApplicationL.Common.CustomEventArguments.NoteCreationEventArgs e)
        {
            lock(e)
            {
                var cmd = (notesViewModel.CreateNoteWithContentCommand as System.Windows.Input.ICommand);

                if(cmd.CanExecute(e.Content))
                {
                    cmd.Execute(e.Content);
                }
            }
        }

        #endregion

    }
}
