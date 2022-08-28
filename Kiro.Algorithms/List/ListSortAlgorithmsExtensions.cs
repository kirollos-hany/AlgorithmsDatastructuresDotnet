using System;
using System.Collections.Generic;
using System.Linq;

namespace Kiro.Algorithms.List
{
    public static class ListSortAlgorithmsExtensions
    {
        public static List<T> QSortAlgorithm<T>(this IList<T> list, Func<T, T, int> compare)
        {
            var sortedList = list.Select(x => x).ToList();
            if (sortedList.Count < 2)
            {
                return sortedList;
            }

            var pivotIndex = sortedList.Count / 2;
            var pivot = sortedList[pivotIndex];
            var leftPartition = sortedList.Where(x =>
            {
                var compareResult = compare(x, pivot);
                return compareResult < 0;
            }).ToList();
            var rightPartition = sortedList.Where(x =>
            {
                var compareResult = compare(x, pivot);
                return compareResult > 0;
            }).ToList();

            return leftPartition.QSortAlgorithm(compare).Concat(new List<T> { pivot })
                .Concat(rightPartition.QSortAlgorithm(compare)).ToList();
        }
        
        public static List<T> QSortDescendingAlgorithm<T>(this IList<T> list, Func<T, T, int> compare)
        {
            var sortedList = list.Select(x => x).ToList();
            if (sortedList.Count < 2)
            {
                return sortedList;
            }

            var pivotIndex = sortedList.Count / 2;
            var pivot = sortedList[pivotIndex];
            var leftPartition = sortedList.Where(x =>
            {
                var compareResult = compare(x, pivot);
                return compareResult < 0;
            }).ToList();
            var rightPartition = sortedList.Where(x =>
            {
                var compareResult = compare(x, pivot);
                return compareResult > 0;
            }).ToList();

            return rightPartition.QSortDescendingAlgorithm(compare).Concat(new List<T> { pivot })
                .Concat(leftPartition.QSortDescendingAlgorithm(compare)).ToList();
        }

        public static List<T> SelectionSortAlgorithm<T>(this IList<T> list, Func<T, T, int> compare)
        {
            var newList = list.Select(item => item).ToList();
            for (var i = 0; i < newList.Count; i++)
            {
                var smallestIndex = GetSmallest(newList, compare, i);
                var min = newList[smallestIndex];
                var temp = newList[i];
                newList[i] = min;
                newList[smallestIndex] = temp;
            }

            return newList;
        }

        public static List<T> SelectionSortDescendingAlgorithm<T>(this IList<T> list, Func<T, T, int> compare)
        {
            var newList = list.Select(item => item).ToList();
            for (var i = 0; i < newList.Count; i++)
            {
                var maxIndex = GetMax(newList, compare, i);
                var max = newList[maxIndex];
                var temp = newList[i];
                newList[i] = max;
                newList[maxIndex] = temp;
            }

            return newList;
        }

        private static int GetMax<T>(IList<T> list, Func<T, T, int> compare, int index)
        {
            var maxIndex = index;
            var max = list[maxIndex];
            for (var i = index + 1; i < list.Count; i++)
            {
                var compareResult = compare(list[i], max);
                if (compareResult <= 0) continue;
                max = list[i];
                maxIndex = i;
            }

            return maxIndex;
        }

        private static int GetSmallest<T>(IList<T> list, Func<T, T, int> compare, int index)
        {
            var smallestIndex = index;
            var smallest = list[smallestIndex];
            for (var i = index + 1; i < list.Count; i++)
            {
                var compareResult = compare(list[i], smallest);
                if (compareResult >= 0) continue;
                smallest = list[i];
                smallestIndex = i;
            }

            return smallestIndex;
        }
    }
}