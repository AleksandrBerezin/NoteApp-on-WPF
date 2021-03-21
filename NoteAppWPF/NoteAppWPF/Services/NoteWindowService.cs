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
        /// <summary>
        /// Окно редактирования заметки
        /// </summary>
        private NoteWindow _window;

        /// <inheritdoc/>
        public bool? OpenWindow(ViewModelBase viewModel)
        {
            _window = new NoteWindow();
            var vm = (NoteVM) viewModel;

            if (vm.CloseAction == null)
            {
                vm.CloseAction = new Action(_window.Close);
            }

            _window.DataContext = viewModel;
            _window.ShowDialog();

            return vm.DialogResult;
        }
    }
}
