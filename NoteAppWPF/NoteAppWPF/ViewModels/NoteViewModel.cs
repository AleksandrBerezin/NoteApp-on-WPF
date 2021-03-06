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
        /// Результат работы окна
        /// </summary>
        private bool _dialogResult = false;

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
        /// Возвращает и задает действие при закрытии окна
        /// </summary>
        public Action CloseAction { get; set; }

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
                           var isError = (bool) obj;
                           if (!isError)
                           {
                               _dialogResult = true;
                               CloseAction?.Invoke();
                           }
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
                           CloseAction?.Invoke();
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
            window.ShowDialog();

            if (!_dialogResult)
            {
                note = null;
            }
        }
    }
}
