using System;
using System.Collections.Generic;
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
            // var (testConfig, consoleConfig) = ArgParser.Parse(args);
            //
            // // todo: check if specified file exists
            //
            // var client = new TestClient(testConfig);
            //
            // IDataProvider<QuestionDto> dataProvider = new QuestionDtoProvider("avs_demo.txt");
            // client.LoadData(dataProvider);
            // client.Setup();
            //
            // if (consoleConfig.DisplaySettings)
            // {
            //     // todo: print settings
            // }
            //
            // var questions = client.GetQuestions();
            // var questionNumber = 1;
            // foreach (var q in questions)
            // {
            //     // 1. независим
            //     Write(q);
            //     
            //     // 2. независим
            //     userInput:
            //     Write("Введите номер ответа: ");
            //     var input = ReadLine();
            //     if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
            //     {
            //         WriteLine("Невалидный ввод. Повторите еще раз.");
            //         goto userInput;
            //     }
            //     else
            //     {
            //         var splitInput = input.Split(' ', ',', StringSplitOptions.RemoveEmptyEntries);
            //         try
            //         {
            //             var chosenAnswers = splitInput.Select(int.Parse);
            //             
            //             // 3. зависит от 2.
            //             var answer = client.AddAnswer(questionNumber, chosenAnswers);
            //
            //             // 4. зависит от конфигурации
            //             if (consoleConfig.DisplayResultInstantly)
            //             {
            //                 var outputString = answer.IsRight
            //                     ? "Правильно."
            //                     : $"Неправильно. Правильный ответ: {string.Join(',', answer.RightAnswers)}";
            //                 WriteLine(outputString);
            //             }
            //             WriteLine();
            //         }
            //         catch (Exception)
            //         {
            //             WriteLine("Невалидный ввод. Повторите еще раз.");
            //             goto userInput;
            //         }
            //     }
            //
            //     questionNumber++;
            // }


            // var result = client.GetResult();
            var result = GetResult(args);
            var wrongCount = result.WrongAnswers.Count;
            var questionCount = result.AnswerHistory.Count;
            var percentage = (int) result.Percentage;

            WriteLine($"Выполнено {questionCount - wrongCount} из {questionCount} правильно ({percentage}%).\n");
            
            WriteLine("1 - выйти.\n2 - вывести список неправильно отвеченных вопросов.");
            var choice = InputHandler.InputNumber(1, 2, ReadLine, WriteLine, "Некорректный ввод. Попробуйте еще раз.");
            WriteLine();
            switch (choice)
            {
                case 2:
                    PrintWrongQuestions(result);
                    break;

                case 1:
                default: break;
            }
        }

        public static IEnumerable<int> ParseInput()
        {
            userInput:
            var input = ReadLine();
            Write("Введите номер ответа: ");
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
            {
                WriteLine("Была введена пустая строка или строка, состоящая целиком из пробелов. Повторите еще раз.");
                goto userInput;
            }
            
            var splitInput = input.Split(' ', ',', StringSplitOptions.RemoveEmptyEntries);

            try
            {
                return splitInput.Select(int.Parse);
            }

            catch (FormatException)
            {
                WriteLine("Какое-то из чисел было введено в неверном формате. Повторите еще раз.");
                goto userInput;
            }

            catch (OverflowException)
            {
                WriteLine("Какое-то из чисел оказалось слишком большое. Повторите еще раз.");
                goto userInput;
            }
        }
        
        public static IEnumerable<(int questionNumber, IEnumerable<int> chosenAnswers)> 
            PerformInputOfUserAnswers(IImmutableList<BaseQuestion> questions)
        {
            var questionNumber = 1;
            foreach (var question in questions)
            {
                Write(question);
                var chosenAnswers = ParseInput();
                yield return (questionNumber, chosenAnswers);
                questionNumber++;
            }
        }

        public static string GetString(UserAnswer answer) => answer.IsRight
            ? "Правильно."
            : $"Неправильно. Правильный ответ: {string.Join(',', answer.RightAnswers)}";

        public static void PrintUserAnswer(UserAnswer answer) => WriteLine(GetString(answer));

        public static Action GetWriteLine() => WriteLine;
        
        public static Result GetResult(string[] args)
        {
            var (testConfig, consoleConfig) = ArgParser.Parse(args);
            
            // todo: check if specified file exists
            
            var client = new TestClient(testConfig);
            
            IDataProvider<QuestionDto> dataProvider = new QuestionDtoProvider("avs_demo.txt");
            client.LoadData(dataProvider);
            client.Setup();

            Action<UserAnswer> print = WriteLine;
            
            if (consoleConfig.DisplayResultInstantly)
                print = PrintUserAnswer + print;
            
            var questions = client.GetQuestions();
            foreach (var answer in PerformUserAnswer(client, questions))
            {
                print(answer);
            }

            return client.GetResult();
        }

        public static IEnumerable<UserAnswer> PerformUserAnswer(TestClient client, IImmutableList<BaseQuestion> questions)
        {
            foreach (var (n, cA) in PerformInputOfUserAnswers(questions))
            {
                var userAnswer = client.AddAnswer(n, cA);
                yield return userAnswer;
            }
        }
        
        public static void PrintWrongQuestions(Result result)
        {
            if (result.WrongAnswers.Count == 0)
            {
                WriteLine("Ни единого неправильного ответа.");
                return;
            }
            
            foreach (var answer in result.WrongAnswers)
            {
                Write(answer.Question);
                WriteLine($"Ваш ответ: {string.Join(',', answer.ChosenAnswers)}.");
                WriteLine($"Правильный ответ: {string.Join(',', answer.RightAnswers)}.");
            }
        }
    }
}