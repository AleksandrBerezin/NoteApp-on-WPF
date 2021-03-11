using System.Windows;

namespace NoteAppWPF.Services
{
    /// <summary>
    /// Класс <see cref="IMessageBoxService"/> для работы с диалоговыми окнами
    /// </summary>
    public class MessageBoxService : IMessageBoxService
    {
        /// <inheritdoc/>
        public void ShowMessage(string message, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            MessageBox.Show(message, caption, button, icon);
        }
    }
}
