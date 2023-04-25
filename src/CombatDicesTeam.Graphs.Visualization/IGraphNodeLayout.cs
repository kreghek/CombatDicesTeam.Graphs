namespace CombatDicesTeam.Graphs.Visualization;

public interface IGraphNodeLayout<TValueData>
{
    public IGraphNode<TValueData> Node { get; }

    public Position Position { get; }
}