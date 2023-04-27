using JetBrains.Annotations;

namespace CombatDicesTeam.Graphs.Visualization;

[PublicAPI]
public sealed class PushHorizontallyPostProcessor<TNodePayload> : ILayoutPostProcessor<TNodePayload>
{
    private readonly int _distance;

    public PushHorizontallyPostProcessor(int distance)
    {
        _distance = distance;
    }

    public IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> Process(
        IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> sourceLayouts)
    {
        var modified = sourceLayouts.OrderBy(x => x.Position.X).ToArray();

        for (var i = 1; i < modified.Length; i++)
        {
            var currentDistance = modified[i].Position.X - modified[i - 1].Position.X + modified[i - 1].Size.Width;

            for (var j = i; j < modified.Length; j++)
            {
                var layout = modified[j];

                modified[j] = new GraphNodeLayout<TNodePayload>(layout.Node,
                    layout.Position with
                    {
                        X = layout.Position.X + _distance
                    }, layout.Size);
            }
        }

        return modified;
    }
}