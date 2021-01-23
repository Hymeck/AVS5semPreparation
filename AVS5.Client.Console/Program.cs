using System;
using System.Linq;
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
            // todo: parse arguments
            var config = new TestConfiguration(questionCount: 2, firstQuestion: 2);
            var client = new TestClient(config);

            IDataProvider<QuestionDto> dataProvider = new QuestionDtoProvider("avs_demo.txt");
            client.LoadData(dataProvider);
            client.Setup();
            
            // todo: print settings
            
            var questions = client.GetQuestions();
            var questionNumber = 1;
            foreach (var q in questions)
            {
                WriteLine(q); 
                userInput:
                var userAnswers = ReadLine();
                if (string.IsNullOrEmpty(userAnswers) || string.IsNullOrWhiteSpace(userAnswers))
                {
                    WriteLine("Неправильный ввод. Повторите еще раз.");
                    goto userInput;
                }
                else
                {
                    var input = userAnswers.Split(' ', ',', StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        var parsedAnswers = input.Select(int.Parse);
                        var answer = client.AddAnswer(questionNumber, parsedAnswers);

                        if (client.Configuration.ShowResultInstantly)
                        {
                            var outputString = answer.IsRight
                                ? "Правильно."
                                : $"Неправильно. Правильный ответ: {string.Join(',', answer.RightAnswers)}";
                            WriteLine(outputString);
                        }
                    }
                    catch (Exception)
                    {
                        WriteLine("Неправильный ввод. Повторите еще раз.");
                        goto userInput;
                    }
                }

                questionNumber++;
            }

            // todo: print verbose result
            // todo: options: list of wrong answers, exit, list of all test question
            
            var result = client.GetResult();
            WriteLine(result);
        }
    }
}