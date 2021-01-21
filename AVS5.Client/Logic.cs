using System.Collections.Immutable;
using AVS5.Configuration;
using AVS5.Core;
using AVS5.Data;
using AVS5.Data.Dto;

namespace AVS5.Client
{
    internal sealed class Logic
    {
        private IImmutableList<QuestionDto> _questionDtos;
        public bool IsLoaded { get; private set; }
        public bool IsConfigured { get; private set; }
        public QuestionPull Pull { get; private set; }

        public void LoadData(IDataProvider<QuestionDto> dataProvider)
        {
            if (IsLoaded)
                return;
            
            _questionDtos ??= dataProvider.GetData().ToImmutableList();
            IsLoaded = true;
        }

        public void Setup(TestConfiguration configuration)
        {
            if (IsConfigured)
                return;
            
            Pull ??= QuestionPull.GetConfiguredInstance(QuestionBuilder.ToQuestion(_questionDtos), configuration);
            IsConfigured = true;
        }

        public void AddAnswer(UserAnswer userAnswer)
        {
            throw new System.NotImplementedException();
        }
    }
}