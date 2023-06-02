using JetBrains.Annotations;

namespace CombatDicesTeam.Graphs;

/// <summary>
/// Base implementation of the graph.
/// </summary>
/// <typeparam name="TNodePayload">Type of node data.</typeparam>
[PublicAPI]
public sealed class DirectedGraph<TNodePayload> : IGraph<TNodePayload>
{
    private readonly IDictionary<IGraphNode<TNodePayload>, IList<IGraphNode<TNodePayload>>> _nodeRelations;

    public DirectedGraph()
    {
        _nodeRelations = new Dictionary<IGraphNode<TNodePayload>, IList<IGraphNode<TNodePayload>>>();
    }

    /// <inheritdoc />
    public void AddNode(IGraphNode<TNodePayload> node)
    {
        var next = new List<IGraphNode<TNodePayload>>();
        _nodeRelations[node] = next;
    }

    /// <inheritdoc />
    public void ConnectNodes(IGraphNode<TNodePayload> sourceNode, IGraphNode<TNodePayload> targetNode)
    {
        if (!_nodeRelations.TryGetValue(sourceNode, out var next))
        {
            next = new List<IGraphNode<TNodePayload>>();
            _nodeRelations[sourceNode] = next;
        }

        next.Add(targetNode);
    }

    /// <inheritdoc />
    public IReadOnlyCollection<IGraphNode<TNodePayload>> GetAllNodes()
    {
        return _nodeRelations.Keys.ToArray();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<IGraphNode<TNodePayload>> GetNext(IGraphNode<TNodePayload> node)
    {
        return _nodeRelations[node].ToArray();
    }
}