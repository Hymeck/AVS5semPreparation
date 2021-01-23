namespace AVS5.Configuration
{
    /// <summary>
    /// Содержит конфигурационные параметры.
    /// </summary>
    public class TestConfiguration
    {
        /// <summary>
        /// Количество вопросов для тренировки.
        /// </summary>
        public readonly int QuestionCount;

        /// <summary>
        /// true - сначала все вопросы перемешиваются, потом из них берутся первые n штук.
        /// false - сначала из исходного упорядоченного списка берётся n тестов, потом они перемешиваются.
        /// Как работает <see cref="ShuffleBeforeTaking"/>?
        /// Пусть у нас есть последовательность чисел (номера вопросов) 123456789.
        /// Допустим, необходимо потренеровать 5 вопросов.
        /// Если <see cref="ShuffleBeforeTaking"/> = true, то произойдет следующее:
        /// 123456789 ---(вопросы перемешиваются)---> 476592138 ---(берется 5 штук)---> 47659 - пул вопросов.
        /// Если <see cref="ShuffleBeforeTaking"/> = false, то произойдет следующее:
        /// 123456789 ---(берём 5 штук)---> 12345 ---(вопросы перемешиваются)---> 43125 - пул вопросов.
        /// Если необходимо потренеровать, например, с 20 по 80 вопрос, то делаем следующее:
        /// Устанавливаем <see cref="ShuffleBeforeTaking"/> в false,
        /// <see cref="FirstQuestion"/> приравниваем числу 20.
        /// Затем в программе указываем, что нужно 60 вопросов (80-20).
        /// </summary>
        public bool ShuffleBeforeTaking;

        /// <summary>
        /// true - результат ответа показывается сразу, после его введения.
        /// false - показывается только итоговый результат в конце теста.
        /// </summary>
        public readonly bool ShowResultInstantly;

        /// <summary>
        /// true - варианты ответов распологаются в случайном порядке.
        /// false - варианты ответов стоят на одном месте.
        /// </summary>
        public readonly bool IsRandomOrder;

        /// <summary>
        /// Номер вопроса, с которого будет начинаться отбор тестов.
        /// Следует использовать, если хотите прорешать определённый вариант.
        /// Работает, если <see cref="ShuffleBeforeTaking"/> сброшен.
        /// </summary>
        public readonly int FirstQuestion;

        public TestConfiguration(
            int questionCount,
            bool shuffleBeforeTaking = false,
            bool showResultInstantly = true,
            bool isRandomOrder = true,
            int firstQuestion = 1)
        {
            QuestionCount = questionCount;
            ShuffleBeforeTaking = shuffleBeforeTaking;
            ShowResultInstantly = showResultInstantly;
            IsRandomOrder = isRandomOrder;
            FirstQuestion = firstQuestion;
        }
    }
}