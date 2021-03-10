using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;

namespace Core.UnitTests
{
    [TestFixture]
    public class ProjectTest
    {
        /// <summary>
        /// Метод для создания списка заметок на 4 элемента
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<Note> GetExampleList()
        {
            return new ObservableCollection<Note>()
            {
                new Note("Заметка 1", NoteCategory.Home, "Текст 1"),
                new Note("Заметка 2", NoteCategory.Work, "Текст 2"),
                new Note("Заметка 3", NoteCategory.Documents, "Текст 3"),
                new Note("Заметка 4", NoteCategory.Work, "Текст 4")
            };
        }

        /// <summary>
        /// Метод для создания списка заметок на 4 элемента с учетом даты изменения
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<Note> GetExampleListWithDate()
        {
            return new ObservableCollection<Note>()
            {
                new Note("Заметка 1", NoteCategory.Home, "Текст 1",
                    DateTime.Parse("1/1/2020 00:00:00"),
                    DateTime.Parse("4/4/2020 00:00:00")),
                new Note("Заметка 2", NoteCategory.Work, "Текст 2",
                    DateTime.Parse("1/1/2020 00:00:00"),
                    DateTime.Parse("2/2/2020 00:00:00")),
                new Note("Заметка 3", NoteCategory.Documents, "Текст 3",
                    DateTime.Parse("1/1/2020 00:00:00"),
                    DateTime.Parse("1/1/2020 00:00:00")),
                new Note("Заметка 4", NoteCategory.Work, "Текст 4",
                    DateTime.Parse("1/1/2020 00:00:00"),
                    DateTime.Parse("3/3/2020 00:00:00"))
            };
        }

        [Test(Description = "Позитивный тест геттера Notes")]
        public void TestNotesGet_CorrectValue()
        {
            var expected = GetExampleList();
            var project = new Project();
            project.Notes = expected;
            var actual = project.Notes;

            CollectionAssert.AreEqual(expected, actual,
                "Геттер Notes возвращает неправильный список заметок");
        }

        [Test(Description = "Позитивный тест сеттера Notes")]
        public void TestNotesSet_CorrectValue()
        {
            var expected = GetExampleList();
            var project = new Project();
            project.Notes = expected;
            var actual = project.Notes;

            CollectionAssert.AreEqual(expected, actual,
                "Сеттер Notes присваивает неправильный список заметок");
        }

        [Test(Description = "Позитивный тест конструктора Project")]
        public void TestProjectConstructor_CorrectValue()
        {
            var project = new Project();
            var isNull = project == null;

            Assert.IsFalse(isNull, "Конструктор не создал экземпляр класса Project");
        }

        [Test(Description = "Позитивный тест сортировки списка по дате изменения")]
        public void TestLashChangeTimeSort_CorrectValue()
        {
            var project = new Project();
            var expectedList = new ObservableCollection<Note>()
            {
                new Note("Заметка 1", NoteCategory.Home, "Текст 1",
                    DateTime.Parse("1/1/2020 00:00:00"),
                    DateTime.Parse("4/4/2020 00:00:00")),
                new Note("Заметка 4", NoteCategory.Work, "Текст 4",
                    DateTime.Parse("1/1/2020 00:00:00"),
                    DateTime.Parse("3/3/2020 00:00:00")),
                new Note("Заметка 2", NoteCategory.Work, "Текст 2",
                    DateTime.Parse("1/1/2020 00:00:00"),
                    DateTime.Parse("2/2/2020 00:00:00")),
                new Note("Заметка 3", NoteCategory.Documents, "Текст 3",
                    DateTime.Parse("1/1/2020 00:00:00"),
                    DateTime.Parse("1/1/2020 00:00:00"))
            };

            var actualList = GetExampleListWithDate();

            project.Notes = actualList;
            actualList = project.LastChangeTimeSort();

            Assert.AreEqual(expectedList, actualList,
                "Список должен сортироваться по дате изменения");
        }

        [Test(Description = "Позитивный тест сортировки списка по дате изменения" +
                            " при определенной категории")]
        public void TestLastChangeTimeSortWithCategory_CorrectValue()
        {
            var project = new Project();
            var expectedList = new List<Note>()
            {
                new Note("Заметка 4", NoteCategory.Work, "Текст 4",
                    DateTime.Parse("1/1/2020 00:00:00"),
                    DateTime.Parse("3/3/2020 00:00:00")),
                new Note("Заметка 2", NoteCategory.Work, "Текст 2",
                    DateTime.Parse("1/1/2020 00:00:00"),
                    DateTime.Parse("2/2/2020 00:00:00"))
            };

            var actualList = GetExampleListWithDate();

            project.Notes = actualList;
            actualList = project.LastChangeTimeSortWithCategory(NoteCategory.Work);

            Assert.AreEqual(expectedList, actualList,
                "Список должен сортироваться по дате изменения, " +
                "должны учитываться значения, соответствующие указанной категории");
        }

        [Test(Description = "Позитивный тест сортировки списка по дате изменения " +
                            "при определенной категории, при отсутствии элементов " +
                            "нужной категории")]
        public void TestLastChangeTimeSortWithCategory_NoRightCategory()
        {
            var project = new Project();
            var expectedList = new List<Note>();
            var actualList = GetExampleListWithDate();

            project.Notes = actualList;
            actualList = project.LastChangeTimeSortWithCategory(NoteCategory.HealthAndSport);

            Assert.AreEqual(expectedList, actualList,
                "Метод LastChangeTimeSort должен вернуть пустой список " +
                "при отсутствии элементов нужной категории");
        }

        [Test(Description = "Позитивный тест геттера CurrentNote")]
        public void TestCurrentNoteGet_CorrectValue()
        {
            var expected = new Note("Новая заметка", NoteCategory.Home, "Текст заметки");
            var project = new Project();
            project.CurrentNote = expected;
            var actual = project.CurrentNote;

            Assert.AreEqual(expected, actual,
                "Геттер CurrentNote возвращает неправильную текущую заметку");
        }

        [Test(Description = "Позитивный тест сеттера CurrentNote")]
        public void TestCurrentNoteSet_CorrectValue()
        {
            var expected = new Note("Новая заметка", NoteCategory.Home, "Текст заметки");
            var project = new Project();
            project.CurrentNote = expected;
            var actual = project.CurrentNote;

            Assert.AreEqual(expected, actual,
                "Сеттер CurrentNote возвращает неправильную текущую заметку");
        }
    }
}