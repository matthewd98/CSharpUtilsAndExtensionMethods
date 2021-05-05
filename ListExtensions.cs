using System.Collections.Generic;

namespace Program
{
    public static class ListExtensions
    {
        public static List<T> ObjectToList<T>(this T instance)
        {
            return new List<T> { instance };
        }
		
		public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}