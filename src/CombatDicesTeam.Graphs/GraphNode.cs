using JetBrains.Annotations;

namespace CombatDicesTeam.Graphs;

[PublicAPI]
public sealed class GraphNode<TValueData> : IGraphNode<TValueData>
{
    public GraphNode(TValueData data)
    {
        Value = data;
    }

    public TValueData Value { get; }
}