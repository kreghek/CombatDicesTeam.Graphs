namespace CombatDicesTeam.Graphs.Visualization;

public interface IGraphNodeVisualizer<TValueData>
{
    public IReadOnlyCollection<IGraphNodeLayout<TValueData>> Create(IGraph<TValueData> graph, ILayoutConfig config);
}