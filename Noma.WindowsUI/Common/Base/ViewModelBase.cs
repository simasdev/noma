using MediatR;
using Microsoft.Extensions.Hosting;
using Noma.WindowsUI.Common.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Noma.WindowsUI.Common.Base
{
    public class ViewModelBase: BindableBase
    {
        protected readonly IMediator mediator;

        protected readonly ISettings userSettings;

        public ViewModelBase(IMediator mediator, ISettings userSettings)
        {
            this.mediator = mediator;
            this.userSettings = userSettings;
        }

        public IHost Host => ((App)Application.Current).AppHost;

        public IServiceProvider Services => Host.Services;

        public virtual Task Save() => Task.CompletedTask;

        public virtual Task Focused() => Task.CompletedTask;

    }
}
