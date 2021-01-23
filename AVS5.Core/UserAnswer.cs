using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AVS5.Core
{
    /// <summary>
    /// Represents user answer. Contains question and a collection of chosen answers.
    /// </summary>
    public record UserAnswer(BaseQuestion Question, IImmutableList<int> ChosenAnswers)
    {
        public UserAnswer(BaseQuestion question, IEnumerable<int> chosenAnswers) : 
            this(question, chosenAnswers.ToImmutableList()) {}
        public bool IsRight => 
            CompareAnswers();

        public IImmutableList<int> RightAnswers =>
            Question.RightAnswers;
        
        private bool CompareAnswers() =>
            Question.RightAnswers.Count == ChosenAnswers.Count &&
            Question.RightAnswers.All(ChosenAnswers.Contains);
    }
}