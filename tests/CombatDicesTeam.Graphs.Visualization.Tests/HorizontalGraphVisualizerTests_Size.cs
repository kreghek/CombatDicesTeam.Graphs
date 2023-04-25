namespace CombatDicesTeam.Graphs.Visualization.Tests;

public sealed class HorizontalGraphVisualizerTests_Size
{
    [Test]
    public void Create_MergeGraphAndSize2_ReturnsChildBetweenRoots()
    {
        // ARRANGE

        const int NODE_SIZE = 2;

        var layoutConfig = Mock.Of<ILayoutConfig>(x => x.NodeSize == NODE_SIZE);

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

        var visualizer = new HorizontalGraphVisualizer<int>();

        // ACT

        var layouts = visualizer.Create(graph, layoutConfig);

        // ASSERT

        const int MIDDLE = NODE_SIZE / 2;

        layouts.Should().Satisfy(
            layout => (layout.Node.Payload == 0 || layout.Node.Payload == 1) && layout.Position.X == 0,
            layout => (layout.Node.Payload == 0 || layout.Node.Payload == 1) && layout.Position.X == 0,
            layout => (layout.Node.Payload == 2) && layout.Position.X == NODE_SIZE && layout.Position.Y == MIDDLE);
    }
}
