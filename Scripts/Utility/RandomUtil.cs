#nullable enable
using System;
using System.Collections.Generic;

namespace ZnZUtil
{
    public static class RandomUtil
    {
        private static readonly Random random = new Random();

        public static int GetRandomInt(int startInkl, int endInkl) => random.Next(startInkl, endInkl + 1);

        public static T? GetRandomItem<T>(this List<T> list) where T : class =>
            list.Count == 0 ? default : list[GetRandomInt(0, list.Count - 1)];

        public static T GetRandomSItem<T>(this List<T> list) where T : struct =>
            list.Count == 0 ? default : list[GetRandomInt(0, list.Count - 1)];

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}