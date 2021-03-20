using GalaSoft.MvvmLight;
using NoteAppWPF.Services;
using NoteAppWPF.Views;

namespace NoteAppWPF.ViewModels
{
    /// <summary>
    /// Модель-представление окна <see cref="AboutWindow"/>
    /// </summary>
    public class AboutVM : ViewModelBase
    {
        // <summary>
        /// Сервис открытия окна
        /// </summary>
        private readonly WindowService _noteWindowService;

        // TODO: xml
        public AboutVM()
        {
            _noteWindowService = new WindowService();
            _noteWindowService.OpenWindow(this);
        }
    }
}
