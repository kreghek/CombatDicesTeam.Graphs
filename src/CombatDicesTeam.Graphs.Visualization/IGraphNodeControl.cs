namespace CombatDicesTeam.Graphs.Visualization;

public interface IGraphNodeControl<TValueData>
{
    public IGraphNode<TValueData> Node { get; }

    public Position Position { get; }
}