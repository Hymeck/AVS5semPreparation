using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AVS5.Configuration;
using AVS5.Core;

namespace AVS5.Client
{
    internal sealed partial class QuestionPull
    {
        public readonly IImmutableList<BaseQuestion> Questions;
        public readonly TestingConfiguration Configuration;

        private QuestionPull(IImmutableList<BaseQuestion> questions, TestingConfiguration configuration)
        {
            Questions = questions;
            Configuration = configuration;
        }

        private QuestionPull(TestingConfiguration configuration) : this(ImmutableList<BaseQuestion>.Empty, configuration)
        {
        }
    }

    internal sealed partial class QuestionPull
    {
        public static QuestionPull GetDefaultInstance(TestingConfiguration configuration) => new(configuration);

        public static QuestionPull GetConfiguredInstance(IEnumerable<BaseQuestion> questions,
            TestingConfiguration configuration) =>
            GetConfiguredInstance(questions.ToImmutableList(), configuration);

        public static QuestionPull GetConfiguredInstance(IImmutableList<BaseQuestion> questions,
            TestingConfiguration configuration)
        {
            var configuredQuestions = Configure(questions, configuration);
            return new QuestionPull(configuredQuestions, configuration);
        }

        private static IImmutableList<BaseQuestion> Configure(IEnumerable<BaseQuestion> questions,
            TestingConfiguration configuration) =>
            questions
                .SkipOrShuffle(configuration.ShuffleBeforeTaking, configuration.FirstQuestion)
                .Take(configuration.QuestionCount)
                .ToShuffledQuestion(configuration.IsRandomOrder)
                .Shuffle()
                .ToImmutableList();
    }

    internal static class Extensions
    {
        public static IEnumerable<BaseQuestion> SkipOrShuffle(
            this IEnumerable<BaseQuestion> source,
            bool shuffleBeforeTaking,
            int startFrom) =>
            !shuffleBeforeTaking && startFrom > 1
                ? source
                    .Skip(startFrom - 1)
                : source.Shuffle();

        public static IEnumerable<BaseQuestion> ToShuffledQuestion(
            this IEnumerable<BaseQuestion> source,
            bool isRandomOrder) =>
            isRandomOrder
                ? QuestionBuilder.ToShuffledQuestion(source)
                : source;

        public static string From(this IEnumerable<BaseQuestion> source) => 
            string.Join('\n', source.Select(x => x.Text));
    }
}