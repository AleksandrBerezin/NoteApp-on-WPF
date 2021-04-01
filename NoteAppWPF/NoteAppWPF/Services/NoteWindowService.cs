using System;
using GalaSoft.MvvmLight;
using NoteAppWPF.ViewModels;
using NoteAppWPF.Views;

namespace NoteAppWPF.Services
{
    /// <summary>
    /// Класс <see cref="NoteWindowService"/> для работы с окном редактирования заметки
    /// </summary>
    public class NoteWindowService : IWindowService
    {
        /// <summary>
        /// Окно редактирования заметки
        /// </summary>
        private NoteWindow _window;

        //TODO: передавать базовый класс небезопасно - сервис работает с одной конкретной VM
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
