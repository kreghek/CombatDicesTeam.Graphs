using JetBrains.Annotations;

namespace CombatDicesTeam.Graphs;

/// <summary>
/// Base implementation of the graph node.
/// </summary>
/// <typeparam name="TValueData"></typeparam>
[PublicAPI]
public sealed class GraphNode<TValueData> : IGraphNode<TValueData>
{
    public GraphNode(TValueData data)
    {
        Payload = data;
    }

    public TValueData Payload { get; }
}