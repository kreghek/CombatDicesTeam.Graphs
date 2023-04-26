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
        if (length == 1) return list.Select(t => new T[] { t });

        return GetPermutations(list, length - 1)
            .SelectMany(t => list.Where(e => !t.Contains(e)),
                (t1, t2) => t1.Concat(new T[] { t2 }));
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
            var nextHaveSameOrder =
                CheckNextLevelHaveSameOrder(graph, stableCurrentLevelNodes, nextLevelNodesPerm.ToArray());

            if (nextHaveSameOrder)
            {
                var totalList = new List<IReadOnlyList<IGraphNode<TValueData>>>(currentTotalLevels);
                totalList.Add(nextLevelNodesPerm.ToArray());
                var next = CollectLevelInner(graph, nextLevelNodesPerm.ToArray(), totalList);

                if (next.Success)
                {
                    return (next.TotalLevels, true);
                }
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
            var totalList = new List<IReadOnlyList<IGraphNode<TValueData>>>();
            totalList.Add(rootNodesPerm.ToArray());
            
            var next = CollectLevelInner(graph, rootNodesPerm.ToArray(), totalList);

            if (next.Success)
            {
                return next.TotalLevels;
            }
        }

        throw new InvalidOperationException();
    }

    private static bool CheckNextLevelHaveSameOrder(
        IGraph<TValueData> graph,
        IReadOnlyCollection<IGraphNode<TValueData>> currentLevelNodes,
        IReadOnlyCollection<IGraphNode<TValueData>> nextLevelNodes)
    {
        var numCurrentLevelNodes = currentLevelNodes.Select((x, i) =>  (x, i) ).ToArray();
        var numNextLevelNodes = nextLevelNodes.Select(x => new { Node = x, Weight = GetWeight(graph, x, numCurrentLevelNodes) }).ToArray();

        for (int i = 0; i < numNextLevelNodes.Length; i++)
        {
            if (i > 0)
            {
                if (numNextLevelNodes[i].Weight < numNextLevelNodes[i - 1].Weight)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static double GetWeight(IGraph<TValueData> graph, IGraphNode<TValueData> graphNode,
        (IGraphNode<TValueData> Node, int Weight)[] numCurrentLevelNodes)
    {
        return numCurrentLevelNodes.Where(x => graph.GetNext(x.Node).Contains(graphNode)).Average(x => x.Weight);
    }

    private static bool CheckCollectionsAreSame(IGraphNode<TValueData>[] testedOpenList, IReadOnlyCollection<IGraphNode<TValueData>> nextInGraph)
    {
        return testedOpenList.All(graphNode => nextInGraph.Contains(graphNode));
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