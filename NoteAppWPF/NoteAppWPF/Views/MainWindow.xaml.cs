﻿using System.Windows;
using NoteAppWPF.Services;
using NoteAppWPF.ViewModels;

namespace NoteAppWPF.Views
{
    // TODO: все View в папку Views (DONE)
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVM(new NoteWindowService(), new AboutWindowService(), new MessageBoxService());
        }
    }
}
