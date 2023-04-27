using JetBrains.Annotations;

namespace CombatDicesTeam.Graphs.Visualization;

[PublicAPI]
public sealed class RotatePostProcessor<TNodePayload> : ILayoutPostProcessor<TNodePayload>
{
    private readonly float _radians;

    public RotatePostProcessor(float radians)
    {
        _radians = radians;
    }

    public IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> Process(IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> sourceLayouts)
    {
        return sourceLayouts.Select(layout => new GraphNodeLayout<TNodePayload>(layout.Node,
            GetPosition(layout), layout.Size)).ToArray();
    }

    private Position GetPosition(IGraphNodeLayout<TNodePayload> layout)
    {
        var distance = Math.Sqrt(Math.Pow(layout.Position.X, 2) + Math.Pow(layout.Position.Y, 2));
        var currentAngle = Math.Atan2(layout.Position.Y, layout.Position.X);

        var x = Math.Cos(currentAngle + _radians) * distance;
        var y = Math.Sin(currentAngle + _radians) * distance;
        return new Position((int)x, (int)y);
    }
}

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

[PublicAPI]
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

            if (!Intersects(newPosition, layout.Size, otherLayouts))
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

    private static bool Intersects(Position newPosition, Size size, IGraphNodeLayout<TNodePayload>[] otherLayouts)
    {
        var x1 = newPosition.X;
        var y1 = newPosition.Y;
        var x2 = newPosition.X + size.Width;
        var y2 = newPosition.Y + size.Height;

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var layout in otherLayouts)
        {
            var x1A = layout.Position.X;
            var y1A = layout.Position.Y;
            var x2A = layout.Position.X + layout.Size.Width;
            var y2A = layout.Position.Y + layout.Size.Height;

            if ((x2A >= x1 && x1A <= x2) && (y2A >= y1 && y1A <= y2))
            {
                return true;
            }
        }

        return false;
    }
}
