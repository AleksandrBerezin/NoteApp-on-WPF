using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Core;
// TODO: если подключил библиотеку, тогда используй её во всех VM и реализациях INPC
using GalaSoft.MvvmLight;
using NoteAppWPF.Services;

namespace NoteAppWPF.ViewModels
{
    // TODO: В именах классов использовать сокращение VM (в названиях папок оставлять без сокращения)
    /// <summary>
    /// Модель-представление окна <see cref="NoteWindow"/>
    /// </summary>
    public class NoteViewModel : ViewModelBase
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
        ///Сервис вывода сообщения
        /// </summary>
        private MessageBoxService _messageBoxService;

        /// <summary>
        /// Сервис открытия окна
        /// </summary>
        private WindowService _noteWindowService;

        /// <summary>
        /// Возвращает и задает текущую заметку
        /// </summary>
        public Note CurrentNote
        {
            get => _currentNote;
            set
            {
                _currentNote = value;
                RaisePropertyChanged(nameof(CurrentNote));
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
                           if (isError)
                           {
                               _messageBoxService.ShowMessage("Invalid values entered", "Error",
                                   MessageBoxButton.OK, MessageBoxImage.Error);
                           }
                           else
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

        // TODO: сделать лист NoteCategory, а биндинг через конвертер 
        /// <summary>
        /// Возвращает список категорий заметок
        /// </summary>
        public List<string> NoteCategories { get; private set; }

        // TODO: xml
        public NoteViewModel(ref Note note)
        {
            _messageBoxService = new MessageBoxService();
            CurrentNote = note;
            NoteCategories = Enum.GetNames(typeof(NoteCategory)).ToList();

            // TODO: WindowService должен хранить ссылку на VM, а не VM хранить сервис
            // т.е. сервис хранится в том окне, откуда он вызывается, а вызываться он должен из главного окна
            // иначе не очевидно, что при вызове конструктора будет загораться окно, да и связка жесткая
            _noteWindowService = new WindowService();
            _noteWindowService.OpenWindow(this);

            if (!_dialogResult)
            {
                note = null;
            }
        }
    }
}
