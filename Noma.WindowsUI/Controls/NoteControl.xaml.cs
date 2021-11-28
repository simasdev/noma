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
using Noma.WindowsUI.Model;

namespace Noma.WindowsUI.Controls
{
    /// <summary>
    /// Interaction logic for NoteControl.xaml
    /// </summary>
    public partial class NoteControl : UserControl
    {
        public NoteControl()
        {
            InitializeComponent();
        }

        public Note Note
        {
            get { return (Note)GetValue(NoteProperty); }
            set { SetValue(NoteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Note.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoteProperty =
            DependencyProperty.Register("Note", typeof(Note), typeof(NoteControl));



        public List<Category> Categories
        {
            get { return (List<Category>)GetValue(CategoriesProperty); }
            set { SetValue(CategoriesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Categories.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoriesProperty =
            DependencyProperty.Register("Categories", typeof(List<Category>), typeof(NoteControl), new PropertyMetadata(null));


        public Visibility MoveToTrashButtonVisibility
        {
            get => btnMoveToTrash.Visibility;
            set => btnMoveToTrash.Visibility = value;
        }

        public Visibility RestoreButtonVisibility
        {
            get => btnRestore.Visibility;
            set => btnRestore.Visibility = value;
        }

        public Visibility DeletePermanentlyButtonVisibility
        {
            get => btnDeletePermanently.Visibility;
            set => btnDeletePermanently.Visibility = value;
        }

        private bool allowEdit = true;

        public bool AllowEdit
        {
            get { return allowEdit; }
            set
            {
                allowEdit = value;

                txtContent.IsReadOnly = !allowEdit;
                comboBoxCategory.IsEditable = allowEdit;
            }
        }


        public event EventHandler OnDeletePermanentlyRequested;
        private void btnDeletePermanently_Click(object sender, RoutedEventArgs e)
        {
            OnDeletePermanentlyRequested?.Invoke(this, new EventArgs());
        }

        public event EventHandler OnMoveToTrashRequested;
        private void btnMoveToTrash_Click(object sender, RoutedEventArgs e)
        {
            OnMoveToTrashRequested?.Invoke(this, new EventArgs());
        }

        public event EventHandler OnRestoreRequested;
        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            OnRestoreRequested?.Invoke(this, new EventArgs());
        }

        public event EventHandler OnExpandRequested;

        private void txtContent_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OnExpandRequested?.Invoke(this, new EventArgs());
        }
    }
}
