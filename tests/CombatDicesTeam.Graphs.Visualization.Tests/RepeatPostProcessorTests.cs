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