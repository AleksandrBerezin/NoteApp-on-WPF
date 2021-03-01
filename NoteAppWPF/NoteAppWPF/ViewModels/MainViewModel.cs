using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace NoteAppWPF.ViewModels
{
    /// <summary>
    /// Модель-представление окна <see cref="MainWindow"/>
    /// </summary>
    public class MainViewModel : Notifier
    {
        /// <summary>
        /// Модель-представление окна редактирования заметки
        /// </summary>
        private NoteViewModel _noteViewModel;

        /// <summary>
        /// Модель-представление справочного окна
        /// </summary>
        private AboutViewModel _aboutViewModel;

        /// <summary>
        /// Проект
        /// </summary>
        private Project _project;

        /// <summary>
        /// Выбранная заметка
        /// </summary>
        private Note _selectedNote;

        /// <summary>
        /// Команда добавления новой заметки
        /// </summary>
        private RelayCommand _addNoteCommand;

        /// <summary>
        /// Команда редактировани заметки
        /// </summary>
        private RelayCommand _editNoteCommand;

        /// <summary>
        /// Команда открытия справочного окна
        /// </summary>
        private RelayCommand _openAboutWindowCommand;

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
        public List<string> NoteCategories { get; private set; }

        /// <summary>
        /// Возвращает команду добавления новой заметки
        /// </summary>
        public RelayCommand AddNoteCommand
        {
            get
            {
                return _addNoteCommand ??
                       (_addNoteCommand = new RelayCommand(obj =>
                       {
                           _noteViewModel = new NoteViewModel(null);
                       }));
            }
        }

        /// <summary>
        /// Возвращает команду редактирования заметки
        /// </summary>
        public RelayCommand EditNoteCommand
        {
            get
            {
                return _editNoteCommand ??
                       (_editNoteCommand = new RelayCommand(obj =>
                       {
                           if (SelectedNote == null)
                           {
                               return;
                           }
                           //TODO передавать копию
                           _noteViewModel = new NoteViewModel(SelectedNote);

                           ProjectManager.SaveToFile(_project, ProjectManager.DefaultPath);
                       }));
            }
        }

        /// <summary>
        /// Возвращает команду открытия справочного окна
        /// </summary>
        public RelayCommand OpenAboutWindowCommand
        {
            get
            {
                return _openAboutWindowCommand ??
                       (_openAboutWindowCommand = new RelayCommand(obj =>
                       {
                           _aboutViewModel = new AboutViewModel();
                       }));
            }
        }

        public MainViewModel()
        {
            _project = ProjectManager.LoadFromFile(ProjectManager.DefaultPath);
            NoteCategories = Enum.GetNames(typeof(NoteCategory)).ToList();
            NoteCategories.Add("All");

            CurrentDisplayedNotes = _project.LastChangeTimeSort();
            _project.Notes = CurrentDisplayedNotes;
            SelectedNote = _project.CurrentNote;
        }
    }
}
