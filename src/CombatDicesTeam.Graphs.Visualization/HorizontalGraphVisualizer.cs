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

    private static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
    {
        if (length == 1) return list.Select(t => new[] { t });

        var materializedList = list as T[] ?? list.ToArray();
        return GetPermutations(materializedList, length - 1)
            .SelectMany(t => materializedList.Where(e => !t.Contains(e)),
                (t1, t2) => t1.Concat(new[] { t2 }));
    }

    private (IReadOnlyList<IReadOnlyList<IGraphNode<TValueData>>>? TotalLevels, bool Success) CollectLevelInner(
        IGraph<TValueData> graph,
        IReadOnlyCollection<IGraphNode<TValueData>> stableCurrentLevelNodes,
        IReadOnlyList<IReadOnlyList<IGraphNode<TValueData>>> currentTotalLevels)
    {

        var nextLevelNodes = GetNextLevelNodes(graph, stableCurrentLevelNodes);

        if (!nextLevelNodes.Any())
        {
            return (currentTotalLevels, true);
        }

        var nextLevelNodesPerms = GetPermutations(nextLevelNodes, nextLevelNodes.Count).ToArray();

        foreach (var nextLevelNodesPerm in nextLevelNodesPerms)
        {
            var levelNodesPermMaterialized =
                nextLevelNodesPerm as IGraphNode<TValueData>[] ?? nextLevelNodesPerm.ToArray();

            var nextHaveSameOrder =
                CheckNextLevelHaveSameOrder(graph, stableCurrentLevelNodes, levelNodesPermMaterialized);

            if (!nextHaveSameOrder)
            {
                // Next level nodes is in wrong order.
                // Try next permutation.
                continue;
            }

            var totalList = new List<IReadOnlyList<IGraphNode<TValueData>>>(currentTotalLevels)
            {
                levelNodesPermMaterialized
            };
            var next = CollectLevelInner(graph, levelNodesPermMaterialized, totalList);

            if (next.Success)
            {
                return (next.TotalLevels, true);
            }
        }

        return (null, false);
    }

    private IReadOnlyList<IReadOnlyList<IGraphNode<TValueData>>> CollectLevels(IGraph<TValueData> graph,
        IReadOnlyCollection<IGraphNode<TValueData>> rootNodes)
    {
        var rootNodesPerms = GetPermutations(rootNodes, rootNodes.Count).ToArray();

        foreach (var rootNodesPerm in rootNodesPerms)
        {
            var rootNodesPermMaterialized = rootNodesPerm as IGraphNode<TValueData>[] ?? rootNodesPerm.ToArray();
            var totalList = new List<IReadOnlyList<IGraphNode<TValueData>>>
            {
                rootNodesPermMaterialized.ToArray()
            };

            var (totalLevels, success) = CollectLevelInner(graph, rootNodesPermMaterialized.ToArray(), totalList);

            if (success && totalLevels is not null)
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

    /// <summary>
    /// Check two collections are contain same elements without order.
    /// </summary>
    private static bool CheckCollectionsAreSame(IReadOnlyCollection<IGraphNode<TValueData>> list1,
        IReadOnlyCollection<IGraphNode<TValueData>> list2)
    {
        return list1.Count == list2.Count && list1.All(list2.Contains);
    }

    public IReadOnlyCollection<IGraphNodeLayout<TValueData>> Create(IGraph<TValueData> graph, ILayoutConfig config)
    {
        var roots = GetRoots(graph);

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