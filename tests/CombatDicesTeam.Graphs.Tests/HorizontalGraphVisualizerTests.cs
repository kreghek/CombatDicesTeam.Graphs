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
        
        var visualizerConfig = Mock.Of<ILayoutConfig>(x => x.NodeSize == 1);
        
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
        
        var visualizerConfig = Mock.Of<ILayoutConfig>(x => x.NodeSize == 1);

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
        
        var visualizerConfig = Mock.Of<ILayoutConfig>(x => x.NodeSize == 1);

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

    [Test]
    public void Create_LinearGraph_ReturnsNodesInLine()
    {
        // ARRANGE
        
        var graph = new Graph<object>();

        GraphNode<object>? prevNode = null;
        for (var i = 0; i < 9; i++)
        {
            var graphNode = new GraphNode<object>(i);
            graph.AddNode(graphNode);
            if (prevNode is not null)
            {
                graph.ConnectNodes(prevNode, graphNode);
            }

            prevNode = graphNode;
        }
        
        var visualizer = new HorizontalGraphVisualizer<object>();
        
        var layoutConfig = Mock.Of<ILayoutConfig>(x => x.NodeSize == 1);
        
        // ACT

        var layouts = visualizer.Create(graph, layoutConfig);
        
        // ASSERT

        layouts.Should().HaveCount(9);
        
        var layoutLine = layouts.OrderBy(x => x.Position.X).ToArray();
        for (var index = 0; index < layoutLine.Length; index++)
        {
            var layout = layoutLine[index];

            if (index > 0)
            {
                var prevLayout = layoutLine[index - 1];

                layout.Position.X.Should().BeGreaterThan(prevLayout.Position.X);
            }
        }
    }

    [Test]
    public void Create_ForkGraph_RootsInSameXAndChildMoved()
    {
        // ARRANGE

        var visualizer = new HorizontalGraphVisualizer<int>();

        var graphMock = new Mock<IGraph<int>>();

        var root = Mock.Of<IGraphNode<int>>(n => n.Value == 0);
        var child1 = Mock.Of<IGraphNode<int>>(n => n.Value == 1);
        var child2 = Mock.Of<IGraphNode<int>>(n => n.Value == 2);

        graphMock.Setup(x => x.GetAllNodes()).Returns(new[] { root, child1, child2 });
        graphMock.Setup(x => x.GetNext(It.Is<IGraphNode<int>>(n => n == root)))
            .Returns(new[] { child1, child2 });
        graphMock.Setup(x => x.GetNext(It.Is<IGraphNode<int>>(n => n == child1 || n == child2)))
            .Returns(ArraySegment<IGraphNode<int>>.Empty);

        var graph = graphMock.Object;

        var layoutConfig = Mock.Of<ILayoutConfig>(x => x.NodeSize == 1);

        // ACT

        var layouts = visualizer.Create(graph, layoutConfig);

        // ASSERT

        layouts.Should().Satisfy(
            layout => layout.Node.Value == 0 && layout.Position.X == 0,
            layout => (layout.Node.Value == 1 || layout.Node.Value == 2) && layout.Position.X == 1,
            layout => (layout.Node.Value == 1 || layout.Node.Value == 2) && layout.Position.X == 1);
    }
    
    [Test]
    public void Create_MergeGraph_RootsInSameXAndChildMoved()
    {
        // ARRANGE

        var visualizer = new HorizontalGraphVisualizer<int>();

        var graphMock = new Mock<IGraph<int>>();

        var root1 = Mock.Of<IGraphNode<int>>(n => n.Value == 0);
        var root2 = Mock.Of<IGraphNode<int>>(n => n.Value == 1);
        var child = Mock.Of<IGraphNode<int>>(n => n.Value == 2);

        graphMock.Setup(x => x.GetAllNodes()).Returns(new[] { root1, root2, child });
        graphMock.Setup(x => x.GetNext(It.Is<IGraphNode<int>>(n => n == root1 || n == root2)))
            .Returns(new[] { child });
        graphMock.Setup(x => x.GetNext(It.Is<IGraphNode<int>>(n => n == child)))
            .Returns(ArraySegment<IGraphNode<int>>.Empty);

        var graph = graphMock.Object;

        var layoutConfig = Mock.Of<ILayoutConfig>(x => x.NodeSize == 1);

        // ACT

        var layouts = visualizer.Create(graph, layoutConfig);

        // ASSERT

        layouts.Should().Satisfy(
            layout => (layout.Node.Value == 0 || layout.Node.Value == 1) && layout.Position.X == 0,
            layout => (layout.Node.Value == 0 || layout.Node.Value == 1) && layout.Position.X == 0,
            layout => (layout.Node.Value == 2) && layout.Position.X == 1);
    }
}