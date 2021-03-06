using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using usantatecla.sudoku.controllers;
using usantatecla.sudoku.controllers.loaders;

namespace usantatecla.sudoku.controllers.loaders
{
    public class RandomFileSudokuLoaderTest
    {
        private const string TEMPLATE_ONE = "142.9...57..4...898.5....242....48...3...126..8..72941.5.2.6....28..941..797.853.";
        private const string TEMPLATE_TWO = "7...2.48.2.6..8..55..9........15.....2.....6.....67........6..36..5..1.4.93.4...7";

        private Mock<FakeRandomGenerator> _mockedRandom;
        private FileSudokuLoader _sut;

        [SetUp]
        public void Setup() {
            _mockedRandom = new Mock<FakeRandomGenerator>();
            _sut = new FileSudokuLoader(_mockedRandom.Object);
        }

        [Test]
        public void Given_RandomFileSudokuLoader_WhenLoad_ThenLoadRandomTemplate()
        {
            _mockedRandom
                .SetupSequence(x => x.Next(It.IsAny<Int32>()))
                .Returns(1)
                .Returns(2);

            var loadedTemplate = _sut.Load();
            Assert.AreEqual(TEMPLATE_ONE, loadedTemplate);

            loadedTemplate = _sut.Load();
            Assert.AreEqual(TEMPLATE_TWO, loadedTemplate);
        }

        [Test]
        public void Given_RandomFileSudokuLoader_WhenLoad_ThenSudokuTemplateHasValidFormat()
        {
            string template = _sut.Load();

            var pattern = @"[.123456789]{81}";
            var regex = new Regex(pattern);
            var formatValidator = regex.Match(template);

            Assert.True(formatValidator.Success);
        }

        [Test]
        public void Given_RandomFileSudokuLoader_WhenLoad1000times_ThenHasLoadAllTemplates()
        {
            _sut = new FileSudokuLoader();

            var loadedTemplates = Load1000Templates();

            var expected = _sut.GetTemplateCount();

            Assert.AreEqual(loadedTemplates.Distinct().Count(), expected);
        }


        private List<string> Load1000Templates() {

            var readedTemplates = new List<string>();

            for (int i = 0; i < 1000; i++)
            {
                var template = _sut.Load();
                readedTemplates.Add(template);
            }

            return readedTemplates;
        }

    }

    public class FakeRandomGenerator : IValueGenerator {

        public virtual int Next(int max) => 0;
    }

}
