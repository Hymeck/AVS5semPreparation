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
        /// Note that <see cref="T"/> should be numeric.
        /// </summary>
        /// <param name="source">A collection to shuffle.</param>
        /// <typeparam name="T">Numeric.</typeparam>
        /// <returns>Shuffled <see cref="IEnumerable{T}"/> collection.</returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) => 
            source.OrderBy(x => Random.Next());

        /// <summary>
        /// Returns multiline string that represents <see cref="source"/> collection.
        /// </summary>
        /// <param name="source">A collection of <see cref="string"/> items.</param>
        /// <param name="closingCharAfterNumber">Encloses number.</param>
        /// <param name="s">Determines starting enumeration number (1 or 0).</param>
        /// <returns>Multiline string. Line contains order number, <see cref="closingCharAfterNumber"/>, space and i'th source element.</returns>
        public static string FromIList<T>(this IList<T> source, char closingCharAfterNumber = ')', StarterPoint s = StarterPoint.One)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < source.Count; i++)
                sb.AppendLine($"{(int)(i + s)}{closingCharAfterNumber} {source[i]}");
            return sb.ToString();
        }
        
        /// <summary>
        /// Used in <see cref="Extensions.FromIList{T}"/> method for determing
        /// starting numerating number.
        /// </summary>
        public enum StarterPoint 
        {
            /// <summary>
            /// Use it when you need to start numerate from zero.
            /// </summary>
            Zero = 0,
            /// <summary>
            /// Use it when you need to start numerate from one.
            /// </summary>
            One = 1
        }
    }
}