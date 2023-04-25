namespace CombatDicesTeam.Graphs;

public sealed class GraphNode<TValueData> : IGraphNode<TValueData>
{
    public GraphNode(TValueData data)
    {
        Value = data;
    }

    public TValueData Value { get; }
}