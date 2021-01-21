using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AVS5.Configuration;
using AVS5.Core;

namespace AVS5.Client
{
    internal sealed partial class QuestionPull
    {
        public IImmutableList<BaseQuestion> Questions { get; init; }
        public TestConfiguration Configuration { get; init; }

        private QuestionPull(IImmutableList<BaseQuestion> questions, TestConfiguration configuration)
        {
            Questions = questions;
            Configuration = configuration;
        }

        private QuestionPull(TestConfiguration configuration) : this(ImmutableList<BaseQuestion>.Empty, configuration)
        {
        }
    }

    internal sealed partial class QuestionPull
    {
        public static QuestionPull GetDefaultInstance(TestConfiguration configuration) => new(configuration);

        public static QuestionPull GetConfiguredInstance(IEnumerable<BaseQuestion> questions,
            TestConfiguration configuration) =>
            GetConfiguredInstance(questions.ToImmutableList(), configuration);

        public static QuestionPull GetConfiguredInstance(IImmutableList<BaseQuestion> questions,
            TestConfiguration configuration)
        {
            var configuredQuestions = Configure(questions, configuration);
            return new QuestionPull(configuredQuestions, configuration);
        }

        private static IImmutableList<BaseQuestion> Configure(IEnumerable<BaseQuestion> questions,
            TestConfiguration configuration) =>
            questions
                .Skip(configuration.ShuffleThenTake, configuration.FirstQuestion)
                .Take(configuration.QuestionCount)
                .ToShuffledQuestion(configuration.IsRandomOrder)
                .Shuffle()
                .ToImmutableList();
    }

    internal static class Extensions
    {
        public static IEnumerable<BaseQuestion> Skip(
            this IEnumerable<BaseQuestion> source,
            bool shuffleThenTake,
            int startFrom) =>
            !shuffleThenTake && startFrom > 1
                ? source
                    .Skip(startFrom - 1)
                : source;

        public static IEnumerable<BaseQuestion> ToShuffledQuestion(
            this IEnumerable<BaseQuestion> source,
            bool isRandomOrder) =>
            isRandomOrder
                ? source.Select(QuestionBuilder.ToShuffledQuestion)
                : source;
    }
}