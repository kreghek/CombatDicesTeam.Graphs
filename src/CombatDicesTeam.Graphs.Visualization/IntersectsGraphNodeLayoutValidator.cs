using JetBrains.Annotations;

namespace CombatDicesTeam.Graphs.Visualization;

/// <summary>
/// Validator to test layout instersections.
/// </summary>
[PublicAPI]
public sealed class IntersectsGraphNodeLayoutValidator<TNodePayload> : IGraphNodeLayoutValidator<TNodePayload>
{
    public bool Validate(IGraphNodeLayout<TNodePayload> layout, IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> sourceLayouts)
    {
        var otherLayouts = sourceLayouts.Where(x => x.Node != layout.Node).ToArray();

        return !Intersects(layout.Position, layout.Size, otherLayouts);
    }

    private static bool Intersects(Position position, Size size, IGraphNodeLayout<TNodePayload>[] testedLayouts)
    {
        var x1 = position.X;
        var y1 = position.Y;
        var x2 = position.X + size.Width;
        var y2 = position.Y + size.Height;

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var layout in testedLayouts)
        {
            var x1A = layout.Position.X;
            var y1A = layout.Position.Y;
            var x2A = layout.Position.X + layout.Size.Width;
            var y2A = layout.Position.Y + layout.Size.Height;

            var xIntersects = x2A >= x1 && x1A <= x2;
            var yIntersects = y2A >= y1 && y1A <= y2;
            if (xIntersects && yIntersects)
            {
                return true;
            }
        }

        return false;
    }
}
