using GalaSoft.MvvmLight;
using NoteAppWPF.Services;

namespace NoteAppWPF.ViewModels
{
    /// <summary>
    /// Модель-представление окна <see cref="AboutWindow"/>
    /// </summary>
    public class AboutViewModel : ViewModelBase
    {
        // <summary>
        /// Сервис открытия окна
        /// </summary>
        private readonly WindowService _noteWindowService;

        public AboutViewModel()
        {
            _noteWindowService = new WindowService();
            _noteWindowService.OpenWindow(this);
        }
    }
}
