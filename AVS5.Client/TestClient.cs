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

        public void LoadData(IDataProvider<QuestionDto> dataProvider) => 
            _logic.LoadData(dataProvider);

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
            if (questionNumber < 1 || questionNumber > _logic.Pull.Questions.Count)
                throw new ArgumentOutOfRangeException(nameof(questionNumber), "Question number is out of range.");
        }

        public UserAnswer AddAnswer(int questionNumber, IEnumerable<int> chosenAnswers)
        {
            CheckIsSetup();
            CheckQuestionNumber(questionNumber);

            var question = _logic.Pull.Questions[questionNumber - 1];
            var userAnswer = new UserAnswer(question, chosenAnswers.ToImmutableList());
            _logic.AddAnswer(userAnswer);
            return userAnswer;
        }

        public IImmutableList<BaseQuestion> GetQuestions()
        {
            CheckIsSetup();
            return _logic.Pull.Questions;
        }

        public Result GetResult()
        {
            CheckIsSetup();
            return _logic.AnswerState.Result;
        }
    }
}