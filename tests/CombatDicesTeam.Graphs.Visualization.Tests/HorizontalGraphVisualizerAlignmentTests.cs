namespace CombatDicesTeam.Graphs.Visualization.Tests;

public sealed class HorizontalGraphVisualizerAlignmentTests
{
    /// <summary>
    /// Test checks the visualizer layouts node between two related.
    /// </summary>
    [Test]
    public void Create_TowWays_ReturnsChildrenLayoutsOppositeParents()
    {
        // ARRANGE

        var graphMock = new Mock<IGraph<int>>();

        const int ROOT1_PAYLOAD = 0;
        const int ROOT2_PAYLOAD = 1;
        const int CHILD1_PAYLOAD = 2;
        const int CHILD2_PAYLOAD = 3;

        var root1 = Mock.Of<IGraphNode<int>>(n => n.Payload == ROOT1_PAYLOAD);
        var root2 = Mock.Of<IGraphNode<int>>(n => n.Payload == ROOT2_PAYLOAD);
        var child1 = Mock.Of<IGraphNode<int>>(n => n.Payload == CHILD1_PAYLOAD);
        var child2 = Mock.Of<IGraphNode<int>>(n => n.Payload == CHILD2_PAYLOAD);

        graphMock.Setup(x => x.GetAllNodes()).Returns(new[] { root1, root2, child1, child2 });

        graphMock.Setup(x => x.GetNext(It.Is<IGraphNode<int>>(n => n == root1)))
            .Returns(new[] { child1 });

        graphMock.Setup(x => x.GetNext(It.Is<IGraphNode<int>>(n => n == root2)))
            .Returns(new[] { child2 });

        graphMock.Setup(x => x.GetNext(It.Is<IGraphNode<int>>(n => n == child1 || n == child2)))
            .Returns(ArraySegment<IGraphNode<int>>.Empty);

        var graph = graphMock.Object;

        var visualizer = new HorizontalGraphVisualizer<int>();

        const int NODE_SIZE = 1;
        var layoutConfig = Mock.Of<ILayoutConfig>(x => x.NodeSize == NODE_SIZE);

        // ACT

        var layouts = visualizer.Create(graph, layoutConfig);

        // ASSERT

        var root1Layout = layouts.Single(x => x.Node.Payload == ROOT1_PAYLOAD);
        var child1Layout = layouts.Single(x => x.Node.Payload == CHILD1_PAYLOAD);

        child1Layout.Position.Y.Should().Be(root1Layout.Position.Y);
        
        var root2Layout = layouts.Single(x => x.Node.Payload == ROOT2_PAYLOAD);
        var child2Layout = layouts.Single(x => x.Node.Payload == CHILD2_PAYLOAD);

        child2Layout.Position.Y.Should().Be(root2Layout.Position.Y);
    }
}