using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using AVS5.Configuration;
using AVS5.Core;
using AVS5.Data;
using AVS5.Data.Dto;

namespace AVS5.Client
{
    public class TestClient
    {
        public readonly TestConfiguration Configuration;
        private readonly Logic _logic;

        public TestClient(TestConfiguration configuration)
        {
            Configuration = configuration;
            _logic = new Logic();
        }

        public void LoadData(IDataProvider<QuestionDto> dataProvider)
        {
            _logic.LoadData(dataProvider);
        }

        /// <summary>
        /// Throws <see cref="InvalidOperationException"/> if data was not loaded.
        /// </summary>
        /// <exception cref="InvalidOperationException">Data was not loaded.</exception>
        private void CheckIsLoaded()
        {
            if (!_logic.IsLoaded)
                throw new InvalidOperationException("Data was not loaded. Load data before invoking this method.");
        }
        
        public void Setup()
        {
            if (_logic.IsConfigured)
                return;
            
            CheckIsLoaded();
            _logic.Setup(Configuration);
        }

        private void CheckIsSetup()
        {
            if (!_logic.IsConfigured)
                throw new InvalidOperationException("Data was not set up. Configure data before invoking this method.");
        }

        private void CheckQuestionNumber(int questionNumber)
        {
            if (questionNumber < 1 || questionNumber >= _logic.Pull.Questions.Count)
                throw new ArgumentOutOfRangeException(nameof(questionNumber), "Question number is out of range.");
        }
        
        public void AddAnswer(int questionNumber, IImmutableList<int> chosenAnswers)
        {
            CheckIsLoaded();
            CheckIsSetup();
            CheckQuestionNumber(questionNumber);
            
            var question = _logic.Pull.Questions[questionNumber];
            var userAnswer = new UserAnswer(question, chosenAnswers);
            _logic.AddAnswer(userAnswer);
        }

        public IImmutableList<BaseQuestion> GetQuestions()
        {
            CheckIsSetup();
            return _logic.Pull.Questions;
        }
    }
}