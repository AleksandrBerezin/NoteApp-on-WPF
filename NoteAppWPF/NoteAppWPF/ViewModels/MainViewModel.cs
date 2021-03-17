using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Core;
using GalaSoft.MvvmLight;
using NoteAppWPF.Services;

namespace NoteAppWPF.ViewModels
{
    /// <summary>
    /// Модель-представление окна <see cref="MainWindow"/>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        // TODO: зачем хранить как поле, если она используется только в команде?
        /// <summary>
        /// Модель-представление окна редактирования заметки
        /// </summary>
        private NoteViewModel _noteViewModel;

        // TODO: зачем хранить как поле
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

        // TODO: Почему object? Хотя бы string, если надо совместить enum и string
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
        /// Команда закрытия приложения
        /// </summary>
        private RelayCommand _exitCommand;

        // TODO: этот сервис хранить на постоянку смысла нет
        /// <summary>
        ///Сервис вывода сообщения
        /// </summary>
        private MessageBoxService _messageBoxService;

        /// <summary>
        /// Возвращает и задает список заметок, сортированный по дате изменения
        /// </summary>
        public ObservableCollection<Note> CurrentDisplayedNotes
        {
            get => _currentDisplayedNotes;
            set
            {
                _currentDisplayedNotes = value;
                RaisePropertyChanged(nameof(CurrentDisplayedNotes));
            }
        }

        // TODO: если в проекте уже есть CurrentNote, то зачем повторяться здесь?
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
                RaisePropertyChanged(nameof(SelectedNote));
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
                if (_selectedCategory == "All")
                {
                    CurrentDisplayedNotes = _project.LastChangeTimeSort();
                }
                else
                {
                    CurrentDisplayedNotes = _project.LastChangeTimeSortWithCategory(
                        (NoteCategory) _selectedCategory);
                }

                RaisePropertyChanged(nameof(SelectedCategory));
            }
        }

        // TODO: просто Categories, из контекста и так понятно какие категории
        // TODO: object ненадежно
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
                           // TODO: неправильное взаимодействие. Главное окно должно вызывать IWindowService,
                           // передавая в него NoteVM. Мотивация - вызов конструктора VM не очевидно,
                           // что внутри него будет показываться форма.
                           var note = new Note();
                           _noteViewModel = new NoteViewModel(ref note);
                           if (note == null)
                           {
                               return;
                           }

                           _project.Notes.Add(note);

                           FillNotesListAfterEdit(note);
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
                           var realIndexInProject = _project.Notes.IndexOf(note);

                           _noteViewModel = new NoteViewModel(ref note);
                           if (note == null)
                           {
                               return;
                           }

                           _project.Notes.RemoveAt(realIndexInProject);
                           _project.Notes.Insert(realIndexInProject, note);

                           FillNotesListAfterEdit(note);
                           ProjectManager.SaveToFile(_project, ProjectManager.DefaultPath);
                       }));
            }
        }

        /// <summary>
        /// Возвращает команду удаления заметки
        /// </summary>
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

                           var result = _messageBoxService.ShowMessage(
                               $"Do you really want to remove this note: {SelectedNote}",
                               "Remove Note",
                               MessageBoxButton.OKCancel,
                               MessageBoxImage.Warning);

                           if (result)
                           {
                               _project.Notes.Remove(SelectedNote);
                               CurrentDisplayedNotes.Remove(SelectedNote);
                               FillNotesListAfterEdit(null);

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

        /// <summary>
        /// Возвращает команду закрытия приложения
        /// </summary>
        public RelayCommand ExitCommand
        {
            get
            {
                return _exitCommand ??
                       (_exitCommand = new RelayCommand(obj =>
                       {
                           ProjectManager.SaveToFile(_project, ProjectManager.DefaultPath);
                           ((MainWindow)obj).Close();
                       }));
            }
        }

        // TODO: программа иногда падает, если добавить заметки разных категорий,
        // затем выбрать на отображение какую-то категорию, а затем начать удалять заметки (с выделением и без)
        // То есть иногда в качестве note приходит null. Протестировать и исправить багу
        /// <summary>
        /// Метод для заполнения списка заметок после и выбора текущей заметки
        /// после добавления, редактирования или удаления заметки
        /// </summary>
        private void FillNotesListAfterEdit(Note note)
        {
            if (SelectedCategory != null && SelectedCategory != "All")
            {
                CurrentDisplayedNotes = _project.LastChangeTimeSortWithCategory(
                    (NoteCategory)SelectedCategory);

                // TODO: перечисления можно сравнивать без Equals
                // TODO: linq?
                if (note.Category.Equals((NoteCategory)SelectedCategory))
                {
                    SelectedNote = note;
                    return;
                }
            }
            else
            {
                CurrentDisplayedNotes = _project.LastChangeTimeSort();
                SelectedNote = CurrentDisplayedNotes[0];
            }

            SelectedNote = CurrentDisplayedNotes.Count > 0 ? CurrentDisplayedNotes[0] : null;
        }

        // TODO: xml
        public MainViewModel()
        {
            _messageBoxService = new MessageBoxService();
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
