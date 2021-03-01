using System.Windows;
using Core;
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
        }
    }
}
