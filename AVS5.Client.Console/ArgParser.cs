using System;
using AVS5.Configuration;

namespace AVS5.Client.Console
{
    public class ArgParser
    {
        public static (TestingConfiguration testingConfig, ConsoleConfiguration displayConfig) Parse(string[] args)
        {
            return (new (questionCount: 2, firstQuestion: 2), new ConsoleConfiguration(displayResultInstantly: false));
        }
    }
}