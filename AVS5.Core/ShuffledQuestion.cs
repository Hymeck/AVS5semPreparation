using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AVS5.Core
{
    /// <summary>
    /// Represents <see cref="BaseQuestion"/> question that get shuffled on initializing.
    /// </summary>
    public partial class ShuffledQuestion : BaseQuestion
    {
        public ShuffledQuestion(string text, IImmutableList<string> answers, IImmutableList<int> rightAnswers)
        {
            Text = text;

            Answers = answers.Shuffle().ToImmutableList();

            var rightAnswerTexts =
                GetRightAnswerTexts(answers, rightAnswers);
            RightAnswers = GetRightAnswers(Answers, rightAnswerTexts);
        }

        public ShuffledQuestion(BaseQuestion question) : 
            this(question.Text, question.Answers, question.RightAnswers)
        {
        }
    }

    public partial class ShuffledQuestion
    {
        private static IImmutableList<string> GetRightAnswerTexts(IImmutableList<string> answers, IImmutableList<int> rightAnswers)
        {
            var rightAnswerTexts = new List<string>(rightAnswers.Count);

            rightAnswerTexts
                .AddRange(rightAnswers
                    .Select(answer => answers[answer - 1]));

            return rightAnswerTexts.ToImmutableList();
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

            return rightAnswers.ToImmutableList();
        }
    }
}