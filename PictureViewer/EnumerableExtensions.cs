using System;
using System.Collections.Generic;
using System.Linq;

namespace PictureViewer
{
    public static class EnumerableExtensions
    {
        private readonly static Random mRandom = new Random();

        public static IOrderedEnumerable<T> RandomOrder<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => mRandom.Next());
        }
    }
}
