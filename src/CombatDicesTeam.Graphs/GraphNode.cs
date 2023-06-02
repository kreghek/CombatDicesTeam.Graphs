using JetBrains.Annotations;

namespace CombatDicesTeam.Graphs;

/// <summary>
/// Base implementation of the graph node.
/// </summary>
/// <typeparam name="TValueData">Type of node data.</typeparam>
[PublicAPI]
public sealed class GraphNode<TValueData> : IGraphNode<TValueData>
{
    public GraphNode(TValueData data)
    {
        Payload = data;
    }

    /// <inheritdoc />
    public TValueData Payload { get; }
}