using System.Collections.Immutable;
using AVS5.Core;

namespace AVS5.Client
{
    public class Result
    {
        public static readonly Result Empty = new(0, ImmutableList<BaseQuestion>.Empty);
        public readonly double Percentage;
        public readonly IImmutableList<BaseQuestion> WrongQuestions;

        public Result(double percentage, IImmutableList<BaseQuestion> wrongQuestions)
        {
            Percentage = percentage;
            WrongQuestions = wrongQuestions;
        }

        public override string ToString() =>
            $"{Percentage}%\n{FromQuestions()}\n";

        private string FromQuestions() => WrongQuestions.Count == 0 ? "-" : WrongQuestions.From();
    }
}