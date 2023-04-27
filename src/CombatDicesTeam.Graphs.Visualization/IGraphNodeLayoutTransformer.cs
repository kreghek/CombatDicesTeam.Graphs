namespace CombatDicesTeam.Graphs.Visualization;

/// <summary>
/// Transformer of node layout.
/// </summary>
/// <typeparam name="TNodeLayout">Type of node payload.</typeparam>
/// <remarks>
/// - Used in the post-processor to handle each node layout personally.
/// - There is no garantee to return same layout object.
/// </remarks>
public interface IGraphNodeLayoutTransformer<TNodeLayout>
{
    /// <summary>
    /// Get position offset for specified layout.
    /// </summary>
    /// <param name="layout">Layout to transform.</param>
    /// <returns>Modified layout.</returns>
    IGraphNodeLayout<TNodeLayout> Get(IGraphNodeLayout<TNodeLayout> layout);
}
