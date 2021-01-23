using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AVS5.Core;

namespace AVS5.Client
{
    internal partial class AnswerState
    {
        public readonly IImmutableList<UserAnswer> CurrentAnswers;
        public readonly Result Result;

        public static readonly AnswerState Empty = new(ImmutableList<UserAnswer>.Empty, Result.Empty);

        private AnswerState(IImmutableList<UserAnswer> currentAnswers, Result result)
        {
            CurrentAnswers = currentAnswers;
            Result = result;
        }

        public AnswerState(UserAnswer answer)
        {
            CurrentAnswers = ImmutableList<UserAnswer>.Empty.Add(answer);
            Result = FromAnswers(CurrentAnswers);
        }

        public AnswerState(IImmutableList<UserAnswer> currentAnswers) :
            this(currentAnswers, FromAnswers(currentAnswers))
        {
        }

        public AnswerState AddAnswer(UserAnswer answer) =>
            new(CurrentAnswers.Add(answer));
    }

    internal partial class AnswerState
    {
        private static Result FromAnswers(IEnumerable<UserAnswer> answers)
        {
            var listedAnswers = answers.ToList();
            var wrongAnswers = listedAnswers
                .Where(x => !x.IsRight)
                .Select(x => x.Question)
                .ToImmutableList();

            var percentage = CalculatePercentage(wrongAnswers.Count, listedAnswers.Count);
            return new Result(percentage, wrongAnswers);
        }

        private static double CalculatePercentage(int wrongCount, int answerCount) => 
            (1 - (double)wrongCount / answerCount) * 100;

        public static AnswerState AddAnswer(AnswerState state, UserAnswer answer) =>
            state.AddAnswer(answer);

        public static AnswerState operator +(AnswerState state, UserAnswer answer) => AddAnswer(state, answer);
    }
}