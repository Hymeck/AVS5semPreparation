using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AVS5.Data;
using AVS5.Data.Dto;
using NUnit.Framework;

namespace AVS5.Test
{
    public class QuestionProviderTest
    {
        private IDataProvider<QuestionDto> _data;
        private IList<QuestionDto> _questions;
        private string _location = "avs_demo.txt";
        
        [SetUp]
        public void Setup()
        {
            _data = new QuestionDtoProvider(_location);
            _questions = _data.GetData().ToList();
        }

        [Test]
        public void QuestionCount()
        {
            Assert.AreEqual(399, _questions.Count);
        }

        [Test]
        public void FirstQuestionText()
        {
            var firstText = "Множество знаков, в котором определен линейный порядок - ...";
            Assert.AreEqual(firstText, _questions[0].Text.Trim());
        }

        // why does this test fail?
        [Test]
        public void FirstQuestionAnswers()
        {
            var actualAnswers = _questions[0].Answers.ToImmutableList();
            IList<string> expectedAnswers = new List<string>(5)
            {
                "Таблица",
                "Сигнал",
                "Алфавит",
                "Список",
                "Очередь"
            };
        
            var isEqual = expectedAnswers
                .Where((t, i) => t.Equals(actualAnswers[i]))
                .Any();
            
            Assert.AreEqual(true, isEqual);
        }
    }
}