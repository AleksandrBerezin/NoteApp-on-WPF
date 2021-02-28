using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace NoteAppWPF.ViewModels
{
    public class MainViewModel : Notifier
    {
        /// <summary>
        /// Проект
        /// </summary>
        private Project _project;

        /// <summary>
        /// Список заметок, сортированный по дате изменения
        /// </summary>
        private List<Note> _currentDisplayedNotes;

        /// <summary>
        /// Выбранная заметка
        /// </summary>
        private Note _selectedNote;

        /// <summary>
        /// Возвращает и задает список заметок, сортированный по дате изменения
        /// </summary>
        public List<Note> CurrentDisplayedNotes { get; private set; }

        /// <summary>
        /// Вщзвращает и задает выбранную заметку
        /// </summary>
        public Note SelectedNote
        {
            get => _selectedNote;
            set
            {
                _selectedNote = value;
                _project.CurrentNote = _selectedNote;
                OnPropertyChanged("SelectedNote");
            }
        }

        /// <summary>
        /// Возвращает список категорий заметок
        /// </summary>
        public List<string> NotesCategories { get; private set; }

        public MainViewModel()
        {
            _project = ProjectManager.LoadFromFile(ProjectManager.DefaultPath);
            NotesCategories = Enum.GetNames(typeof(NoteCategory)).ToList();
            NotesCategories.Add("All");

            CurrentDisplayedNotes = _project.LastChangeTimeSort();
            _project.Notes = CurrentDisplayedNotes;
            SelectedNote = _project.CurrentNote;
        }
    }
}
