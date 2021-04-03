using NoteAppWPF.Views;

namespace NoteAppWPF.Services
{
    /// <summary>
    /// Класс <see cref="AboutWindowService"/> для работы со справочным окном
    /// </summary>
    public class AboutWindowService : IAboutWindowService
    {
        /// <inheritdoc/>
        public bool? OpenWindow()
        {
            var window = new AboutWindow();
            window.ShowDialog();

            return true;
        }
    }
}
