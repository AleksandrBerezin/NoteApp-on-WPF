using System;
using Core;
using NoteAppWPF.ViewModels;
using NoteAppWPF.Views;

namespace NoteAppWPF.Services
{
    /// <summary>
    /// Класс <see cref="NoteWindowService"/> для работы с окном редактирования заметки
    /// </summary>
    public class NoteWindowService : INoteWindowService
    {
        //TODO: передавать базовый класс небезопасно - сервис работает с одной конкретной VM (DONE)
        /// <inheritdoc/>
        public bool? OpenWindow(Note note)
        {
            var window = new NoteWindow();
            var viewModel = new NoteVM(note, new MessageBoxService());

            if (viewModel.CloseAction == null)
            {
                viewModel.CloseAction = new Action(window.Close);
            }

            window.DataContext = viewModel;
            window.ShowDialog();

            return viewModel.DialogResult;
        }
    }
}
