using GalaSoft.MvvmLight;

namespace NoteAppWPF.Services
{
    /// <summary>
    /// Интерфейс <see cref="INoteWindowService"/> для работы с окнами приложения
    /// </summary>
    public interface INoteWindowService
    {
        /// <summary>
        /// Открытие окна
        /// </summary>
        /// <param name="viewModel"></param>
        void OpenWindow(ViewModelBase viewModel);
    }
}