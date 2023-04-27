namespace CombatDicesTeam.Graphs.Visualization;

/// <summary>
/// Provider of the position offset for specified layout node.
/// </summary>
/// <typeparam name="TNodeLayout">Type of node payload.</typeparam>
/// <remarks>
/// Used in the post-processor to handle each node layout personally.
/// </remarks>
public interface IGraphNodeLayoutTransformProvider<TNodeLayout>
{
    /// <summary>
    /// Get position offset for specified layout.
    /// </summary>
    /// <param name="layout">Layout to transform.</param>
    /// <returns>Modified layout.</returns>
    IGraphNodeLayout<TNodeLayout> Get(IGraphNodeLayout<TNodeLayout> layout);
}
