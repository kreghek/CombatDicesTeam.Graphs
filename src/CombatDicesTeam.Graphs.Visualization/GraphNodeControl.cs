namespace CombatDicesTeam.Graphs.Visualization;

/// <summary>
/// Base implementation of node layput.
/// </summary>
public sealed record GraphNodeLayout<TNodePayload>
    (IGraphNode<TNodePayload> Node, Position Position, Size Size) : IGraphNodeLayout<TNodePayload>;