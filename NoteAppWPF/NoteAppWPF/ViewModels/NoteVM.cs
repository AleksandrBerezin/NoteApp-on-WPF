using System;
using System.Collections.Generic;
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
    /// Модель-представление окна <see cref="NoteWindow"/>
    /// </summary>
    public class NoteVM : ViewModelBase
    {
        /// <summary>
        /// Текущая заметка
        /// </summary>
        private Note _currentNote;

        /// <summary>
        /// Команда закрытия окна с сохранением данных
        /// </summary>
        private RelayCommand<object> _okCommand;

        /// <summary>
        /// Команда закрытия окна без сохранения данных
        /// </summary>
        private RelayCommand<object> _cancelCommand;

        /// <summary>
        /// Сервис для работы с окном вывода сообщения
        /// </summary>
        private readonly IMessageBoxService _messageBoxService;

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
        /// Результат работы окна редактирования заметки
        /// </summary>
        public bool? DialogResult { get; private set; }

        /// <summary>
        /// Возвращает и задает действие при закрытии окна
        /// </summary>
        public Action CloseAction { get; set; }
        
        /// <summary>
        /// Возвращает команду закрытия окна с сохранением данных
        /// </summary>
        public RelayCommand<object> OkCommand
        {
            get
            {
                return _okCommand ??
                       (_okCommand = new RelayCommand<object>(obj =>
                       {
                           var isError = (bool) obj;
                           if (isError)
                           {
                               // TODO: никакого создания кокнретных View на уровне VM (DONE)
                               _messageBoxService.ShowMessage("Invalid values entered", "Error",
                                   MessageBoxButton.OK, MessageBoxImage.Error);
                           }
                           else
                           {
                               DialogResult = true;
                               CloseAction?.Invoke();
                           }
                       }));
            }
        }

        /// <summary>
        /// Возвращает команду закрытия окна без сохранения данных
        /// </summary>
        public RelayCommand<object> CancelCommand
        {
            get
            {
                return _cancelCommand ??
                       (_cancelCommand = new RelayCommand<object>(obj =>
                       {
                           DialogResult = false;
                           CloseAction?.Invoke();
                       }));
            }
        }

        /// <summary>
        /// Возвращает список категорий заметок
        /// </summary>
        public List<NoteCategory> Categories { get; private set; }

        /// <summary>
        /// Создает экземпляр класса <see cref="NoteVM"/>
        /// </summary>
        /// <param name="note"></param>
        /// <param name="messageBoxService"></param>
        public NoteVM(Note note, IMessageBoxService messageBoxService)
        {
            _messageBoxService = messageBoxService;
            CurrentNote = note;
            Categories = Enum.GetValues(typeof(NoteCategory)).Cast<NoteCategory>().ToList();
        }
    }
}
