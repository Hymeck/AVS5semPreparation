using System.Collections.Immutable;
using AVS5.Core;

namespace AVS5.Client
{
    public sealed class Result
    {
        public readonly double Percentage;
        public readonly IImmutableQueue<BaseQuestion> WrongQuestions;

        public Result(double percentage, IImmutableQueue<BaseQuestion> wrongQuestions)
        {
            Percentage = percentage;
            WrongQuestions = wrongQuestions;
        }

        public static Result Empty { get; } = new (0, ImmutableQueue<BaseQuestion>.Empty);
    }
}