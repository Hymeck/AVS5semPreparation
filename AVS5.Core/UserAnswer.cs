using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AVS5.Core
{
    public record UserAnswer(BaseQuestion Question, IImmutableList<int> ChosenAnswers)
    {
        public UserAnswer(BaseQuestion question, IEnumerable<int> chosenAnswers) : 
            this(question, chosenAnswers.ToImmutableList()) {}
        public bool IsRight => 
            CompareAnswers();

        private bool CompareAnswers() =>
            Question.RightAnswers.Count == ChosenAnswers.Count &&
            Question.RightAnswers.All(ChosenAnswers.Contains);
    }
}