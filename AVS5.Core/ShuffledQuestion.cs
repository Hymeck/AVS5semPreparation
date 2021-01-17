using System.Collections.Generic;
using System.Linq;

namespace AVS5.Core
{
    public class ShuffledQuestion
    {
        public readonly IList<string> Answers;
        public readonly IList<int> RightAnswers;

        public ShuffledQuestion(Question question)
        {
            var (_, answers, rightAnswers) = question;
            var rightAnswerTexts = new List<string>(rightAnswers.Count);

            rightAnswerTexts
                .AddRange(rightAnswers
                    .Select(answer => answers[answer - 1]));

            Answers = answers.Shuffle().ToArray();

            // todo: make more efficient. O(m * n) -> O(m), where m - length of answers, n - length of right answers
            RightAnswers = new List<int>(rightAnswers.Count);
            for (var i = 0; i < Answers.Count; i++)
                foreach (var _ in rightAnswerTexts.Where(t => t.Equals(Answers[i])))
                    RightAnswers.Add(i + 1);
        }

        public override string ToString() => 
            Answers.FromIList();
    }
}