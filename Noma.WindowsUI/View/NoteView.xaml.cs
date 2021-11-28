using Noma.WindowsUI.Common.Interfaces;
using Noma.WindowsUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Noma.WindowsUI.View
{
    /// <summary>
    /// Interaction logic for NoteView.xaml
    /// </summary>
    public partial class NoteView : Window, ICloseable
    {
        public NoteView(NoteViewModel noteViewModel)
        {
            InitializeComponent();

            btnMinimize.Click += (s, e) => this.WindowState = WindowState.Minimized;
            btnClose.Click += (s, e) => this.Close();

            this.DataContext = noteViewModel;
        }
    }
}
