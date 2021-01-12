﻿using System;
using System.Collections.Generic;

namespace AVS5
{
    public static class ListExtension
    {
        private static readonly Random R = new ();

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            
            while (n > 1)
            {
                n--;
                var k = R.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            
            return list;
        }
    }
}
