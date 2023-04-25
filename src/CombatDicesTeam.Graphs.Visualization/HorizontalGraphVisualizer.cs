using JetBrains.Annotations;

namespace CombatDicesTeam.Graphs.Visualization;

[PublicAPI]
public sealed class HorizontalGraphVisualizer<TValueData> : IGraphNodeVisualizer<TValueData>
{
    public IReadOnlyCollection<IGraphNodeControl<TValueData>> Create(IGraph<TValueData> graph, IVisualizerConfig config)
    {
        var roots = GetRoots(graph);

        var levels = new List<IReadOnlyCollection<IGraphNode<TValueData>>>
        {
            roots
        };

        var controls = new List<IGraphNodeControl<TValueData>>();

        for (var levelIndex = 0; levelIndex < levels.Count; levelIndex++)
        {
            var levelItems = levels[levelIndex];
            for (var itemIndex = 0; itemIndex < levelItems.ToArray().Length; itemIndex++)
            {
                var node = levelItems.ToArray()[itemIndex];
                controls.Add(new GraphNodeControl<TValueData>(node,
                    new Position(levelIndex * config.NodeSize, itemIndex * config.NodeSize)));
            }
        }

        return controls;
    }
    
    private IReadOnlyCollection<IGraphNode<TValueData>> GetRoots(IGraph<TValueData> campaignGraph)
    {
        // Look node are not targets for other nodes.
        
        var nodesOpenList = campaignGraph.GetAllNodes().ToList();

        foreach (var node in nodesOpenList.ToArray())
        {
            var otherNodes = campaignGraph.GetAllNodes().Where(x=>x != node).ToArray();

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
}