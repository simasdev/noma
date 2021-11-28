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
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(MainViewModel mainViewModel)
        {
            InitializeComponent();

            this.DataContext = mainViewModel;
        }

        private void NavigationMenu_SelectedItemChaged(string obj) // change using Interaction triggers
        {
            ((MainViewModel)this.DataContext).OnNavSelectedItemChanged(obj);
        }
    }
}
