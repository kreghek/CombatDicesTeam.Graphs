namespace CombatDicesTeam.Graphs.Visualization.Tests;

public class ScaleHorizontallyPostProcessorTests
{
    [Test]
    public void Process_MultipleIterations_BaseProcessorsCalledMultipleTimes()
    {
        // ARRANGE
        
        var processor = new ScaleHorizontallyPostProcessor<object>(1);

        var sourceLayouts = new[]
        {
            Mock.Of<IGraphNodeLayout<object>>(x=>x.Position == new Position(0, 0))
        };
        
        // ACT

        var layouts = processor.Process(sourceLayouts);
        
        // ASSERT

        layouts.First().Position.X.Should().Be(1);
    }
}
