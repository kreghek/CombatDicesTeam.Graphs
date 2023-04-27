namespace CombatDicesTeam.Graphs.Visualization;

/// <summary>
/// Validator of node layout.
/// </summary>
/// <typeparam name="TNodePayload">Type of ode payload.</typeparam>
/// <remarks>
/// Used to validate modified layoutand retry to modify. Especially with random-based <see cref="IGraphNodeLayoutTransformer{TNodeLayout}"/>.
/// </remarks>
public interface IGraphNodeLayoutValidator<in TNodePayload>
{
    bool Validate(IGraphNodeLayout<TNodePayload> layout, IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> sourceLayouts);
}
