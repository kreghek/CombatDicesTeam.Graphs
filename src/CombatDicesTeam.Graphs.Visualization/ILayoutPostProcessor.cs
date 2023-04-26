namespace CombatDicesTeam.Graphs.Visualization;
public interface ILayoutPostProcessor<TNodePayload>
{
    public IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> Process(IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> sourceLayouts);
}