using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace NoteAppWPF.ViewModels
{
    /// <summary>
    /// Модель-представление окна <see cref="NoteWindow"/>
    /// </summary>
    public class NoteViewModel : Notifier
    {
        /// <summary>
        /// Текущая заметка
        /// </summary>
        private Note _currentNote;

        /// <summary>
        /// Команда закрытия окна с сохраненем данных
        /// </summary>
        private RelayCommand _okCommand;

        /// <summary>
        /// Команда закрытия окна без сохранения данных
        /// </summary>
        private RelayCommand _cancelCommand;

        /// <summary>
        /// Возвращает и задает текущую заметку
        /// </summary>
        public Note CurrentNote
        {
            get => _currentNote;
            set
            {
                _currentNote = value;
                OnPropertyChanged("CurrentNote");
            }
        }

        /// <summary>
        /// Возвращает команду закрытия окна с сохраненем данных
        /// </summary>
        public RelayCommand OkCommand
        {
            get
            {
                return _okCommand ??
                       (_okCommand = new RelayCommand(obj =>
                       {
                           var window = obj as NoteWindow;
                           window.DialogResult = true;
                           window.Close();
                       }));
            }
        }

        /// <summary>
        /// Возвращает команду закрытия окна без сохранения данных
        /// </summary>
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ??
                       (_cancelCommand = new RelayCommand(obj =>
                       {
                           ((NoteWindow)obj).Close();
                       }));
            }
        }

        /// <summary>
        /// Возвращает список категорий заметок
        /// </summary>
        public List<string> NoteCategories { get; private set; }

        public NoteViewModel(ref Note note)
        {
            CurrentNote = note;
            NoteCategories = Enum.GetNames(typeof(NoteCategory)).ToList();
            var window = new NoteWindow(this);
            var result = window.ShowDialog();

            if (result == null || !result.Value)
            {
                note = null;
            }
        }
    }
}
