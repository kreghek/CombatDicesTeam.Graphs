namespace CombatDicesTeam.Graphs.Visualization.Tests;

public sealed class HorizontalGraphVisualizerAlignmentTests
{
    /// <summary>
    /// Test checks the visualizer layouts child node opposite related parents.
    /// </summary>
    [Test]
    [TestCaseSource(
        typeof(HorizontalGraphVisualizerTestCases.HorizontalGraphVisualizerTestCases),
        nameof(HorizontalGraphVisualizerTestCases.HorizontalGraphVisualizerTestCases.NodePerms2))]
    public void Create_TowWays_ReturnsChildrenLayoutsOppositeParents(int[] indexes)
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

        var nodeList = GetNodeListByIndexes(new[] { root1, root2, child1, child2 }, indexes);

        graphMock.Setup(x => x.GetAllNodes()).Returns(nodeList);

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
        
        var root2Layout = layouts.Single(x => x.Node.Payload == ROOT2_PAYLOAD);
        var child2Layout = layouts.Single(x => x.Node.Payload == CHILD2_PAYLOAD);

        if (root1Layout.Position.Y < root2Layout.Position.Y)
        {
            child1Layout.Position.Y.Should().BeLessThan(child2Layout.Position.Y);
        }
        else
        {
            child1Layout.Position.Y.Should().BeGreaterThan(child2Layout.Position.Y);
        }
    }

    private static IReadOnlyCollection<IGraphNode<T>> GetNodeListByIndexes<T>(IReadOnlyList<IGraphNode<T>> graphNodes, IEnumerable<int> indexes)
    {
        return indexes.Select(index => graphNodes[index]).ToList();
    }

    /// <summary>
    /// Test checks the visualizer layouts child node opposite related parents.
    /// </summary>
    [Test]
    [TestCaseSource(
        typeof(HorizontalGraphVisualizerTestCases.HorizontalGraphVisualizerTestCases),
        nameof(HorizontalGraphVisualizerTestCases.HorizontalGraphVisualizerTestCases.NodePermsIsolated))]
    public void Create_TowWaysAndIsolatedRoot_ReturnsChildrenLayoutsOppositeParents(int[] indexes)
    {
        // ARRANGE

        var graphMock = new Mock<IGraph<int>>();

        const int ROOT1_PAYLOAD = 0;
        const int ROOT2_PAYLOAD = 1;
        const int ROOT_ISOLATED_PAYLOAD = 10;
        const int CHILD1_PAYLOAD = 2;
        const int CHILD2_PAYLOAD = 3;

        var root1 = Mock.Of<IGraphNode<int>>(n => n.Payload == ROOT1_PAYLOAD);
        var root2 = Mock.Of<IGraphNode<int>>(n => n.Payload == ROOT2_PAYLOAD);
        var rootIsolated = Mock.Of<IGraphNode<int>>(n => n.Payload == ROOT_ISOLATED_PAYLOAD);
        var child1 = Mock.Of<IGraphNode<int>>(n => n.Payload == CHILD1_PAYLOAD);
        var child2 = Mock.Of<IGraphNode<int>>(n => n.Payload == CHILD2_PAYLOAD);
        
        var nodeList = GetNodeListByIndexes(new[] { root1, root2, child1, child2, rootIsolated }, indexes);

        graphMock.Setup(x => x.GetAllNodes()).Returns(nodeList);

        graphMock.Setup(x => x.GetNext(It.Is<IGraphNode<int>>(n => n == root1)))
            .Returns(new[] { child1 });

        graphMock.Setup(x => x.GetNext(It.Is<IGraphNode<int>>(n => n == root2)))
            .Returns(new[] { child2 });
        
        graphMock.Setup(x => x.GetNext(It.Is<IGraphNode<int>>(n => n == rootIsolated)))
            .Returns(ArraySegment<IGraphNode<int>>.Empty);

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
        
        var root2Layout = layouts.Single(x => x.Node.Payload == ROOT2_PAYLOAD);
        var child2Layout = layouts.Single(x => x.Node.Payload == CHILD2_PAYLOAD);

        if (root1Layout.Position.Y < root2Layout.Position.Y)
        {
            child1Layout.Position.Y.Should().BeLessThan(child2Layout.Position.Y);
        }
        else
        {
            child1Layout.Position.Y.Should().BeGreaterThan(child2Layout.Position.Y);
        }
    }
    
    /// <summary>
    /// Test checks the visualizer layouts child node opposite related parents.
    /// </summary>
    [Test]
    [TestCaseSource(
        typeof(HorizontalGraphVisualizerTestCases.HorizontalGraphVisualizerTestCases),
        nameof(HorizontalGraphVisualizerTestCases.HorizontalGraphVisualizerTestCases.NodePermsIsolated))]
    public void Create_ZigZag_ReturnsChildrenLayoutsOppositeParents(int[] indexes)
    {
        // ARRANGE

        var graphMock = new Mock<IGraph<int>>();

        const int ROOT1_PAYLOAD = 0;
        const int ROOT2_PAYLOAD = 1;
        const int ROOT3_PAYLOAD = 2;
        const int CHILD1_PAYLOAD = 10;
        const int CHILD2_PAYLOAD = 20;

        var root1 = Mock.Of<IGraphNode<int>>(n => n.Payload == ROOT1_PAYLOAD);
        var root2 = Mock.Of<IGraphNode<int>>(n => n.Payload == ROOT2_PAYLOAD);
        var root3 = Mock.Of<IGraphNode<int>>(n => n.Payload == ROOT3_PAYLOAD);
        var child1 = Mock.Of<IGraphNode<int>>(n => n.Payload == CHILD1_PAYLOAD);
        var child2 = Mock.Of<IGraphNode<int>>(n => n.Payload == CHILD2_PAYLOAD);
        
        var nodeList = GetNodeListByIndexes(new[] { root1, root2, child1, child2, root3 }, indexes);

        graphMock.Setup(x => x.GetAllNodes()).Returns(nodeList);

        graphMock.Setup(x => x.GetNext(It.Is<IGraphNode<int>>(n => n == root1)))
            .Returns(new[] { child1 });

        graphMock.Setup(x => x.GetNext(It.Is<IGraphNode<int>>(n => n == root2)))
            .Returns(new[] { child2 });
        
        graphMock.Setup(x => x.GetNext(It.Is<IGraphNode<int>>(n => n == root3)))
            .Returns(new[] { child1, child2 });

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
        
        var root2Layout = layouts.Single(x => x.Node.Payload == ROOT2_PAYLOAD);
        var child2Layout = layouts.Single(x => x.Node.Payload == CHILD2_PAYLOAD);

        if (root1Layout.Position.Y < root2Layout.Position.Y)
        {
            child1Layout.Position.Y.Should().BeLessThan(child2Layout.Position.Y);
        }
        else
        {
            child1Layout.Position.Y.Should().BeGreaterThan(child2Layout.Position.Y);
        }
    }
}