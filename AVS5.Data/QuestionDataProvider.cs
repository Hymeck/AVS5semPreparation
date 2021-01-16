using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using AVS5.Data.Dto;

namespace AVS5.Data
{
    public class QuestionDataProvider : IDataProvider<QuestionDto>
    {
        private readonly string _fileLocation;
        private const char AnswerSeparator = ';';
        private const char RightAnswerSeparator = ' ';

        public QuestionDataProvider(string fileLocation) =>
            _fileLocation = fileLocation;

        public IEnumerable<QuestionDto> GetData()
        {
            var lines = File.ReadAllLines(_fileLocation);
            IList<QuestionDto> questions = new List<QuestionDto>();
            for (var i = 0; i < lines.Length; i += 4)
            {
                var (t, v, r) = GetCoreInfo(lines, i);
                var q = new QuestionDto(t, ParseVariants(v), ParseRightAnswers(r));
                questions.Add(q);
            }

            return questions;
        }
        
        private (string text, string variants, string rightAnswers) GetCoreInfo(string[] lines, int index) =>
            (lines[index], lines[index + 1], lines[index + 2]);
        
        private IList<string> ParseVariants(string variants) => 
            variants
                .Split(AnswerSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Split(')')[1]).ToArray();
        
        private IList<int> ParseRightAnswers(string rightAnswers) =>
            rightAnswers.Split(RightAnswerSeparator, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
    }
}