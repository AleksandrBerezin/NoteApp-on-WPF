using NoteAppWPF.ViewModels;

namespace NoteAppWPF.Services
{
    /// <summary>
    /// Интерфейс <see cref="INoteWindowService"/> для работы с окном редактирования заметки
    /// </summary>
    public interface INoteWindowService
    {
        /// <summary>
        /// Открытие окна
        /// </summary>
        /// <param name="viewModel"></param>
        void OpenWindow(NoteViewModel viewModel);
    }
}