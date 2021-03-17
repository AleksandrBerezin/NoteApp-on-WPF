using System;
using System.Windows;
using GalaSoft.MvvmLight;
using NoteAppWPF.ViewModels;

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

            if (viewModel is NoteViewModel noteViewModel)
            {
                window = new NoteWindow();
                if (noteViewModel.CloseAction == null)
                {
                    noteViewModel.CloseAction = new Action(window.Close);
                }
            }
            else if (viewModel is AboutViewModel aboutViewModel)
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
