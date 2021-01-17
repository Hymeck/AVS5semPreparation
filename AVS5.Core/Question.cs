using System.Collections.Generic;

namespace AVS5.Core
{
    public record Question(string Text, IList<string> Answers, IList<int> RightAnswers)
    {
        public override string ToString() => 
            Answers.FromIList();
    }
}