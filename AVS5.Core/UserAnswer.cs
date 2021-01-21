using System.Collections.Immutable;
using System.Linq;

namespace AVS5.Core
{
    public record UserAnswer(BaseQuestion Question, IImmutableList<int> ChosenAnswers)
    {
        public bool IsRight => 
            CompareAnswers();

        private bool CompareAnswers() =>
            Question.RightAnswers.Count == ChosenAnswers.Count &&
            Question.RightAnswers.All(ChosenAnswers.Contains);
    }
}