using System.Collections.Immutable;

namespace AVS5.Core
{
    public abstract class BaseQuestion
    {
        /// <summary>
        /// Text of the question.
        /// </summary>
        public string Text { get; init; }
        /// <summary>
        /// Possible answers for the question.
        /// </summary>
        public IImmutableList<string> Answers { get; init; }
        /// <summary>
        /// Right aswers for the question.
        /// </summary>
        public IImmutableList<int> RightAnswers { get; init; }

        protected BaseQuestion() {}
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseQuestion"/> class with question text, possible answers and right answers.
        /// </summary>
        /// <param name="text">The text of the questiton.</param>
        /// <param name="answers"><see cref="IImmutableList{T}"/> collection of possible answers.</param>
        /// <param name="rightAnswers"><see cref="IImmutableList{T}"/> collection of right answers.</param>
        protected BaseQuestion(string text, IImmutableList<string> answers, IImmutableList<int> rightAnswers)
        {
            Text = text;
            Answers = answers;
            RightAnswers = rightAnswers;
        }

        /// <summary>
        /// Returns string representation of <see cref="Answers"/>. Uses <see cref="AVS5.Core.Extensions.From{T}"/> extension method.
        /// </summary>
        /// <returns>Multiline string that represents <see cref="Answers"/>.</returns>
        public string GetAnswers() =>
            Answers.From();

        /// <summary>
        /// Returns string representation of <see cref="RightAnswers"/>. Uses <see cref="AVS5.Core.Extensions.From{T}"/> extension method.
        /// </summary>
        /// <returns>Multiline string that represents <see cref="RightAnswers"/>.</returns>
        public string GetRightAnswers() =>
            RightAnswers.From();
        
        public override string ToString() =>
            $"{Text}\n{GetAnswers()}";

        public virtual string GetFull() =>
            ToString() + $"{string.Join(", ", RightAnswers)}\n";
    }
}