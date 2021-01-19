using System.Collections.Generic;

namespace AVS5.Core
{
    public class Question : BaseQuestion
    {
        public Question(string text, IList<string> answers, IList<int> rightAnswers) : base(text, answers, rightAnswers) { }
    }
}