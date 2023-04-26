namespace CombatDicesTeam.Graphs.Visualization;

public sealed class PositionOffsetLayoutPostProcessor<TNodePayload> : ILayoutPostProcessor<TNodePayload>
{
    private readonly IPositionOffsetRandomSource _positionOffsetRandomSource;

    public PositionOffsetLayoutPostProcessor(IPositionOffsetRandomSource positionOffsetRandomSource)
    {
        _positionOffsetRandomSource = positionOffsetRandomSource;
    }

    public IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> Process(IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> sourceLayouts)
    {
        var processedLayouts = new List<IGraphNodeLayout<TNodePayload>>();

        foreach (var layout in sourceLayouts)
        {
            var offset = _positionOffsetRandomSource.GetNext();
            var newPosition = new Position(layout.Position.X + offset.X, layout.Position.Y + offset.Y);

            var otherLayouts = sourceLayouts.Where(x => x != layout).ToArray();

            if (!PositionOffsetLayoutPostProcessor<TNodePayload>.Instersects(newPosition, layout.Size, otherLayouts))
            {
                var changedLayout = new GraphNodeLayout<TNodePayload>(layout.Node, newPosition, layout.Size);
                processedLayouts.Add(changedLayout);
            }
            else
            {
                processedLayouts.Add(layout);
            }
        }

        return processedLayouts;
    }

    private static bool Instersects(Position newPosition, Size size, IGraphNodeLayout<TNodePayload>[] otherLayouts)
    {
        var x1 = newPosition.X;
        var y1 = newPosition.Y;
        var x2 = newPosition.X + size.Width;
        var y2 = newPosition.Y + size.Height;

        foreach (var layout in otherLayouts)
        {
            var x1a = layout.Position.X;
            var y1a = layout.Position.Y;
            var x2a = layout.Position.X + layout.Size.Width;
            var y2a = layout.Position.Y + layout.Size.Height;

            if ((x2a >= x1 && x1a <= x2) && (y2a >= y1 && y1a <= y2))
            {
                return true;
            }
        }

        return false;
    }
}
