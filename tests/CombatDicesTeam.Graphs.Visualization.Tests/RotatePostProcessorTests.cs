namespace CombatDicesTeam.Graphs.Visualization.Tests;

public class RotatePostProcessorTests
{
    [Test]
    public void Process_MultipleIterations_BaseProcessorsCalledMultipleTimes()
    {
        // ARRANGE
        
        var processor = new RotatePostProcessor<object>(Math.PI / 2);

        var sourceLayouts = new[]
        {
            Mock.Of<IGraphNodeLayout<object>>(x=>x.Position == new Position(1, 0))
        };
        
        // ACT

        var layouts = processor.Process(sourceLayouts);
        
        // ASSERT

        layouts.First().Position.X.Should().Be(0);
        layouts.First().Position.Y.Should().Be(1);
    }
    
    [Test]
    public void Process_MultipleIterations_BaseProcessorsCalledMultipleTimes2()
    {
        // ARRANGE
        
        var processor = new RotatePostProcessor<object>(Math.PI);

        var sourceLayouts = new[]
        {
            Mock.Of<IGraphNodeLayout<object>>(x=>x.Position == new Position(1, 0))
        };
        
        // ACT

        var layouts = processor.Process(sourceLayouts);
        
        // ASSERT

        layouts.First().Position.X.Should().Be(-1);
        layouts.First().Position.Y.Should().Be(0);
    }
    
    [Test]
    public void Process_MultipleIterations_BaseProcessorsCalledMultipleTimes3()
    {
        // ARRANGE
        
        var processor = new RotatePostProcessor<object>(Math.PI / 2);

        var sourceLayouts = new[]
        {
            Mock.Of<IGraphNodeLayout<object>>(x=>x.Position == new Position(1, 1))
        };
        
        // ACT

        var layouts = processor.Process(sourceLayouts);
        
        // ASSERT

        layouts.First().Position.X.Should().Be(-1);
        layouts.First().Position.Y.Should().Be(1);
    }
}