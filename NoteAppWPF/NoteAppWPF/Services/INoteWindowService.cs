using Core;

namespace NoteAppWPF.Services
{
    /// <summary>
    /// Интерфейс <see cref="INoteWindowService"/> для работы с окном редактирования заметки
    /// </summary>
    public interface INoteWindowService
    {
        /// <summary>
        ///  Открытие окна
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        bool? OpenWindow(Note note);
    }
}