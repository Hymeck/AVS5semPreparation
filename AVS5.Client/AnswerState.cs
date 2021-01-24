using System.Collections.Generic;
using System.Collections.Immutable;
using AVS5.Core;

namespace AVS5.Client
{
    internal partial class AnswerState
    {
        public readonly Result Result;

        public static readonly AnswerState Empty = new(Result.Empty);

        private AnswerState(Result result) => 
            Result = result;

        public AnswerState(UserAnswer answer) : 
            this(Result.FromAnswers(ImmutableList<UserAnswer>.Empty.Add(answer)))
        {
        }

        public AnswerState(IImmutableList<UserAnswer> answerHistory) :
            this(Result.FromAnswers(answerHistory))
        {
        }

        public AnswerState(IEnumerable<UserAnswer> answerHistory) : 
            this(answerHistory.ToImmutableList())
        {
        }

        public AnswerState AddAnswer(UserAnswer answer) =>
            new(Result.AnswerHistory.Add(answer));
    }

    internal partial class AnswerState
    {
        public static AnswerState AddAnswer(AnswerState state, UserAnswer answer) =>
            state.AddAnswer(answer);

        public static AnswerState operator +(AnswerState state, UserAnswer answer) => AddAnswer(state, answer);
    }
}