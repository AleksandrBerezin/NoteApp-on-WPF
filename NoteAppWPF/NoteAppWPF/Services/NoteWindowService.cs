using System;
using NoteAppWPF.ViewModels;

namespace NoteAppWPF.Services
{
    /// <summary>
    /// Класс <see cref="NoteWindowService"/> для работы с окном редактирования заметки
    /// </summary>
    public class NoteWindowService : INoteWindowService
    {
        /// <inheritdoc/>
        public void OpenWindow(NoteViewModel viewModel)
        {
            var window = new NoteWindow {DataContext = viewModel};

            if (viewModel.CloseAction == null)
            {
                viewModel.CloseAction = new Action(window.Close);
            }

            window.ShowDialog();
        }
    }
}
