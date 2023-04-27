using JetBrains.Annotations;

namespace CombatDicesTeam.Graphs.Visualization;

/// <summary>
/// Post-processor which retry transformation if modified layout is not valid.
/// </summary>
[PublicAPI]
public sealed class RetryTransformLayoutPostProcessor<TNodePayload> : ILayoutPostProcessor<TNodePayload>
{
    private readonly IGraphNodeLayoutTransformer<TNodePayload> _nodeLayoutTransformProvider;
    private readonly IGraphNodeLayoutValidator<TNodePayload> _layoutValidator;
    private readonly int _attemptLimit;

    public RetryTransformLayoutPostProcessor(IGraphNodeLayoutTransformer<TNodePayload> layoutTransformProvider, IGraphNodeLayoutValidator<TNodePayload> layoutValidator, int attemptLimit)
    {
        _nodeLayoutTransformProvider = layoutTransformProvider;
        _layoutValidator = layoutValidator;
        _attemptLimit = attemptLimit;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> Process(IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> sourceLayouts)
    {
        var processedList = new List<IGraphNodeLayout<TNodePayload>>();
        var openList = new List<IGraphNodeLayout<TNodePayload>>(sourceLayouts);

        var attemptIndex = 0;
        while (attemptIndex < _attemptLimit - 1)
        {
            foreach (var layout in openList.ToArray())
            {
                var transformedLayout = _nodeLayoutTransformProvider.Get(layout);
                var isValidTransformation = _layoutValidator.Validate(transformedLayout, sourceLayouts);

                if (isValidTransformation)
                {
                    processedList.Add(transformedLayout);
                    openList.Remove(layout);
                }
            }

            attemptIndex++;

            if (!openList.Any())
            {
                break;
            }
        }

        if (openList.Any())
        {
            processedList.AddRange(openList);
        }

        return processedList;
    }
}
