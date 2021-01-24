using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AVS5.Core;

namespace AVS5.Client
{
    public partial class Result
    {
        public static readonly Result Empty = new(
            ImmutableList<UserAnswer>.Empty);

        public readonly IImmutableList<UserAnswer> WrongAnswers;
        public readonly IImmutableList<UserAnswer> AnswerHistory;

        public double Percentage =>
            CalculateRightPercentage(WrongAnswers.Count, AnswerHistory.Count);

        public Result(IImmutableList<UserAnswer> answerHistory)
        {
            AnswerHistory = answerHistory;
            WrongAnswers = GetWrongAnswers(answerHistory);
        }

        public override string ToString() =>
            $"{Percentage}%\n{FromQuestions()}\n";

        private string FromQuestions() => WrongAnswers.Count == 0 ? "-" : WrongAnswers.From();
    }

    public partial class Result
    {
        public static Result FromAnswers(IImmutableList<UserAnswer> answers) =>
            new(answers);

        public static Result FromAnswers(IEnumerable<UserAnswer> answers) =>
            FromAnswers(answers.ToImmutableList());

        private static double CalculateRightPercentage(int wrongCount, int answerCount) =>
            answerCount == 0 ? 0 : (1 - (double) wrongCount / answerCount) * 100;

        private static IImmutableList<UserAnswer> GetWrongAnswers(IImmutableList<UserAnswer> answers) =>
            answers
                .Where(x => !x.IsRight)
                .ToImmutableList();
    }
}