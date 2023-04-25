using JetBrains.Annotations;

namespace CombatDicesTeam.Graphs.Visualization;

/// <summary>
/// Visualizer to create node layouts.
/// </summary>
/// <typeparam name="TNodePayload">Type of node's payload.</typeparam>
[PublicAPI]
public interface IGraphNodeVisualizer<TNodePayload>
{
    /// <summary>
    /// Creates layouts of graph nodes.
    /// </summary>
    /// <param name="graph">Graph to visualize.</param>
    /// <param name="config">Visualization config.</param>
    /// <returns>List of layouts for each graph node.</returns>
    public IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> Create(IGraph<TNodePayload> graph, ILayoutConfig config);
}