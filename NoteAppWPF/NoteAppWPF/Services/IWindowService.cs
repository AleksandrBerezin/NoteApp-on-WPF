using GalaSoft.MvvmLight;

namespace NoteAppWPF.Services
{
    /// <summary>
    /// Интерфейс <see cref="IWindowService"/> для работы с окнами приложения
    /// </summary>
    public interface IWindowService
    {
        /// <summary>
        /// Открытие окна
        /// </summary>
        /// <param name="viewModel"></param>
        void OpenWindow(ViewModelBase viewModel);
    }
}