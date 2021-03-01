namespace NoteAppWPF.ViewModels
{
    /// <summary>
    /// Модель-представление окна <see cref="AboutWindow"/>
    /// </summary>
    public class AboutViewModel
    {
        public AboutViewModel()
        {
            var window = new AboutWindow();
            window.ShowDialog();
        }
    }
}
