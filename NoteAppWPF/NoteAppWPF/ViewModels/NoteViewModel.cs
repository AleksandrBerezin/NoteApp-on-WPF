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
        /// Возвращает список категорий заметок
        /// </summary>
        public List<string> NoteCategories { get; private set; }

        public NoteViewModel(Note note)
        {
            CurrentNote = note;
            NoteCategories = Enum.GetNames(typeof(NoteCategory)).ToList();
            var window = new NoteWindow(this);
            window.ShowDialog();
        }
    }
}
