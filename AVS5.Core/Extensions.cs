using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AVS5.Core
{
    public static class Extensions
    {
        private static readonly Random Random = new ();
        /// <summary>
        /// Shuffles a <see cref="IEnumerable{T}"/> collection.
        /// </summary>
        /// <param name="source">A <see cref="IEnumerable{T}"/> collection to shuffle.</param>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <returns>Shuffled <see cref="IEnumerable{T}"/> collection.</returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) => 
            source.OrderBy(x => Random.Next());

        /// <summary>
        /// Returns multiline string that represents <see cref="IEnumerable{T}"/> collection.
        /// </summary>
        /// <param name="source">A <see cref="IEnumerable{T}"/> collection to represent.</param>
        /// <param name="closingCharAfterNumber">A character that encloses item number.</param>
        /// <param name="s">A <see cref="StarterPoint"/> object that determines starting enumeration number (1 or 0).</param>
        /// <returns>Multiline string. Line contains order number, <see cref="closingCharAfterNumber"/>, space and i'th source element.</returns>
        public static string From<T>(this IEnumerable<T> source, char closingCharAfterNumber = ')', StarterPoint s = StarterPoint.One)
        {
            var sb = new StringBuilder();
            
            var i = (int) s;
            foreach (var item in source) 
                sb.AppendLine($"{i++}{closingCharAfterNumber} {item}");

            return sb.ToString();
        }
        
        /// <summary>
        /// Used in <see cref="Extensions.From{T}"/> method for determing starting enumerating number.
        /// </summary>
        public enum StarterPoint 
        {
            /// <summary>
            /// Use it when you need to start enumerate from zero.
            /// </summary>
            Zero = 0,
            /// <summary>
            /// Use it when you need to start enumerate from one.
            /// </summary>
            One = 1
        }
    }
}