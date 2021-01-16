using System.Collections.Generic;
using System.Text;

namespace AVS5.Core
{
    public record Question(string Text, IList<string> Answers, IList<int> RightAnswers)
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < Answers.Count; i++)
                sb.AppendLine($"{i + 1}) {Answers[i]}");
            return sb.ToString();
        }
    }
}