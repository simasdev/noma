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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Noma.WindowsUI.Controls
{
    /// <summary>
    /// Interaction logic for NavigationMenu.xaml
    /// </summary>
    public partial class NavigationMenu : UserControl
    {
        public event Action<string> SelectedItemChaged;

        public NavigationMenu()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private ListBoxItem selectedItem;
        public ListBoxItem SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                SelectedItemChaged?.Invoke(selectedItem?.Tag?.ToString());
            }
        }

       
    }
}
