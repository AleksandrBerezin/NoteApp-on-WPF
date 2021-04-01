using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;

namespace Core
{
    /// <summary>
    /// Класс <see cref="Project"/>, хранящий список заметок
    /// </summary>
    public class Project : ObservableObject
    {
        /// <summary>
        /// Возвращает и задает список всех заметок
        /// </summary>
        public ObservableCollection<Note> Notes { get; set; } = new ObservableCollection<Note>();

        /// <summary>
        /// Возвращает и задает текущую заметку
        /// </summary>
        public Note CurrentNote { get; set; }

        /// <summary>
        /// Создает экземпляр <see cref="Project"/>
        /// </summary>
        public Project()
        {
            // TODO: зачем пустой конструктор?
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
