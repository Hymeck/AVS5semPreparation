using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AVS5.Core
{
    // todo: refactor copy-pasted code
    public sealed class ShuffledQuestion : BaseQuestion
    {
        public ShuffledQuestion(string text, IImmutableList<string> answers, IImmutableList<int> rightAnswers)
        {
            Text = text;
            
            Answers = Shuffle(answers);

            var rightAnswerTexts =
                GetRightAnswerTexts(answers, rightAnswers);
            RightAnswers = GetRightAnswers(Answers, rightAnswerTexts);
        }

        public ShuffledQuestion(BaseQuestion question)
        {
            Text = question.Text;

            Answers = Shuffle(question.Answers);

            var rightAnswerTexts = GetRightAnswerTexts(question.Answers, question.RightAnswers);
            RightAnswers = GetRightAnswers(Answers, rightAnswerTexts);
        }

        private static IImmutableList<T> Shuffle<T>(IEnumerable<T> source) =>
            source.Shuffle().ToImmutableList();        
        
        private static IImmutableList<string> GetRightAnswerTexts(IImmutableList<string> answers, IImmutableList<int> rightAnswers)
        {
            var rightAnswerTexts = new List<string>(rightAnswers.Count);

            rightAnswerTexts
                .AddRange(rightAnswers
                    .Select(answer => answers[answer - 1]));

            return rightAnswerTexts.AsEnumerable().ToImmutableList();
        }

        private static IImmutableList<int> GetRightAnswers(IImmutableList<string> answers, IImmutableList<string> rightAnswerTexts)
        {
            // todo: make more efficient?
            var rightAnswers = new List<int>(rightAnswerTexts.Count);

            for (var i = 0; i < answers.Count; i++)
                rightAnswers
                    .AddRange(rightAnswerTexts
                        .Where(t => t.Equals(answers[i]))
                        .Select(_ => i + 1));

            return rightAnswers.AsEnumerable().ToImmutableList();
        }
    }
}