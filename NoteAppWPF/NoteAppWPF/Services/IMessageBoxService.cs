using System.Windows;

namespace NoteAppWPF.Services
{
    /// <summary>
    /// Интерфейс <see cref="IMessageBoxService"/> для работы с диалоговыми окнами
    /// </summary>
    public interface IMessageBoxService
    {
        /// <summary>
        /// Вывод сообщения
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <param name="button"></param>
        /// <param name="icon"></param>
        void ShowMessage(string message, string caption, MessageBoxButton button, MessageBoxImage icon);
    }
}