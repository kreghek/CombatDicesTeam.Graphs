using JetBrains.Annotations;

namespace CombatDicesTeam.Graphs.Visualization;

/// <summary>
/// Post-processor which retry transformation if modified layout is not valid.
/// </summary>
[PublicAPI]
public sealed class RetryTransformLayoutPostProcessor<TNodePayload> : ILayoutPostProcessor<TNodePayload>
{
    private readonly IGraphNodeLayoutTransformProvider<TNodePayload> _nodeLayoutTransformProvider;
    private readonly IGraphNodeLayoutValidator<TNodePayload> _layoutValidator;
    private readonly int _attemptLimit;

    public RetryTransformLayoutPostProcessor(IGraphNodeLayoutTransformProvider<TNodePayload> layoutTransformProvider, IGraphNodeLayoutValidator<TNodePayload> layoutValidator, int attemptLimit)
    {
        _nodeLayoutTransformProvider = layoutTransformProvider;
        _layoutValidator = layoutValidator;
        _attemptLimit = attemptLimit;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> Process(IReadOnlyCollection<IGraphNodeLayout<TNodePayload>> sourceLayouts)
    {
        var processedLayouts = new List<IGraphNodeLayout<TNodePayload>>();

        foreach (var layout in sourceLayouts)
        {
            var transformedLayout = _nodeLayoutTransformProvider.Get(layout);

            var attemptIndex = 1;
            var wasTransformed = false;
            while (true)
            { 
                var isValidTransformation = _layoutValidator.Validate(transformedLayout, sourceLayouts);

                if (isValidTransformation)
                { 
                    wasTransformed = true;
                    processedLayouts.Add(transformedLayout);
                    break;
                }

                attemptIndex++;

                if (attemptIndex > _attemptLimit)
                {
                    break;
                }
            }

            if (!wasTransformed)
            {
                processedLayouts.Add(layout);
            }
        }

        return processedLayouts;
    }
}
