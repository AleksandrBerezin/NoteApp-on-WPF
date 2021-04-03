using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NoteAppWPF.Services;
using NoteAppWPF.Views;

namespace NoteAppWPF.ViewModels
{
    /// <summary>
    /// Модель-представление окна <see cref="MainWindow"/>
    /// </summary>
    public class MainVM : ViewModelBase
    {
        /// <summary>
        /// Проект
        /// </summary>
        private Project _project;

        /// <summary>
        /// Список заметок, сортированный по дате изменения
        /// </summary>
        private ObservableCollection<Note> _currentDisplayedNotes;

        /// <summary>
        /// Выбранная категория заметки
        /// </summary>
        private string _selectedCategory;

        /// <summary>
        /// Команда добавления новой заметки
        /// </summary>
        private RelayCommand<object> _addNoteCommand;

        /// <summary>
        /// Команда редактирования заметки
        /// </summary>
        private RelayCommand<object> _editNoteCommand;

        /// <summary>
        /// Команда удаления заметки
        /// </summary>
        private RelayCommand<object> _removeNoteCommand;

        /// <summary>
        /// Команда открытия справочного окна
        /// </summary>
        private RelayCommand<object> _openAboutWindowCommand;

        /// <summary>
        /// Команда закрытия приложения
        /// </summary>
        private RelayCommand<object> _exitCommand;

        /// <summary>
        /// Сервис для работы с окном редактирования заметки
        /// </summary>
        private readonly INoteWindowService _noteWindowService;

        /// <summary>
        /// Сервис для работы со справочным окном
        /// </summary>
        private readonly IAboutWindowService _aboutWindowService;

        /// <summary>
        /// Сервис для работы с окном вывода сообщения
        /// </summary>
        private readonly IMessageBoxService _messageBoxService;

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

        // TODO: если в проекте уже есть CurrentNote, то зачем повторяться здесь? (DONE)
        // (_project закрытый, как тогда к CurrentNote биндиться?)
        // .. Речь про поле _selectedNote, а не свойство. Просто используй поле в проекте.
        /// <summary>
        /// Возвращает и задает выбранную заметку
        /// </summary>
        public Note SelectedNote
        {
            get => _project.CurrentNote;
            set
            {
                _project.CurrentNote = value;
                RaisePropertyChanged(nameof(SelectedNote));
            }
        }

        /// <summary>
        /// Возвращает и задает выбранную категорию заметки
        /// </summary>
        public string SelectedCategory
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
                    Enum.TryParse(_selectedCategory, out NoteCategory category);
                    CurrentDisplayedNotes = _project.LastChangeTimeSortWithCategory(category);
                }

                RaisePropertyChanged(nameof(SelectedCategory));
            }
        }

        /// <summary>
        /// Возвращает список категорий заметок
        /// </summary>
        public List<string> Categories { get; private set; }

        /// <summary>
        /// Возвращает команду добавления новой заметки
        /// </summary>
        public RelayCommand<object> AddNoteCommand
        {
            get
            {
                return _addNoteCommand ??
                       (_addNoteCommand = new RelayCommand<object>(obj =>
                       {
                           var note = new Note();
                           // TODO: VM не должна создавать экземпляры конкретных сервисов! (DONE)
                           // Работа только через интерфейсы сервисов, экземпляры которых передаются в конструктор
                           var result = _noteWindowService.OpenWindow(note);

                           if (result != true)
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
        public RelayCommand<object> EditNoteCommand
        {
            get
            {
                return _editNoteCommand ??
                       (_editNoteCommand = new RelayCommand<object>(obj =>
                       {
                           if (SelectedNote == null)
                           {
                               return;
                           }

                           var note = (Note) SelectedNote.Clone();
                           var realIndexInProject = _project.Notes.IndexOf(note);
                           // TODO: см. выше (DONE)
                           var result = _noteWindowService.OpenWindow(note);

                           if (result != true)
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
        public RelayCommand<object> RemoveNoteCommand
        {
            get
            {
                return _removeNoteCommand ??
                       (_removeNoteCommand = new RelayCommand<object>(obj =>
                       {
                           if (SelectedNote == null)
                           {
                               return;
                           }
                           // TODO: см. выше (DONE)
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
        public RelayCommand<object> OpenAboutWindowCommand
        {
            get
            {
                return _openAboutWindowCommand ??
                       (_openAboutWindowCommand = new RelayCommand<object>(obj =>
                       {
                           // TODO: см. выше (DONE)
                           _aboutWindowService.OpenWindow();
                       }));
            }
        }

        /// <summary>
        /// Возвращает команду закрытия приложения
        /// </summary>
        public RelayCommand<object> ExitCommand
        {
            get
            {
                return _exitCommand ??
                       (_exitCommand = new RelayCommand<object>(obj =>
                       {
                           ProjectManager.SaveToFile(_project, ProjectManager.DefaultPath);
                           var window = (MainWindow) obj;

                           // Если окно закрывается через крестик, то window = null
                           window?.Close();
                       }));
            }
        }

        /// <summary>
        /// Метод для заполнения списка заметок после и выбора текущей заметки
        /// после добавления, редактирования или удаления заметки
        /// </summary>
        private void FillNotesListAfterEdit(Note note)
        {
            if (SelectedCategory != null && SelectedCategory != "All")
            {
                Enum.TryParse(_selectedCategory, out NoteCategory category);
                CurrentDisplayedNotes = _project.LastChangeTimeSortWithCategory(category);

                if (note?.Category == category)
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

        /// <summary>
        /// Создает экземпляр класса <see cref="MainVM"/>
        /// </summary>
        /// <param name="noteWindowService"></param>
        /// <param name="aboutWindowService"></param>
        /// <param name="messageBoxService"></param>
        public MainVM(INoteWindowService noteWindowService, IAboutWindowService aboutWindowService,
            IMessageBoxService messageBoxService)
        {
            _project = ProjectManager.LoadFromFile(ProjectManager.DefaultPath);
            _noteWindowService = noteWindowService;
            _aboutWindowService = aboutWindowService;
            _messageBoxService = messageBoxService;

            Categories = Enum.GetNames(typeof(NoteCategory)).ToList();
            Categories.Add("All");

            CurrentDisplayedNotes = _project.LastChangeTimeSort();
            _project.Notes = CurrentDisplayedNotes;
            SelectedNote = _project.CurrentNote;
        }
    }
}
