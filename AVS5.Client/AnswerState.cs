using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AVS5.Core;

namespace AVS5.Client
{
    internal partial class AnswerState
    {
        public readonly IImmutableList<UserAnswer> Answers;
        public readonly Result Result;

        public static readonly AnswerState Empty = new(ImmutableList<UserAnswer>.Empty, Result.Empty);

        private AnswerState(IImmutableList<UserAnswer> answers, Result result)
        {
            Answers = answers;
            Result = result;
        }

        public AnswerState(UserAnswer answer)
        {
            Answers = ImmutableList<UserAnswer>.Empty.Add(answer);
            Result = FromAnswers(Answers);
        }

        public AnswerState(IImmutableList<UserAnswer> answers) :
            this(answers, FromAnswers(answers))
        {
        }

        public AnswerState AddAnswer(UserAnswer answer) =>
            new(Answers.Add(answer));
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

            // todo: correct this place
            var percentage = (double)(1 - wrongAnswers.Count / listedAnswers.Count) * 100;
            return new Result(percentage, wrongAnswers);
        }

        public static AnswerState AddAnswer(AnswerState state, UserAnswer answer) =>
            state.AddAnswer(answer);

        public static AnswerState operator +(AnswerState state, UserAnswer answer) => AddAnswer(state, answer);
    }
}