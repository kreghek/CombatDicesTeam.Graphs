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

    public IReadOnlyCollection<IGraphNodeLayout<TValueData>> Create(IGraph<TValueData> graph, ILayoutConfig config)
    {
        var roots = GetRoots(graph);

        var levels = new List<IReadOnlyCollection<IGraphNode<TValueData>>>
        {
            roots
        };

        var currentList = roots;
        while (true)
        {
            var openListNextLevel = GetNextLevelNodes(graph, currentList);
            levels.Add(openListNextLevel.ToArray());

            if (!openListNextLevel.Any())
            {
                break;
            }

            currentList = openListNextLevel;
        }

        var controls = new List<IGraphNodeLayout<TValueData>>();

        var maxLevelHeight = levels.Max(x => x.Count * config.NodeSize);

        for (var levelIndex = 0; levelIndex < levels.Count; levelIndex++)
        {
            var levelItems = levels[levelIndex];
            var sumHeight = levelItems.Count * config.NodeSize;
            var offsetY = (maxLevelHeight - sumHeight) / 2;
            for (var itemIndex = 0; itemIndex < levelItems.ToArray().Length; itemIndex++)
            {
                var node = levelItems.ToArray()[itemIndex];

                controls.Add(new GraphNodeControl<TValueData>(node,
                    new Position(levelIndex * config.NodeSize, itemIndex * config.NodeSize + offsetY)));
            }
        }

        return controls;
    }
}