using System.Windows;
using NoteAppWPF.ViewModels;

namespace NoteAppWPF
{
    // TODO: все View в папку Views
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
