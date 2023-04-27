namespace CombatDicesTeam.Graphs.Visualization.Tests;

public class RetryTransformLayoutPostProcessorTests
{
    [Test]
    public void Process_FirstAttemptIsInvalid_CallTransformation2Times()
    {
        // ARRANGE

        var transformerMock = new Mock<IGraphNodeLayoutTransformer<object>>();
        transformerMock.Setup(x => x.Get(It.IsAny<IGraphNodeLayout<object>>())).Returns(Mock.Of<IGraphNodeLayout<object>>());

        var attemptIndex = 0;
        var validatorMock = new Mock<IGraphNodeLayoutValidator<object>>();
        validatorMock.Setup(x => x.Validate(It.IsAny<IGraphNodeLayout<object>>(), It.IsAny<IReadOnlyCollection<IGraphNodeLayout<object>>>())).Returns(() =>
        {
            attemptIndex++;
            return attemptIndex - 1 == 0;
        });

        var processor = new RetryTransformLayoutPostProcessor<object>(transformerMock.Object, validatorMock.Object, 2);

        var sourceLayouts = new[]
        {
            Mock.Of<IGraphNodeLayout<object>>(x=>x.Position == new Position(0, 0))
        };

        // ACT

        var layouts = processor.Process(sourceLayouts);

        // ASSERT

        transformerMock.Verify(x => x.Get(It.IsAny<IGraphNodeLayout<object>>()), Times.Exactly(1));
    }
}