using Moq;

namespace CombatDicesTeam.Graphs.Tests;

public class DirectedGraphTests
{
    /// <summary>
    /// Test checks a node from the middle of linear directed graph returns related node from the end as next.
    /// </summary>
    [Test]
    public void GetNext_LinearGraph3_ReturnsSingleNodeFromRoot()
    {
        // ARRANGE

        var graph = new DirectedGraph<int>();

        var nodeList = new List<IGraphNode<int>>();
        const int SEQUENCE_LENGTH = 3;
        for (var i = 0; i < SEQUENCE_LENGTH; i++)
        {
            var nodePayload = i;
            var graphNode = Mock.Of<IGraphNode<int>>(x => x.Payload == nodePayload);
            nodeList.Add(graphNode);
        }

        CreateLinearGraph(graph, nodeList);

        // ACT

        var next = graph.GetNext(nodeList[1]);

        // ASSERT

        next.Should().HaveCount(1).And.Subject.Should().Satisfy(x => x == nodeList[2]);
    }

    /// <summary>
    /// Test checks a root returns related node as next.
    /// </summary>
    [Test]
    public void GetNext_SimplestGraph_ReturnsSingleOtherNode()
    {
        // ARRANGE

        var graph = new DirectedGraph<int>();

        var graphNodeRoot = Mock.Of<IGraphNode<int>>(x => x.Payload == 0);
        graph.AddNode(graphNodeRoot);
        var graphNodeNext = Mock.Of<IGraphNode<int>>(x => x.Payload == 1);
        graph.AddNode(graphNodeNext);

        graph.ConnectNodes(graphNodeRoot, graphNodeNext);

        // ACT

        var rootNext = graph.GetNext(graphNodeRoot);

        // ASSERT

        rootNext.Should().HaveCount(1).And.Subject.Should().Satisfy(x => x == graphNodeNext);
    }

    private static void CreateLinearGraph<TNodePayload>(DirectedGraph<TNodePayload> targetGraph,
        IReadOnlyList<IGraphNode<TNodePayload>> nodeSequence)
    {
        IGraphNode<TNodePayload>? prevNode = null;

        foreach (var node in nodeSequence)
        {
            targetGraph.AddNode(node);
            if (prevNode is not null)
            {
                targetGraph.ConnectNodes(prevNode, node);
            }

            prevNode = node;
        }
    }
}