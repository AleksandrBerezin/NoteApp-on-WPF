using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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
        /// Список заметок, сортированный по дате изменения
        /// </summary>
        private ObservableCollection<Note> _currentDisplayedNotes;

        /// <summary>
        /// Выбранная заметка
        /// </summary>
        private Note _selectedNote;

        /// <summary>
        /// Выбранная категория заметки
        /// </summary>
        private object _selectedCategory;

        /// <summary>
        /// Команда добавления новой заметки
        /// </summary>
        private RelayCommand _addNoteCommand;

        /// <summary>
        /// Команда редактировани заметки
        /// </summary>
        private RelayCommand _editNoteCommand;

        /// <summary>
        /// Команда удаления заметки
        /// </summary>
        private RelayCommand _removeNoteCommand;

        /// <summary>
        /// Команда открытия справочного окна
        /// </summary>
        private RelayCommand _openAboutWindowCommand;

        /// <summary>
        /// Возвращает и задает список заметок, сортированный по дате изменения
        /// </summary>
        public ObservableCollection<Note> CurrentDisplayedNotes
        {
            get => _currentDisplayedNotes;
            set
            {
                _currentDisplayedNotes = value;
                _project.Notes = _currentDisplayedNotes;
                OnPropertyChanged("CurrentDisplayedNotes");
            }
        }

        /// <summary>
        /// Возвращает и задает выбранную заметку
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
        /// Возвращает и задает выбранную категорию заметки
        /// </summary>
        public object SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }

        /// <summary>
        /// Возвращает список категорий заметок
        /// </summary>
        public List<object> NoteCategories { get; private set; }

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
                           var note = new Note();
                           _noteViewModel = new NoteViewModel(ref note);
                           if (note == null)
                           {
                               return;
                           }

                           CurrentDisplayedNotes.Add(note);
                           ProjectManager.SaveToFile(_project, ProjectManager.DefaultPath);
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

                           var note = (Note) SelectedNote.Clone();
                           _noteViewModel = new NoteViewModel(ref note);
                           if (note == null)
                           {
                               return;
                           }

                           var indexInProject = CurrentDisplayedNotes.IndexOf(SelectedNote);
                           CurrentDisplayedNotes.RemoveAt(indexInProject);
                           CurrentDisplayedNotes.Insert(indexInProject, note);

                           ProjectManager.SaveToFile(_project, ProjectManager.DefaultPath);
                       }));
            }
        }

        public RelayCommand RemoveNoteCommand
        {
            get
            {
                return _removeNoteCommand ??
                       (_removeNoteCommand = new RelayCommand(obj =>
                       {
                           if (SelectedNote == null)
                           {
                               return;
                           }

                           var result = MessageBox.Show(
                               $"Do you really want to remove this note: {SelectedNote}",
                               "Remove Note",
                               MessageBoxButton.OKCancel,
                               MessageBoxImage.Warning,
                               MessageBoxResult.Cancel);

                           if (result == MessageBoxResult.OK)
                           {
                               CurrentDisplayedNotes.Remove(SelectedNote);

                               if (SelectedCategory != null && SelectedCategory != "All")
                               {
                                   CurrentDisplayedNotes = _project.LastChangeTimeSortWithCategory(
                                       (NoteCategory)SelectedCategory);
                               }
                               else
                               {
                                   CurrentDisplayedNotes = _project.LastChangeTimeSort();
                               }

                               if (CurrentDisplayedNotes.Count > 0)
                               {
                                   SelectedNote = CurrentDisplayedNotes[0];
                               }
                               else
                               {
                                   SelectedNote = null;
                               }

                               ProjectManager.SaveToFile(_project, ProjectManager.DefaultPath);
                           }
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

            NoteCategories = new List<object>();
            foreach (var category in Enum.GetValues(typeof(NoteCategory)))
            {
                NoteCategories.Add(category);
            }
            NoteCategories.Add("All");

            CurrentDisplayedNotes = _project.LastChangeTimeSort();
            _project.Notes = CurrentDisplayedNotes;
            SelectedNote = _project.CurrentNote;
        }
    }
}
