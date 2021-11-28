using MediatR;
using Noma.WindowsUI.Common.Base;
using Noma.WindowsUI.Common.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Noma.WindowsUI.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Initialization
        public SettingsViewModel(IMediator mediator, ISettings userSettings) : base(mediator, userSettings)
        {

        }
        #endregion
    }
}
