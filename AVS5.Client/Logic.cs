using System.Collections.Generic;
using System.Linq;
using AVS5.Data;
using AVS5.Data.Dto;

namespace AVS5.Client
{
    internal class Logic
    {
        private IList<QuestionDto> _questionDtos;
        public bool IsLoaded { get; private set; }

        public void LoadData(IDataProvider<QuestionDto> dataProvider)
        {
            _questionDtos ??= dataProvider.GetData().ToArray();
            IsLoaded = true;
        }
    }
}