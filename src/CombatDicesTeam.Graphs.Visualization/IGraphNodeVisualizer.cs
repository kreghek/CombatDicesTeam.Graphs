namespace CombatDicesTeam.Graphs.Visualization;

public interface IGraphNodeVisualizer<TValueData>
{
    public IReadOnlyCollection<IGraphNodeControl<TValueData>> Create(IGraph<TValueData> graph, IVisualizerConfig config);
}