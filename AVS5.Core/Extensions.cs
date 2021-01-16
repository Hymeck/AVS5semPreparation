using System;
using System.Collections.Generic;
using System.Linq;

namespace AVS5.Core
{
    public static class Extensions
    {
        private static Random _random = new Random();
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) => 
            source.OrderBy(x => _random.Next());
    }
}