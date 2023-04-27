namespace CombatDicesTeam.Graphs.Visualization;

/// <summary>
/// Node layout post-processor. Modify layouts after visualizing to gain additional effects.
/// </summary>
/// <typeparam name="TNodePayload">Type of node payload.</typeparam>
public interface ILayoutPostProcessor<TNodePayload>
{
    /// <summary>
    /// Process a set of layouts.
    /// There is no garantee to keep source order.
    /// </summary>
    /// <param name="sourceLayouts">Set of source node layouts.</param>
    /// <returns>Modified set of node layouts.</returns>
    public IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> Process(IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> sourceLayouts);
}