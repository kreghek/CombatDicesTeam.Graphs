using JetBrains.Annotations;

namespace CombatDicesTeam.Graphs.Visualization;

[PublicAPI]
public sealed class ScaleHorizontallyPostProcessor<TNodePayload> : ILayoutPostProcessor<TNodePayload>
{
    private readonly int _distance;

    public ScaleHorizontallyPostProcessor(int distance)
    {
        _distance = distance;
    }

    public IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> Process(
        IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> sourceLayouts)
    {
        return sourceLayouts.Select(layout => new GraphNodeLayout<TNodePayload>(layout.Node,
            layout.Position with
            {
                X = layout.Position.X + _distance
            }, layout.Size)).ToArray();
    }
}