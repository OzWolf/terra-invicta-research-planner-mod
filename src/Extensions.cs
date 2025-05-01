using System.Collections.Generic;
using UnityModManagerNet;

namespace ResearchPlanner
{
    // ReSharper disable InconsistentNaming
    public static class Extensions
    {
        public static int point(this int x) => UnityModManager.UI.Scale(x);
        public static List<List<T>> Chunk<T>(this List<T> list, int chunkSize)
        {
            var result = new List<List<T>>();

            var current = new List<T>();
            foreach (var t in list)
            {
                current.Add(t);
                if (current.Count != chunkSize) continue;
                result.Add(current);
                current = new List<T>();
            }

            if (current.Count > 0)
                result.Add(current);
            return result;
        }
    }
}
