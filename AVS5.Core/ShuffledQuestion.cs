using System.Collections.Generic;
using System.Linq;

namespace AVS5.Core
{
    // todo: refactor copy-pasted code
    public class ShuffledQuestion : BaseQuestion
    {
        public ShuffledQuestion(string text, IList<string> answers, IList<int> rightAnswers)
        {
            Text = text;
            
            Answers = answers.Shuffle().ToArray();

            var rightAnswerTexts =
                GetRightAnswerTexts(answers, rightAnswers);
            RightAnswers = GetRightAnswers(Answers, rightAnswerTexts);
        }

        public ShuffledQuestion(BaseQuestion question)
        {
            Text = question.Text;
            
            Answers = question.Answers.Shuffle().ToArray();

            var rightAnswerTexts = GetRightAnswerTexts(question.Answers, question.RightAnswers);
            RightAnswers = GetRightAnswers(Answers, rightAnswerTexts);
        }
        
        private static IList<string> GetRightAnswerTexts(IList<string> answers, IList<int> rightAnswers)
        {
            var rightAnswerTexts = new List<string>(rightAnswers.Count);

            rightAnswerTexts
                .AddRange(rightAnswers
                    .Select(answer => answers[answer - 1]));

            return rightAnswerTexts;
        }

        private static IList<int> GetRightAnswers(IList<string> answers, IList<string> rightAnswerTexts)
        {
            // todo: make more efficient. O(m * n) -> O(m), where m - length of answers, n - length of right answers
            var rightAnswers = new List<int>(rightAnswerTexts.Count);

            for (var i = 0; i < answers.Count; i++)
                rightAnswers
                    .AddRange(rightAnswerTexts
                        .Where(t => t.Equals(answers[i]))
                        .Select(_ => i + 1));

            return rightAnswers;
        }
    }
}