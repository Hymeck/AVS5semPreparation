using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AVS5.Core
{
    public abstract class BaseQuestion
    {
        public string Text { get; init; }
        public IImmutableList<string> Answers { get; init; }
        public IImmutableList<int> RightAnswers { get; init; }

        protected BaseQuestion() {}
        
        protected BaseQuestion(string text, IImmutableList<string> answers, IImmutableList<int> rightAnswers)
        {
            Text = text;
            Answers = answers;
            RightAnswers = rightAnswers;
        }

        public string GetAnswers() =>
            Answers.AsEnumerable().ToList().FromIList();

        public string GetRightAnswers() =>
            RightAnswers.AsEnumerable().ToList().FromIList();
        
        public override string ToString() =>
            $"{Text}\n{GetAnswers()}";

        public virtual string GetFull() =>
            ToString() + $"{string.Join(", ", RightAnswers)}\n";
    }
}