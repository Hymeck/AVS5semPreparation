using System.Collections.Generic;
using AVS5.Configuration;
using AVS5.Core;
using AVS5.Data;
using AVS5.Data.Dto;

namespace AVS5.Client
{
    internal sealed class Logic
    {
        private IEnumerable<QuestionDto> _questionDtos;
        public bool IsLoaded => _questionDtos != null;
        public bool IsConfigured => Pull != null;
        public QuestionPull Pull { get; private set; }
        public AnswerState AnswerState { get; private set; } = AnswerState.Empty;

        public void LoadData(IDataProvider<QuestionDto> dataProvider) => 
            _questionDtos ??= dataProvider.GetData();

        public void Setup(TestingConfiguration configuration) => 
            Pull ??= QuestionPull.GetConfiguredInstance(QuestionBuilder
                .ToQuestion(_questionDtos), configuration);

        public void AddAnswer(UserAnswer userAnswer) => 
            AnswerState += userAnswer;
    }
}