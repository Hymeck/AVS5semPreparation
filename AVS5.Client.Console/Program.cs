using System;
using System.Collections.Immutable;
using System.Linq;
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
            var (testConfig, consoleConfig) = ArgParser.Parse(args);
            var client = new TestClient(testConfig);

            // todo: check if specified file exists

            IDataProvider<QuestionDto> dataProvider = new QuestionDtoProvider(consoleConfig.File);
            client.LoadData(dataProvider);
            client.Setup();

            if (consoleConfig.DisplaySettings)
            {
                // todo: print settings
            }

            var questions = client.GetQuestions();
            var questionNumber = 1;
            foreach (var q in questions)
            {
                Write(q);
                userInput:
                Write("Введите номер ответа: ");
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

                        if (consoleConfig.DisplayResultInstantly)
                        {
                            var outputString = answer.IsRight
                                ? "Правильно."
                                : $"Неправильно. Правильный ответ: {string.Join(',', answer.RightAnswers)}";
                            WriteLine(outputString + "\n");
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


            var result = client.GetResult();
            var wrongCount = result.WrongQuestions.Count;
            var questionCount = questions.Count;
            var percentage = (int) result.Percentage;

            var answerHistory = client.GetAnswerHistory();
            WriteLine($"Выполнено {questionCount - wrongCount} из {questionCount} правильно ({percentage}%).\n");

            // todo: options: print list of wrong answers, exit
            WriteLine("1 - выйти.\n2 - вывести список неправильно отвеченных вопросов.");
            var choice = InputHandler.InputNumber(1, 2, ReadLine, WriteLine, "Некорректный ввод. Попробуйте еще раз");
            WriteLine();
            switch (choice)
            {
                case 2:
                    PrintWrongQuestions(result, answerHistory);
                    break;

                case 1:
                default: break;
            }
        }

        public static void PrintWrongQuestions(Result result, IImmutableList<UserAnswer> history)
        {
            if (result.WrongQuestions.Count == 0)
            {
                WriteLine("Ни единого неправильного ответа.");
                return;
            }
            
            foreach (var answer in history.Where(a => !a.IsRight))
            {
                Write(answer.Question);
                WriteLine($"Ваш ответ: {string.Join(',', answer.ChosenAnswers)}.");
                WriteLine($"Правильный ответ: {string.Join(',', answer.RightAnswers)}.");
            }
        }
    }
}