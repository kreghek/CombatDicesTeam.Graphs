using System.Collections;

namespace CombatDicesTeam.Graphs.Visualization.Tests.HorizontalGraphVisualizerTestCases;

public static class HorizontalGraphVisualizerTestCases
{
    public static IEnumerable NodePerms2
    {
        get
        {
            var indexes = Enumerable.Range(0, 4).ToArray();

            var perms = GetPermutations(indexes, indexes.Length);

            foreach (var perm in perms)
            {
                yield return new TestCaseData(perm.ToArray());
            }
        }
    }
    
    public static IEnumerable NodePermsIsolated
    {
        get
        {
            var indexes = Enumerable.Range(0, 5).ToArray();

            var perms = GetPermutations(indexes, indexes.Length);

            foreach (var perm in perms)
            {
                yield return new TestCaseData(perm.ToArray());
            }
        }
    }
    
    private static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
    {
        if (length == 1) return list.Select(t => new [] { t });

        var materializedList = list as T[] ?? list.ToArray();
        return GetPermutations(materializedList, length - 1)
            .SelectMany(t => materializedList.Where(e => !t.Contains(e)),
                (t1, t2) => t1.Concat(new [] { t2 }));
    }
}