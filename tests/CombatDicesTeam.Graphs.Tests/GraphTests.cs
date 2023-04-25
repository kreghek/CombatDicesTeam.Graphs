namespace CombatDicesTeam.Graphs.Tests;

public class GraphTests
{
    [Test]
    public void GetNext_SimplestGraph_ReturnsSingleOtherNode()
    {
        // ARRANGE

        var graph = new Graph<object>();

        var graphNodeRoot = new GraphNode<object>(0);
        graph.AddNode(graphNodeRoot);
        var graphNodeNext = new GraphNode<object>(1);
        graph.AddNode(graphNodeNext);
        
        graph.ConnectNodes(graphNodeRoot, graphNodeNext);
        
        // ACT

        var rootNext = graph.GetNext(graphNodeRoot);
        
        // ASSERT

        rootNext.Should().HaveCount(1).And.Subject.Should().Satisfy(x => x == graphNodeNext);
    }
    
    [Test]
    public void GetNext_LinearGraph9_ReturnsSingleNodeFromRoot()
    {
        // ARRANGE
        
        var graph = new Graph<object>();

        GraphNode<object>? prevNode = null;
        var nodeList = new List<GraphNode<object>>();
        for (var i = 0; i < 3; i++)
        {
            var graphNode = new GraphNode<object>(i);
            nodeList.Add(graphNode);
            graph.AddNode(graphNode);
            if (prevNode is not null)
            {
                graph.ConnectNodes(prevNode, graphNode);
            }

            prevNode = graphNode;
        }
        
        // ACT

        var next = graph.GetNext(nodeList[1]);
        
        // ASSERT

        next.Should().HaveCount(1).And.Subject.Should().Satisfy(x => x == nodeList[2]);
    }
}