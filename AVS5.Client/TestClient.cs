using System;
using AVS5.Configuration;
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
            // FirstQuestion
            // IsRandomOrder
            // ShowResultInstantly
            // ShuffleThenTake
        }

        public void LoadData(IDataProvider<QuestionDto> dataProvider)
        {
            _logic.LoadData(dataProvider);
        }

        public void Setup()
        {
            if (!_logic.IsLoaded)
                throw new InvalidOperationException("Data was not loaded. Load data before invoking this method");
            // todo: setup
        }
    }
}