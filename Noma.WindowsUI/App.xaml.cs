using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration.Json;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Noma.Infrastructure;
using Noma.ApplicationL;
using Microsoft.Extensions.Hosting;
using Noma.WindowsUI.ViewModel;
using Noma.WindowsUI.View;
using Noma.WindowsUI.Common.Interfaces;
using Noma.WindowsUI.Model;

namespace Noma.WindowsUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost host;
        public IHost AppHost { get => host; }

        public App()
        {
            host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services);
                })
                .Build();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure();
            services.AddApplication();
            services.AddSingleton<ISettings, UserSettings>();
            services.AddSingleton<MainView>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<NotesViewModel>();
            services.AddSingleton<NotesTrashViewModel>();
            services.AddTransient<NoteViewModel>();
            services.AddSingleton<CategoriesViewModel>();
            services.AddSingleton<SettingsViewModel>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await host.StartAsync();

            var mainWindow = host.Services.GetRequiredService<MainView>();
            mainWindow.Show();

            base.OnStartup(e);
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            await host.StopAsync();
            host.Dispose();

            base.OnExit(e);
        }
    }
}
