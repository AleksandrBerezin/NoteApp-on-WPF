using System.Windows;

namespace NoteAppWPF.Services
{
    /// <summary>
    /// Класс <see cref="IMessageBoxService"/> для работы с диалоговыми окнами
    /// </summary>
    public class MessageBoxService : IMessageBoxService
    {
        /// <inheritdoc/>
        public bool ShowMessage(string message, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            var result = MessageBox.Show(message, caption, button, icon);
            return result is MessageBoxResult.OK;
        }
    }
}
