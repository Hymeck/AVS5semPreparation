using System;

namespace AVS5.Client.Console
{
    /// <summary>
    /// Helps to handler input.
    /// </summary>
    public class InputHandler
    {
        /// <summary>
        /// Tries to parse input string value to a number in cycle.
        /// </summary>
        /// <param name="min">Left bound of input.</param>
        /// <param name="max">Right bound of input.</param>
        /// <param name="input">Lambda that returns string.</param>
        /// <param name="output">Lambda for outputing <see cref="errorMessage"/>.</param>
        /// <param name="errorMessage">Message that will be sent in <see cref="output"/>.</param>
        /// <returns>Parsed number that lies from <see cref="min"/> to <see cref="max"/>.</returns>
        /// <exception cref="ArgumentException">Incorrect bound values.</exception>
        public static int InputNumber(int min, int max, Func<string> input, Action<string> output, string errorMessage)
        {
            if (min >= max)
                throw new ArgumentException($"{nameof(min)} >= {nameof(max)}.");
            
            int result;
            while (!int.TryParse(input(), out result) || !(result >= min && result <= max))
                output(errorMessage);
            
            return result;
        }
    }
}