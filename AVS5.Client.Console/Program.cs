using AVS5.Configuration;
using AVS5.Data;
using AVS5.Data.Dto;
using static System.Console;

namespace AVS5.Client.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new TestConfiguration(questionCount: 2, firstQuestion: 2);
            var client = new TestClient(config);

            IDataProvider<QuestionDto> dataProvider = new QuestionDataProvider("avs_demo.txt");
            client.LoadData(dataProvider);
            client.Setup();
            
            var questions = client.GetQuestions();
            foreach (var q in questions) 
                WriteLine(q);
        }
    }
}