namespace AVS5.Configuration
{
    public class TestConfigurationBuilder
    {
        public static TestConfiguration Default() =>
            new();

        public static TestConfiguration WithShuffle() =>
            new(shuffleThenTake: true);

        public static TestConfiguration Full(bool shuffleThenTake,
            bool showResultInstantly,
            bool isRandomOrder,
            int firstQuestion,
            string location) =>
            new(shuffleThenTake, showResultInstantly, isRandomOrder, firstQuestion, location);

        public static TestConfiguration DefaultLocation(bool shuffleThenTake,
            bool showResultInstantly,
            bool isRandomOrder,
            int firstQuestion) =>
            Full(shuffleThenTake, showResultInstantly, isRandomOrder, firstQuestion, TestConfiguration.DefaultLocation);
    }
}