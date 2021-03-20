using System;
using GalaSoft.MvvmLight;
using NoteAppWPF.ViewModels;
using NoteAppWPF.Views;

namespace NoteAppWPF.Services
{
    // TODO: для разных окон лучше делать разные сервисы (DONE)
    /// <summary>
    /// Класс <see cref="NoteWindowService"/> для работы с окном редактирования заметки
    /// </summary>
    public class NoteWindowService : IWindowService
    {
        /// <inheritdoc/>
        public void OpenWindow(ViewModelBase viewModel)
        {
            var window = new NoteWindow();
            var vm = (NoteVM) viewModel;

            if (vm.CloseAction == null)
            {
                vm.CloseAction = new Action(window.Close);
            }

            window.DataContext = viewModel;
            window.ShowDialog();
        }
    }
}
