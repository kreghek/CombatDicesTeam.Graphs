using JetBrains.Annotations;

namespace CombatDicesTeam.Graphs.Visualization;

[PublicAPI]
public sealed class HorizontalGraphVisualizer<TValueData> : IGraphNodeVisualizer<TValueData>
{
    private static IReadOnlyCollection<IGraphNode<TValueData>> GetNextLevelNodes(IGraph<TValueData> graph,
        IReadOnlyCollection<IGraphNode<TValueData>> roots)
    {
        return roots.Select(graph.GetNext).SelectMany(x => x).Distinct().ToArray();
    }

    private static IReadOnlyCollection<IGraphNode<TValueData>> GetRoots(IGraph<TValueData> campaignGraph)
    {
        // Look node are not targets for other nodes.

        var nodesOpenList = campaignGraph.GetAllNodes().ToList();

        foreach (var node in nodesOpenList.ToArray())
        {
            var otherNodes = campaignGraph.GetAllNodes().Where(x => x != node).ToArray();

            foreach (var otherNode in otherNodes)
            {
                var nextNodes = campaignGraph.GetNext(otherNode);

                if (nextNodes.Contains(node))
                {
                    nodesOpenList.Remove(node);
                }
            }
        }

        return nodesOpenList;
    }

    private static IEnumerable<IReadOnlyList<T>> GetPermutations<T>(IReadOnlyList<T> list, int length)
    {
        if (length == 1)
        {
            return list.Select(t => new[] { t });
        }

        var permutationsMinus1 = GetPermutations(list, length - 1);
        return permutationsMinus1
            .SelectMany(t => list.Where(e => !t.Contains(e)),
                (t1, t2) => t1.Concat(new[] { t2 }).ToArray());
    }

    private static IEnumerable<IReadOnlyList<T>> GetPermutations<T>(IReadOnlyList<T> list)
    {
        return GetPermutations(list, list.Count);
    }

    private static (IReadOnlyList<IReadOnlyList<IGraphNode<TValueData>>> TotalLevels, bool Success) CollectLevelInner(
        IGraph<TValueData> graph,
        IReadOnlyCollection<IGraphNode<TValueData>> stableCurrentLevelNodes,
        IReadOnlyList<IReadOnlyList<IGraphNode<TValueData>>> currentTotalLevels)
    {
        var nextLevelNodes = GetNextLevelNodes(graph, stableCurrentLevelNodes).ToArray();

        if (!nextLevelNodes.Any())
        {
            return (currentTotalLevels, true);
        }

        var nextLevelNodesPerms = GetPermutations(nextLevelNodes);

        foreach (var nextLevelNodesPerm in nextLevelNodesPerms)
        {
            var nextHaveSameOrder = CheckNextLevelHaveSameOrder(
                graph,
                stableCurrentLevelNodes,
                nextLevelNodesPerm);

            if (!nextHaveSameOrder)
            {
                // Next level nodes is in wrong order.
                // Try next permutation.
                continue;
            }

            var totalList = new List<IReadOnlyList<IGraphNode<TValueData>>>(currentTotalLevels)
            {
                nextLevelNodesPerm
            };
            var (totalLevels, success) = CollectLevelInner(graph, nextLevelNodesPerm, totalList);

            if (success)
            {
                return (totalLevels, true);
            }
        }

        return (currentTotalLevels, false);
    }

    private static IReadOnlyList<IReadOnlyList<IGraphNode<TValueData>>> CollectLevels(IGraph<TValueData> graph,
        IReadOnlyList<IGraphNode<TValueData>> rootNodes)
    {
        var rootNodesPerms = GetPermutations(rootNodes, rootNodes.Count);

        foreach (var rootNodesPerm in rootNodesPerms)
        {
            var totalList = new List<IReadOnlyList<IGraphNode<TValueData>>>
            {
                rootNodesPerm
            };

            var (totalLevels, success) = CollectLevelInner(graph, rootNodesPerm, totalList);

            if (success)
            {
                return totalLevels;
            }
        }

        throw new InvalidOperationException();
    }

    private static bool CheckNextLevelHaveSameOrder(
        IGraph<TValueData> graph,
        IReadOnlyCollection<IGraphNode<TValueData>> currentLevelNodes,
        IReadOnlyCollection<IGraphNode<TValueData>> nextLevelNodes)
    {
        var weightedCurrentLevelNodes = currentLevelNodes.Select((x, i) => (x, i)).ToArray();
        var weightedNextLevelNodes = nextLevelNodes.Select(x => new
        {
            Node = x,
            Weight = CalculateChildWeight(graph, x, weightedCurrentLevelNodes)
        }).ToArray();

        var nextLevelNodeWeights = weightedNextLevelNodes.Select(x => x.Weight).ToArray();

        return CheckListIsOrdered(nextLevelNodeWeights);
    }

    private static bool CheckListIsOrdered(IReadOnlyList<double> list)
    {
        return !list.Where((t, i) => i > 0 && t < list[i - 1]).Any();
    }

    /// <summary>
    /// Calculates weight of node base on parent weight.
    /// </summary>
    private static double CalculateChildWeight(IGraph<TValueData> graph, IGraphNode<TValueData> graphNode,
        (IGraphNode<TValueData> Node, int Weight)[] weightedParentNodes)
    {
        return weightedParentNodes.Where(x => graph.GetNext(x.Node).Contains(graphNode)).Average(x => x.Weight);
    }

    public IReadOnlyCollection<IGraphNodeLayout<TValueData>> Create(IGraph<TValueData> graph, ILayoutConfig config)
    {
        var roots = GetRoots(graph).ToArray();

        var levels = CollectLevels(graph, roots);

        var layouts = new List<IGraphNodeLayout<TValueData>>();

        var maxLevelHeight = levels.Max(x => x.Count * config.NodeSize);

        for (var levelIndex = 0; levelIndex < levels.Count; levelIndex++)
        {
            var levelItems = levels[levelIndex];
            var sumHeight = levelItems.Count * config.NodeSize;
            var offsetY = (maxLevelHeight - sumHeight) / 2;
            for (var itemIndex = 0; itemIndex < levelItems.ToArray().Length; itemIndex++)
            {
                var node = levelItems.ToArray()[itemIndex];

                var levelX = levelIndex * config.NodeSize;
                var itemY = itemIndex * config.NodeSize;
                var layoutPosition = new Position(levelX, itemY + offsetY);
                layouts.Add(new GraphNodeControl<TValueData>(node, layoutPosition));
            }
        }

        return layouts;
    }
}