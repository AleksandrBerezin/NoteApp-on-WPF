using System;
using System.Windows;
using GalaSoft.MvvmLight;
using NoteAppWPF.ViewModels;
using NoteAppWPF.Views;

namespace NoteAppWPF.Services
{
    // TODO: для разных окон лучше делать разные сервисы
    /// <summary>
    /// Класс <see cref="WindowService"/> для работы с окнами приложения
    /// </summary>
    public class WindowService : INoteWindowService
    {
        /// <inheritdoc/>
        public void OpenWindow(ViewModelBase viewModel)
        {
            Window window;

            if (viewModel is NoteVM noteViewModel)
            {
                window = new NoteWindow();
                if (noteViewModel.CloseAction == null)
                {
                    noteViewModel.CloseAction = new Action(window.Close);
                }
            }
            else if (viewModel is AboutVM aboutViewModel)
            {
                window = new AboutWindow();
            }
            else
            {
                throw new ArgumentException();
            }

            window.DataContext = viewModel;
            window.ShowDialog();
        }
    }
}
