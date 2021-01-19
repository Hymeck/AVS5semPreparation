using System.Collections.Generic;

namespace AVS5.Core
{
    public abstract class BaseQuestion
    {
        public string Text { get; init; }
        public IList<string> Answers { get; init; }
        public IList<int> RightAnswers { get; init; }

        protected BaseQuestion() {}
        
        protected BaseQuestion(string text, IList<string> answers, IList<int> rightAnswers)
        {
            Text = text;
            Answers = answers;
            RightAnswers = rightAnswers;
        }

        public string GetAnswers() =>
            Answers.FromIList();

        public string GetRightAnswers() =>
            RightAnswers.FromIList();
        
        public override string ToString() =>
            $"{Text}\n{Answers.FromIList()}{string.Join(", ", RightAnswers)}";
    }
}