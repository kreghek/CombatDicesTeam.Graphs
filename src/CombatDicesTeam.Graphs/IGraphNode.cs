namespace CombatDicesTeam.Graphs;

public interface IGraphNode<TValueData>
{
    public TValueData Value { get; }
}