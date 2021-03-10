using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace Core.UnitTests
{
    [TestFixture]
    public class ProjectManagerTest
    {
        /// <summary>
        /// Путь к файлу
        /// </summary>
        private string _defaultPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
            "\\NoteApp\\NoteApp.notes";

        /// <summary>
        /// Метод для создания экземпляра проекта, содержащего список из 4 элементов
        /// </summary>
        /// <returns></returns>
        private Project GetExampleProject()
        {
            var project = new Project();
            project.Notes.Add(new Note("Заметка 1", NoteCategory.Home, "Текст1",
                new DateTime(), new DateTime()));
            project.Notes.Add(new Note("Заметка 2", NoteCategory.Home, "Текст2",
                new DateTime(), new DateTime()));
            project.Notes.Add(new Note("Заметка 3", NoteCategory.Home, "Текст3",
                new DateTime(), new DateTime()));
            project.Notes.Add(new Note("Заметка 4", NoteCategory.Home, "Текст4",
                new DateTime(), new DateTime()));

            project.CurrentNote = project.Notes[0];

            return project;
        }

        [Test(Description = "Позитивный тест геттера DefaultPath")]
        public void TestDefaultPathGet_CorrectValue()
        {
            var expected
                = _defaultPath;
            ProjectManager.DefaultPath = expected;
            var actual = ProjectManager.DefaultPath;

            Assert.AreEqual(expected, actual, "Геттер DefaultPath возвращает неправильный путь");
        }

        [Test(Description = "Позитивный тест сеттера DefaultPath")]
        public void TestDefaultPathSet_CorrectValue()
        {
            var expected = _defaultPath;
            ProjectManager.DefaultPath = expected;
            var actual = ProjectManager.DefaultPath;

            Assert.AreEqual(expected, actual, "Сеттер DefaultPath присваивает неправильный путь");
        }

        [Test(Description = "Позитивный тест метода сохранения данных в файл")]
        public void TestSaveToFile_CorrectValue()
        {
            var project = GetExampleProject();

            var location = Assembly.GetExecutingAssembly().Location;

            var testDataLocation = Path.GetFullPath(location + "\\..\\TestData\\Test.txt");
            var referenceDataLocation =
                Path.GetFullPath(location + "\\..\\TestData\\Reference.txt");

            if (File.Exists(testDataLocation))
            {
                File.Delete(testDataLocation);
            }

            ProjectManager.SaveToFile(project, testDataLocation);
            Assert.IsTrue(File.Exists(testDataLocation),
                "Файл для сохранения данных не был создан");

            var expectedFileAsText = File.ReadAllText(testDataLocation);
            var actualFileAsText = File.ReadAllText(referenceDataLocation);

            Assert.AreEqual(expectedFileAsText, actualFileAsText,
                "Метод SaveToFile сохраняет данные неверно");
        }

        [Test(Description = "Позитивный тест метода загрузки данных из файла")]
        public void TestLoadFromFile_CorrectValue()
        {
            var expectedProject = GetExampleProject();

            var location = Assembly.GetExecutingAssembly().Location;
            var referenceDataLocation =
                Path.GetFullPath(location + "\\..\\TestData\\Reference.txt");

            var actualProject = ProjectManager.LoadFromFile(referenceDataLocation);

            CollectionAssert.AreEqual(expectedProject.Notes, actualProject.Notes,
                "Метод LoadFromFile загружает данные неверно");
        }

        [Test(Description = "Тест метода загрузки данных из файла при отсутствии файла")]
        public void TestLoadFromFile_NoFile()
        {
            var expectedProject = new Project();
            var location = Assembly.GetExecutingAssembly().Location;
            var testDataLocation = Path.GetFullPath(location + "\\..\\TestData\\Test.txt");

            if (File.Exists(testDataLocation))
            {
                File.Delete(testDataLocation);
            }

            var actualProject = ProjectManager.LoadFromFile(testDataLocation);

            CollectionAssert.AreEqual(expectedProject.Notes, actualProject.Notes,
                "Метод LoadFromFile не создает новый объект Project при отсутствии файла");
        }

        [Test(Description = "Тест метода загрузки данных из поврежденного файла")]
        public void TestLoadFromFile_CorruptedFile()
        {
            var expectedProject = new Project();
            var location = Assembly.GetExecutingAssembly().Location;
            var corruptedDataLocation =
                Path.GetFullPath(location + "\\..\\TestData\\Corrupted.txt");

            var actualProject = ProjectManager.LoadFromFile(corruptedDataLocation);

            CollectionAssert.AreEqual(expectedProject.Notes, actualProject.Notes,
                "Метод LoadFromFile не создает новый объект Project " +
                "при поврежденном файле");
        }
    }
}