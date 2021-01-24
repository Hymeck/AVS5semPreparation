namespace AVS5.Client.Console
{
    public class ConsoleConfiguration
    {
        /// <summary>
        /// true - результат ответа показывается сразу, после его введения.
        /// false - показывается только итоговый результат в конце теста.
        /// </summary>
        public bool DisplayResultInstantly { get; init; } = true;

        public bool DisplaySettings { get; init; } = true;

        public ConsoleConfiguration(bool displayResultInstantly = true, bool displaySettings = true)
        {
            DisplayResultInstantly = displayResultInstantly;
            DisplaySettings = displaySettings;
        }

        public ConsoleConfiguration()
        {
        }
    }
}