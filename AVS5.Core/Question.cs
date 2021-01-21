using System.Collections.Immutable;

namespace AVS5.Core
{
    public sealed class Question : BaseQuestion
    {
        public Question(string text, IImmutableList<string> answers, IImmutableList<int> rightAnswers) : 
            base(text, answers, rightAnswers) { }
    }
}