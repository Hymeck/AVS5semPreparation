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

        public string File { get; init; } = "avs_demo.txt";

        public ConsoleConfiguration(string file, bool displayResultInstantly = true, bool displaySettings = true)
        {
            File = file;
            DisplayResultInstantly = displayResultInstantly;
            DisplaySettings = displaySettings;
        }

        public ConsoleConfiguration()
        {
        }
    }
}