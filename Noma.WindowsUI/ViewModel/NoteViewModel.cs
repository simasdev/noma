using MediatR;
using Noma.WindowsUI.Common;
using Noma.WindowsUI.Common.Base;
using Noma.WindowsUI.Common.Interfaces;
using Noma.WindowsUI.Enums;
using Noma.WindowsUI.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Noma.WindowsUI.ViewModel
{
    public class NoteViewModel: ViewModelBase
    {
        #region Public Properties
        public Note Note { get; set; }
        public List<Category> Categories { get; set; }
        #endregion

        #region Commands 
        public RelayCommand DeleteNoteCommand { get; set; }
        public RelayCommand MoveNoteToTrashCommand { get; set; }
        public RelayCommand<ICloseable> CloseWindowCommand { get; set; }
        #endregion

        #region Events
        public event EventHandler OnNoteDeleted;
        public event EventHandler OnNoteMovedToTrash;
        #endregion

        #region Initialization
        public NoteViewModel(IMediator mediator, ISettings userSettings) : base(mediator, userSettings)
        {
            DeleteNoteCommand = new RelayCommand(OnDeleteRequested);
            MoveNoteToTrashCommand = new RelayCommand(OnMoveNoteToTrashRequested);
            CloseWindowCommand = new RelayCommand<ICloseable>(OnCloseWindow);
        }
        #endregion

        #region Command Execution Methods
        private void OnMoveNoteToTrashRequested()
        {
            Note.InTrash = true;

            OnNoteMovedToTrash?.Invoke(this, new EventArgs());
        }

        private void OnDeleteRequested()
        {
            Note.InTrash = true;
            Note.State = ModelState.Deleted;

            OnNoteDeleted?.Invoke(this, new EventArgs());
        }

        private void OnCloseWindow(ICloseable window)
        {
            window?.Close();
        }
        #endregion
    }
}
