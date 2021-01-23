using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AVS5.Data.Dto;

namespace AVS5.Data
{
    /// <summary>
    /// Default implementation of <see cref="IDataProvider{T}"/>. Takes data from file.
    /// </summary>
    public sealed class QuestionDtoProvider : IDataProvider<QuestionDto>
    {
        private readonly string _fileLocation;
        private const char AnswerSeparator = ';';
        private const char RightAnswerSeparator = ' ';

        public QuestionDtoProvider(string fileLocation) =>
            _fileLocation = fileLocation;

        public IEnumerable<QuestionDto> GetData()
        {
            var lines = ReadAllFromFile();
            return FromLines(lines);
        }

        private static IEnumerable<QuestionDto> FromLines(string[] lines)
        {
            // IList<QuestionDto> questions = new List<QuestionDto>(lines.Length / 4 + 1);
            
            for (var i = 0; i < lines.Length; i += 4)
            {
                var (t, v, r) = GetCoreInfo(lines, i);
                var q = new QuestionDto(t, ParseAnswers(v), ParseRightAnswers(r));
                // questions.Add(q);
                yield return q;
            }

            // return questions;
        }

        private string[] ReadAllFromFile() => 
            File.ReadAllLines(_fileLocation);

        private static (string text, string variants, string rightAnswers) GetCoreInfo(string[] lines, int index) =>
            (lines[index], lines[index + 1], lines[index + 2]);
        
        private static IEnumerable<string> ParseAnswers(string variants) => 
            variants
                .Split(AnswerSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Split(')')[1]);
        
        private static IEnumerable<int> ParseRightAnswers(string rightAnswers) =>
            rightAnswers
                .Split(RightAnswerSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse);
    }
}