namespace CombatDicesTeam.Graphs.Visualization.Tests;

public class RepeatPostProcessorTests
{
    [Test]
    public void Process_MultipleIterations_BaseProcessorsCalledMultipleTimes()
    {
        var baseProcessorMock = new Mock<ILayoutPostProcessor<object>>();
        baseProcessorMock.Setup(x => x.Process(It.IsAny<IReadOnlyCollection<IGraphNodeLayout<object>>>()))
            .Returns(new []
            {
                Mock.Of<IGraphNodeLayout<object>>()
            });

        var processor = new RepeatPostProcessor<object>(2, baseProcessorMock.Object);

        var sourceLayouts = new[]
        {
            Mock.Of<IGraphNodeLayout<object>>()
        };
        
        // ACT

        processor.Process(sourceLayouts);
        
        // ASSERT

        baseProcessorMock.Verify(x => x.Process(It.IsAny<IReadOnlyCollection<IGraphNodeLayout<object>>>()),
            Times.Exactly(2));
    }
}