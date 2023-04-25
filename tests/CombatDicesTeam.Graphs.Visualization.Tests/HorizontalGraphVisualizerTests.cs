namespace CombatDicesTeam.Graphs.Visualization.Tests;

public sealed class HorizontalGraphVisualizerTests
{
    private const int NODE_SIZE = 1;
    private readonly ILayoutConfig _layoutConfig = Mock.Of<ILayoutConfig>(x => x.NodeSize == NODE_SIZE);

    /// <summary>
    /// Test checks a graph with single root and multiple children visualized horizontally.
    /// </summary>
    [Test]
    public void Create_ForkGraph_RootsInSameXAndChildMoved()
    {
        // ARRANGE

        var visualizer = new HorizontalGraphVisualizer<int>();

        var graphMock = new Mock<IGraph<int>>();

        var root = Mock.Of<IGraphNode<int>>(n => n.Payload == 0);
        var child1 = Mock.Of<IGraphNode<int>>(n => n.Payload == 1);
        var child2 = Mock.Of<IGraphNode<int>>(n => n.Payload == 2);

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
            layout => layout.Node.Payload == 0 && layout.Position.X == 0,
            layout => (layout.Node.Payload == 1 || layout.Node.Payload == 2) && layout.Position.X == 1,
            layout => (layout.Node.Payload == 1 || layout.Node.Payload == 2) && layout.Position.X == 1);
    }

    /// <summary>
    /// Test checks a graph as sequence of nodes visualized like line.
    /// </summary>
    [Test]
    public void Create_LinearGraph_ReturnsNodesInLine()
    {
        // ARRANGE

        const int LINE_COUNT = 3;

        var graph = CreateLineGraph(LINE_COUNT);

        var visualizer = new HorizontalGraphVisualizer<object>();

        // ACT

        var layouts = visualizer.Create(graph, _layoutConfig);

        // ASSERT

        layouts.Should().HaveCount(LINE_COUNT);

        var layoutLine = layouts.OrderBy(x => x.Position.X).ToArray();
        for (var index = 0; index < layoutLine.Length; index++)
        {
            var layout = layoutLine[index];

            if (index <= 0)
            {
                continue;
            }

            var prevLayout = layoutLine[index - 1];

            layout.Position.X.Should().BeGreaterThan(prevLayout.Position.X);
        }
    }

    /// <summary>
    /// Test checks a graph with multiple roots and single child visualized horizontally.
    /// </summary>
    [Test]
    public void Create_MergeGraph_RootsInSameXAndChildMoved()
    {
        // ARRANGE

        var visualizer = new HorizontalGraphVisualizer<int>();

        var graphMock = new Mock<IGraph<int>>();

        var root1 = Mock.Of<IGraphNode<int>>(n => n.Payload == 0);
        var root2 = Mock.Of<IGraphNode<int>>(n => n.Payload == 1);
        var child = Mock.Of<IGraphNode<int>>(n => n.Payload == 2);

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
            layout => (layout.Node.Payload == 0 || layout.Node.Payload == 1) && layout.Position.X == 0,
            layout => (layout.Node.Payload == 0 || layout.Node.Payload == 1) && layout.Position.X == 0,
            layout => (layout.Node.Payload == 2) && layout.Position.X == 1);
    }

    /// <summary>
    /// Test checks a nodes have relations layout horizontally.
    /// </summary>
    [Test]
    public void Create_ParentAndChildNodes_PlacesChildNextToParent()
    {
        // ARRANGE

        var visualizer = new HorizontalGraphVisualizer<object>();

        var graphMock = new Mock<IGraph<object>>();

        var root = Mock.Of<IGraphNode<object>>();
        var child = Mock.Of<IGraphNode<object>>();

        graphMock.Setup(x => x.GetAllNodes()).Returns(new[] { root, child });
        graphMock.Setup(x => x.GetNext(It.Is<IGraphNode<object>>(n => n == root))).Returns(new[] { child });
        graphMock.Setup(x => x.GetNext(It.Is<IGraphNode<object>>(n => n == child)))
            .Returns(ArraySegment<IGraphNode<object>>.Empty);

        var graph = graphMock.Object;

        var visualizerConfig = Mock.Of<ILayoutConfig>(x => x.NodeSize == 1);

        // ACT

        var layouts = visualizer.Create(graph, visualizerConfig);

        // ASSERT

        var parentLayout = layouts.Single(x => x.Node == root);
        var childLayout = layouts.Single(x => x.Node == child);

        childLayout.Position.X.Should().BeGreaterThan(parentLayout.Position.X);
    }

    /// <summary>
    /// Test checks a graph with single node gets single layout.
    /// </summary>
    [Test]
    public void Create_SingleNode_PlacesToZeroPosition()
    {
        // ARRANGE

        var visualizer = new HorizontalGraphVisualizer<object>();

        var graphMock = new Mock<IGraph<object>>();
        graphMock.Setup(x => x.GetAllNodes()).Returns(new[] { Mock.Of<IGraphNode<object>>() });
        graphMock.Setup(x => x.GetNext(It.IsAny<IGraphNode<object>>())).Returns(ArraySegment<IGraphNode<object>>.Empty);

        var graph = graphMock.Object;

        // ACT

        var layouts = visualizer.Create(graph, _layoutConfig);

        // ASSERT

        layouts.Should().HaveCount(1)
            .And.Subject.Single().Position.Should().Be(new Position(0, 0));
    }

    /// <summary>
    /// Test checks a graph with 2 isolated nodes gets layouts with same X-coordinate and Y-coordinates are different.
    /// </summary>
    [Test]
    public void Create_TwoRoots_PlacesVertically()
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

        var expectedPositions = new Position[]
        {
            new(0, 0),
            new(0, 1)
        };

        // ACT

        var layouts = visualizer.Create(graph, _layoutConfig);

        // ASSERT

        var positions = layouts.Select(x => x.Position).ToArray();
        positions.Should().BeEquivalentTo(expectedPositions);
    }

    private static Graph<object> CreateLineGraph(int lineCount)
    {
        var graph = new Graph<object>();

        GraphNode<object>? prevNode = null;

        for (var i = 0; i < lineCount; i++)
        {
            var graphNode = new GraphNode<object>(i);
            graph.AddNode(graphNode);
            if (prevNode is not null)
            {
                graph.ConnectNodes(prevNode, graphNode);
            }

            prevNode = graphNode;
        }

        return graph;
    }
}