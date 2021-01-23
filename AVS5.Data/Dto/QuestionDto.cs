using System.Collections.Generic;

namespace AVS5.Data.Dto
{
    /// <summary>
    /// Question Data Transfer Object. This object is a intermediate layer between data source and core entity.
    /// </summary>
    public class QuestionDto
    {
        /// <summary>
        /// Holds the text of the question.
        /// </summary>
        public readonly string Text;
        /// <summary>
        /// Holds possible answer variants.
        /// </summary>
        public readonly IEnumerable<string> Answers;
        /// <summary>
        /// Holds right answers.
        /// </summary>
        public readonly IEnumerable<int> RightAnswers;

        /// <summary>
        /// Default constructor for <see cref="QuestionDto"/> object.
        /// </summary>
        /// <param name="text">The text of the question.</param>
        /// <param name="answers">A collection of possible answer variants.</param>
        /// <param name="rightAnswers">A collection of right answers.</param>
        public QuestionDto(string text, IEnumerable<string> answers, IEnumerable<int> rightAnswers) =>
            (Text, Answers, RightAnswers) = (text, answers, rightAnswers);
    }
}