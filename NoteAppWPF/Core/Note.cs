using System;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace Core
{
    /// <summary>
    /// Класс <see cref="Note"/>, хранящий информацию о заметке
    /// </summary>
    public class Note : ObservableObject, ICloneable, IDataErrorInfo
    {
        /// <summary>
        /// Название заметки. Название не должно превышать 50 символов.
        /// </summary>
        private string _name = "Без названия";
        
        /// <summary>
        /// Категория заметки.
        /// </summary>
        private NoteCategory _category;
        
        /// <summary>
        /// Текст заметки.
        /// </summary>
        private string _text;

        /// <summary>
        /// Время последнего изменения заметки
        /// </summary>
        private DateTime _lastChangeTime;

        /// <summary>
        /// Возвращает и задает название заметки
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                LastChangeTime = DateTime.Now;
            }
        }
        
        /// <summary>
        /// Возвращает и задает категорию заметки
        /// </summary>
        public NoteCategory Category
        {
            get => _category;
            set
            {
                _category = value;
                LastChangeTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Возвращает и задает текст заметки
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                LastChangeTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Возвращает и задает время создания заметки
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Возвращает и задает время последнего изменения заметки
        /// </summary>
        public DateTime LastChangeTime
        {
            get => _lastChangeTime;
            set
            {
                _lastChangeTime = value;
                RaisePropertyChanged(nameof(LastChangeTime));
            }
        }

        [JsonIgnore]
        /// <inheritdoc/>
        public string this[string columnName]
        {
            get
            {
                var error = String.Empty;
                switch (columnName)
                {
                    case nameof(Name):
                    {
                        if (Name.Length > 50)
                        {
                            error = "Название не должно превышать 50 символов.";
                        }
                        break;
                    }
                }

                return error;
            }
        }

        [JsonIgnore]
        /// <inheritdoc/>
        public string Error => string.Empty;

        /// <summary>
        /// Создает экземпляр <see cref="Note"/>
        /// </summary>
        public Note()
        {
            CreationTime = DateTime.Now;
            LastChangeTime = DateTime.Now;
        }

        /// <summary>
        /// Создает экземпляр <see cref="Note"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="category"></param>
        /// <param name="text"></param>
        public Note(string name, NoteCategory category, string text)
        {
            Name = name;
            Category = category;
            Text = text;
            CreationTime = DateTime.Now;
            LastChangeTime = DateTime.Now;
        }

        /// <summary>
        /// Создает экземпляр <see cref="Note"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="category"></param>
        /// <param name="text"></param>
        /// <param name="creationTime"></param>
        /// <param name="lastChangeTime"></param>
        [JsonConstructor]
        public Note(string name, NoteCategory category, string text, DateTime creationTime,
            DateTime lastChangeTime)
        {
            Name = name;
            Category = category;
            Text = text;
            CreationTime = creationTime;
            LastChangeTime = lastChangeTime;
        }

        /// <summary>
        /// Метод для создания копии объекта
        /// </summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Метод для сравнения значений двух объектов
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var note = (Note)obj;

            if (Name == note.Name && Category == note.Category && Text == note.Text &&
                CreationTime == note.CreationTime && LastChangeTime == note.LastChangeTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Name;
        }
    }
}
