using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Core
{
    /// <summary>
    /// Класс <see cref="Project"/>, хранящий список заметок
    /// </summary>
    public class Project : Notifier
    {
        /// <summary>
        /// Список всех заметок
        /// </summary>
        private ObservableCollection<Note> _notes;
        
        /// <summary>
        /// Текущая заметка
        /// </summary>
        private Note _currentNote;

        /// <summary>
        /// Возвращает и задает список всех заметок
        /// </summary>
        public ObservableCollection<Note> Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }

        /// <summary>
        /// Возвращает и задает текущую заметку
        /// </summary>
        public Note CurrentNote
        {
            get => _currentNote;
            set
            {
                _currentNote = value;
                OnPropertyChanged(nameof(CurrentNote));
            }
        }

        /// <summary>
        /// Создает экземпляр <see cref="Project"/>
        /// </summary>
        public Project()
        {
            Notes = new ObservableCollection<Note>();
        }

        /// <summary>
        /// Метод для сортировки списка заметок по дате изменения (по убыванию)
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Note> LastChangeTimeSort()
        {
            var orderedList =
                Notes.OrderByDescending(note => note.LastChangeTime);
            return new ObservableCollection<Note>(orderedList.ToList());
        }

        /// <summary>
        /// Метод для сортировки списка заметок по дате изменения (по убыванию)
        /// при определенной категории
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public ObservableCollection<Note> LastChangeTimeSortWithCategory(NoteCategory category)
        {
            var orderedList = Notes.OrderByDescending(note =>
                note.LastChangeTime).Where(note => note.Category == category).ToList();
            return new ObservableCollection<Note>(orderedList);
        }
    }
}
