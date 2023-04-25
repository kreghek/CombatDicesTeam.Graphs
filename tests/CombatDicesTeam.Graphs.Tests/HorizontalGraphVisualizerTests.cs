using CombatDicesTeam.Graphs.Visualization;

namespace CombatDicesTeam.Graphs.Tests;

public class HorizontalGraphVisualizerTests
{
    private record TestSid(string Value);
    
    [Test]
    public void Create_SingleRoot_PlaceToZeroPosition()
    {
        // ARRANGE

        var visualizer = new HorizontalGraphVisualizer<object>();

        var graphMock = new Mock<IGraph<object>>();
        graphMock.Setup(x => x.GetAllNodes()).Returns(new[] { Mock.Of<IGraphNode<object>>() });
        graphMock.Setup(x => x.GetNext(It.IsAny<IGraphNode<object>>())).Returns(ArraySegment<IGraphNode<object>>.Empty);

        var graph = graphMock.Object;
        
        var visualizerConfig = Mock.Of<IVisualizerConfig>(x => x.NodeSize == 1);
        
        // ACT

        var layouts = visualizer.Create(graph, visualizerConfig);
        
        // ASSERT

        layouts.Should().HaveCount(1)
            .And.Subject.Single().Position.Should().Be(new Position(0, 0));
    }
    
    [Test]
    public void Create_TwoRoots_PlaceToVertically()
    {
        // ARRANGE

        var visualizer = new HorizontalGraphVisualizer<object>();

        var graphMock = new Mock<IGraph<object>>();
        graphMock.Setup(x => x.GetAllNodes()).Returns(new[]
        {
            Mock.Of<IGraphNode<object>>(),
            Mock.Of<IGraphNode<object>>()
        });
        graphMock.Setup(x => x.GetNext(It.IsAny<IGraphNode<object>>())).Returns(ArraySegment<IGraphNode<object>>.Empty);

        var graph = graphMock.Object;
        
        var visualizerConfig = Mock.Of<IVisualizerConfig>(x => x.NodeSize == 1);

        var expectedPositions = new Position[]
        {
            new(0, 0),
            new(0, 1)
        };
        
        // ACT

        var layouts = visualizer.Create(graph, visualizerConfig);
        
        // ASSERT

        var positions = layouts.Select(x => x.Position).ToArray();
        positions.Should().BeEquivalentTo(expectedPositions);
    }
    
    [Test]
    public void Create_TheSimplestGraph_PlaceChildRightOnParent()
    {
        // ARRANGE

        var visualizer = new HorizontalGraphVisualizer<object>();

        var graphMock = new Mock<IGraph<object>>();

        var root = Mock.Of<IGraphNode<object>>();
        var child = Mock.Of<IGraphNode<object>>();
        
        graphMock.Setup(x => x.GetAllNodes()).Returns(new[] { root, child });
        graphMock.Setup(x => x.GetNext(It.Is<IGraphNode<object>>(n => n == root))).Returns(new[]{child });
        graphMock.Setup(x => x.GetNext(It.Is<IGraphNode<object>>(n => n == child)))
            .Returns(ArraySegment<IGraphNode<object>>.Empty);

        var graph = graphMock.Object;
        
        var visualizerConfig = Mock.Of<IVisualizerConfig>(x => x.NodeSize == 1);

        var expectedPositions = new Position[]
        {
            new(0, 0),
            new(1, 0)
        };
        
        // ACT

        var layouts = visualizer.Create(graph, visualizerConfig);
        
        // ASSERT

        layouts.Should().AllSatisfy(x => expectedPositions.Contains(x.Position));
    }
}