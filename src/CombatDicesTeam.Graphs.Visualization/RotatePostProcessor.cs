using JetBrains.Annotations;

namespace CombatDicesTeam.Graphs.Visualization;

[PublicAPI]
public sealed class RotatePostProcessor<TNodePayload> : ILayoutPostProcessor<TNodePayload>
{
    private readonly double _radians;

    public RotatePostProcessor(double radians)
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