using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AVS5.Core;
using AVS5.Data.Dto;

namespace AVS5.Client
{
    internal class QuestionBuilder
    {
        public static BaseQuestion ToQuestion(QuestionDto questionDto) => 
            new Question(
                questionDto.Text, 
                questionDto.Answers.AsEnumerable().ToImmutableList(), 
                questionDto.RightAnswers.AsEnumerable().ToImmutableList());

        public static IEnumerable<BaseQuestion> ToQuestion(IEnumerable<QuestionDto> source) =>
            source.Select(ToQuestion);

        public static BaseQuestion ToShuffledQuestion(QuestionDto questionDto) =>
            ToShuffledQuestion(ToQuestion(questionDto));

        public static BaseQuestion ToShuffledQuestion(BaseQuestion question) =>
            new ShuffledQuestion(question);

        public static IEnumerable<BaseQuestion> ToShuffledQuestion(IEnumerable<QuestionDto> source) =>
            source.Select(ToShuffledQuestion);
        
        public static IEnumerable<BaseQuestion> ToShuffledQuestion(IEnumerable<BaseQuestion> source) =>
            source.Select(ToShuffledQuestion);
        
        
    }
}