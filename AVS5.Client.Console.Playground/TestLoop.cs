using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AVS5.Configuration;
using AVS5.Core;
using AVS5.Data;
using AVS5.Data.Dto;
using static System.Console;
using static AVS5.Client.Console.Playground.PunctuationConstants;
using static AVS5.Client.Console.Playground.MessageConstants;

namespace AVS5.Client.Console.Playground
{
    // api
    public sealed partial class TestLoop
    {
        private readonly string[] _args;
        
        public TestLoop(string[] args) => 
            _args = args;

        public void Start()
        {
            var (testConfig, consoleConfig) = ArgParser.Parse(_args);
            // todo: check if specified file exists
            if (consoleConfig.DisplaySettings)
            {
                // todo: print settings
            }

            var client = SetupClient(testConfig);
            var result = PerformTestingAndGetResult(client, consoleConfig);
            PrintWrongAnswers(result);
        }
        
    }

    // logic methods
    public sealed partial class TestLoop
    {
        private static TestClient SetupClient(TestingConfiguration testConfig)
        {
            var client = new TestClient(testConfig);
            
            IDataProvider<QuestionDto> dataProvider = new QuestionDtoProvider("avs_demo.txt");
            client.LoadData(dataProvider);
            client.Setup();

            return client;
        }
        
        private static IEnumerable<int> ProvideAndParseInput()
        {
            userInput:
            Write(InputNumber + Colon + Space);
            var input = ReadLine();
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
            {
                WriteLine(NullOrEmptyError + Point + Space + RepeatAgain + Point);
                goto userInput;
            }
            
            var splitInput = input.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                // todo: correct (fails when input > int32.MaxValue)
                var chosenAnswers = splitInput.Select(int.Parse);
                return chosenAnswers;
            }

            catch (OverflowException)
            {
                WriteLine(OverflowInputError + Point + Space + Point);
                goto userInput;
            }

            catch (FormatException)
            {
                WriteLine(WrongInputFormatError + Point + Space + Point);
                goto userInput;
            }
        }

        private static Result PerformTestingAndGetResult(TestClient client, ConsoleConfiguration consoleConfig)
        {
            var answerOutput = GetAnswerOutput(consoleConfig.DisplayResultInstantly);

            var questions = client.GetQuestions();
            foreach (var (n, cA) in PerformInputOfUserAnswers(questions))
            {
                var userAnswer = client.AddAnswer(n, cA);
                Write(answerOutput(userAnswer));
            }
            
            return client.GetResult();
        }

        private static IEnumerable<(int questionNumber, IEnumerable<int> chosenAnswers)> 
            PerformInputOfUserAnswers(IImmutableList<BaseQuestion> questions)
        {
            var questionNumber = 1;
            foreach (var question in questions)
            {
                Write(question);
                var chosenAnswers = ProvideAndParseInput();
                yield return (questionNumber, chosenAnswers);
                questionNumber++;
            }
        }
        
        private static void PrintWrongAnswers(Result result)
        {
            WriteLine(MessageConstants.Result + Point);
            if (result.WrongAnswers.Count == 0)
            {
                WriteLine(NoWrongAnswers + Point);
                return;
            }
            
            foreach (var answer in result.WrongAnswers)
            {
                Write(answer.Question);
                WriteLine(YourAnswer + Colon + Space + FromList(answer.ChosenAnswers) + Point);
                WriteLine(RightAnswer + Colon + Space + FromList(answer.RightAnswers) + Point + NewLine);
            }
        }
    }

    // utils
    public sealed partial class TestLoop
    {
        private static Func<UserAnswer, string> GetAnswerOutput(bool displayResultInstantly) =>
            displayResultInstantly
                ? answer => GetString(answer) + NewLine + NewLine
                : _ => NewLine.ToString();

        private static string GetString(UserAnswer answer) => answer.IsRight
            ? Correct + Point
            : Incorrect + Point + Space + RightAnswer + Colon + Space + FromList(answer.RightAnswers);
        
        private static string FromList(IImmutableList<int> source) => 
            string.Join(Comma, source);
    }
}