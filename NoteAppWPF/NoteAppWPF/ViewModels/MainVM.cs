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
        /// Выбранная заметка
        /// </summary>
        private Note _selectedNote;

        // TODO: Почему object? Хотя бы string, если надо совместить enum и string (DONE)
        /// <summary>
        /// Выбранная категория заметки
        /// </summary>
        private string _selectedCategory;

        /// <summary>
        /// Команда добавления новой заметки
        /// </summary>
        private RelayCommand<object> _addNoteCommand;

        /// <summary>
        /// Команда редактировани заметки
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
        // TODO: (_project закрытый, как тогда к CurrentNote биндиться?)
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

        // TODO: просто Categories, из контекста и так понятно какие категории (Done)
        // TODO: object ненадежно (Done)
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
                           // TODO: неправильное взаимодействие. Главное окно должно вызывать IWindowService, (DONE)
                           // передавая в него NoteVM. Мотивация - вызов конструктора VM не очевидно,
                           // что внутри него будет показываться форма.
                           var note = new Note();
                           var viewModel = new NoteVM(note);
                           var windowService = new NoteWindowService();
                           var result = windowService.OpenWindow(viewModel);

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

                           var viewModel = new NoteVM(note);
                           var windowService = new NoteWindowService();
                           var result = windowService.OpenWindow(viewModel);

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

                           var messageBoxService = new MessageBoxService();
                           var result = messageBoxService.ShowMessage(
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
                           var viewModel = new AboutVM();
                           var windowService = new AboutWindowService();
                           windowService.OpenWindow(viewModel);
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
                           ((MainWindow)obj).Close();
                       }));
            }
        }

        // TODO: программа иногда падает, если добавить заметки разных категорий, (DONE)
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
                Enum.TryParse(_selectedCategory, out NoteCategory category);
                CurrentDisplayedNotes = _project.LastChangeTimeSortWithCategory(category);

                // TODO: перечисления можно сравнивать без Equals (DONE)
                // TODO: linq? (Тут ведь нет коллекции, не из чего выбирать)
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
        public MainVM()
        {
            _project = ProjectManager.LoadFromFile(ProjectManager.DefaultPath);

            Categories = Enum.GetNames(typeof(NoteCategory)).ToList();
            Categories.Add("All");

            CurrentDisplayedNotes = _project.LastChangeTimeSort();
            _project.Notes = CurrentDisplayedNotes;
            SelectedNote = _project.CurrentNote;
        }
    }
}
