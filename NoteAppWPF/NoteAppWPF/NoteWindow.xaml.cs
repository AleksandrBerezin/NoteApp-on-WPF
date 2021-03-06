using System;
using System.Windows;
using NoteAppWPF.ViewModels;

namespace NoteAppWPF
{
    /// <summary>
    /// Interaction logic for NoteWindow.xaml
    /// </summary>
    public partial class NoteWindow : Window
    {
        public NoteWindow(NoteViewModel noteViewModel)
        {
            InitializeComponent();
            DataContext = noteViewModel;

            if (noteViewModel.CloseAction == null)
            {
                noteViewModel.CloseAction = new Action(this.Close);
            }
        }
    }
}
