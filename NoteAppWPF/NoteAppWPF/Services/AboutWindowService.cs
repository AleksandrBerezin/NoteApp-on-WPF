using GalaSoft.MvvmLight;
using NoteAppWPF.Views;

namespace NoteAppWPF.Services
{
    /// <summary>
    /// Класс <see cref="AboutWindowService"/> для работы со справочным окном
    /// </summary>
    public class AboutWindowService : IWindowService
    {
        /// <inheritdoc/>
        public bool? OpenWindow(ViewModelBase viewModel)
        {
            var window = new AboutWindow
            {
                DataContext = viewModel
            };
            window.ShowDialog();

            return true;
        }
    }
}
