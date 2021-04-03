namespace NoteAppWPF.Services
{
    /// <summary>
    /// Интерфейс <see cref="IAboutWindowService"/> для работы со справочным окном
    /// </summary>
    public interface IAboutWindowService
    {
        /// <summary>
        ///  Открытие окна
        /// </summary>
        /// <returns></returns>
        bool? OpenWindow();
    }
}