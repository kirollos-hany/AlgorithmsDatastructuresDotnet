using System;
using System.Collections.Generic;

namespace Kiro.Algorithms.List
{
    public static class ListSearchAlgorithmsExtensions
    {
        public static int BinarySearchAlgorithm<T>(this IList<T> list, T key, Func<T, T, int> compare)
        {
            var low = 0;
            var high = list.Count - 1;
            while (low <= high)
            {
                var mid = (high + low) / 2;
                var guess = list[mid];
                var compareResult = compare(guess, key);
                if (compareResult == 0)
                {
                    return mid;
                }

                if (compareResult < 0)
                {
                    low = mid + 1;
                    continue;
                }

                high = mid - 1;
            }

            return -1;
        }

        public static int BinarySearchAlgorithm<T>(this T[] arr, T key, Func<T, T, int> compare)
        {
            var low = 0;
            var high = arr.Length - 1;
            while (low <= high)
            {
                var mid = (high + low) / 2;
                var guess = arr[mid];
                var compareResult = compare(guess, key);
                if (compareResult == 0)
                {
                    return mid;
                }

                if (compareResult < 0)
                {
                    low = mid + 1;
                    continue;
                }

                high = mid - 1;
            }

            return -1;
        }

       
    }
}