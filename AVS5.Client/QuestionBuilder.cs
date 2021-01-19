using AVS5.Core;
using AVS5.Data.Dto;

namespace AVS5.Client
{
    internal class QuestionBuilder
    {
        public static Question ToQuestion(QuestionDto questionDto) => 
            new (questionDto.Text, questionDto.Answers, questionDto.RightAnswers);

        public static ShuffledQuestion ToShuffledQuestion(Question question) =>
            new(question);

        public static ShuffledQuestion ToShuffledQuestion(QuestionDto questionDto) =>
            ToShuffledQuestion(ToQuestion(questionDto));
    }
}