using System;
using NUnit.Framework;

namespace Core.UnitTests
{
    [TestFixture]
    public class NoteTest
    {
        [Test(Description = "Позитивный тест геттера Name")]
        public void TestNameGet_CorrectValue()
        {
            var expected = "Новая заметка";
            var note = new Note();
            note.Name = expected;
            var actual = note.Name;

            Assert.AreEqual(expected, actual, "Геттер Name возвращает неправильное название");
        }

        [Test(Description = "Позитивный тест сеттера Name")]
        public void TestNameSet_CorrectValue()
        {
            var expected = "Новая заметка";
            var note = new Note();
            note.Name = expected;
            var actual = note.Name;

            Assert.AreEqual(expected, actual, "Сеттер Name присваивает неправильное название");
        }

        [Test(Description = "Позитивный тест геттера Category")]
        public void TestCategoryGet_CorrectValue()
        {
            var expected = NoteCategory.Work;
            var note = new Note();
            note.Category = expected;
            var actual = note.Category;

            Assert.AreEqual(expected, actual, "Геттер Category возвращает неправильную категорию");
        }

        [Test(Description = "Позитивный тест сеттера Category")]
        public void TestCategorySet_CorrectValue()
        {
            var expected = NoteCategory.Work;
            var note = new Note();
            note.Category = expected;
            var actual = note.Category;

            Assert.AreEqual(expected, actual, "Сеттер Category присваивает неправильную категорию");
        }

        [Test(Description = "Позитивный тест геттера Text")]
        public void TestTextGet_CorrectValue()
        {
            var expected = "Текст заметки";
            var note = new Note();
            note.Text = expected;
            var actual = note.Text;

            Assert.AreEqual(expected, actual, "Геттер Text возвращает неправильный текст");
        }

        [Test(Description = "Позитивный тест сеттера Text")]
        public void TestTextSet_CorrectValue()
        {
            var expected = "Текст заметки";
            var note = new Note();
            note.Text = expected;
            var actual = note.Text;

            Assert.AreEqual(expected, actual, "Сеттер Text присваивает неправильный текст");
        }

        [Test(Description = "Позитивный тест геттера CreationTime")]
        public void TestCreationTimeGet_CorrectValue()
        {
            var expected = new DateTime();
            var note = new Note("Новая заметка", NoteCategory.Home, "Текст заметки",
                new DateTime(), new DateTime());
            var actual = note.CreationTime;

            Assert.AreEqual(expected, actual,
                "Геттер CreationTime возвращает неправильное время создания");
        }

        [Test(Description = "Позитивный тест геттера LastChangeTime")]
        public void TestLastChangeTimeGet_CorrectValue()
        {
            var expected = new DateTime();
            var note = new Note("Новая заметка", NoteCategory.Home, "Текст заметки",
                new DateTime(), new DateTime());
            var actual = note.LastChangeTime;

            Assert.AreEqual(expected, actual,
                "Геттер LastChangeTime возвращает неправильное время последнего изменения");
        }

        [Test(Description = "Тест конструктора Note без параметров")]
        public void TestNoteConstructor_WithoutParameters()
        {
            var note = new Note();
            var isNull = note == null;

            Assert.IsFalse(isNull, "Конструктор не создал экземпляр класса Note");
        }

        [Test(Description = "Тест конструктора Note без дат")]
        public void TestNoteConstructor_WithoutDate()
        {
            var note = new Note("Новая заметка", NoteCategory.Home, "Текст заметки");
            var isNull = note == null;

            Assert.IsFalse(isNull, "Конструктор не создал экземпляр класса Note");
        }

        [Test(Description = "Тест конструктора Note с датами")]
        public void TestNoteConstructor_WithDate()
        {
            var note = new Note("Новая заметка", NoteCategory.Home, "Текст заметки",
                DateTime.Now, DateTime.Now);
            var isNull = note == null;

            Assert.IsFalse(isNull, "Конструктор не создал экземпляр класса Note");
        }

        [Test(Description = "Тест метода копирования")]
        public void TestClone_CorrectValue()
        {
            var note = new Note("Новая заметка", NoteCategory.Home, "Текст заметки");
            var clonedNote = (Note)note.Clone();
            var isEqual = clonedNote.Equals(note);

            Assert.IsFalse(!isEqual, "Метод копирования должен создать точную копию объекта");
        }

        [Test(Description = "Тест метода сравнения двух объектов")]
        public void TestEquals_CorrectValue()
        {
            var note = new Note("Новая заметка", NoteCategory.Home, "Текст заметки");
            var clonedNote = (Note)note.Clone();
            var isEqual = clonedNote.Equals(note);

            Assert.IsFalse(!isEqual,
                "Метод сравнения должен вернуть истину, так как объекты идентичны");
        }
    }
}