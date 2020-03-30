using System.Collections.Generic;
using System.Linq;

namespace Bds.TechTest.Extensions
{
    public static class IEnumerableExtesions
    {
        public static IEnumerable<T> MergeOneByOne<T>(this IEnumerable<T>[] results)
        {
            var enumerators = results.Select(r => r.GetEnumerator()).ToArray();
            var empty = new int[enumerators.Length];

            while (empty.Any(e => e != 1))
            {
                for (int i = 0; i < enumerators.Length; i++)
                {
                    if (empty[i] == 1)
                    {
                        continue;
                    }
                    var enumerator = enumerators[i];
                    if (enumerator.Current != null)
                    {
                        yield return enumerator.Current;
                    }
                    if (enumerator.MoveNext())
                    {
                        continue;
                    }
                    empty[i] = 1;
                }
            }
        }
    }
}
