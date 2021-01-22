using System;
using System.Linq;
using AVS5.Configuration;
using AVS5.Core;
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
            var questionNumber = 1;
            foreach (var q in questions)
            {
                WriteLine(q);
                userInput:
                var userAnswers = ReadLine();
                if (string.IsNullOrEmpty(userAnswers) || string.IsNullOrWhiteSpace(userAnswers))
                {
                    // todo: enter?
                    WriteLine("Неправильный ввод. Повторите еще раз.");
                    goto userInput;
                }
                else
                {
                    var input = userAnswers.Split(' ', ',', StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        var parsedAnswers = input.Select(int.Parse);
                        client.AddAnswer(questionNumber, parsedAnswers);
                    }
                    catch (Exception)
                    {
                        WriteLine("Неправильный ввод. Повторите еще раз.");
                        goto userInput;
                    }
                }

                questionNumber++;
            }

            var result = client.GetResult();
            WriteLine(result);
        }
    }
}