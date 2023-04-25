namespace CombatDicesTeam.Graphs.Visualization;

public sealed record GraphNodeControl<TValueData>(IGraphNode<TValueData> Node, Position Position) : IGraphNodeControl<TValueData>;