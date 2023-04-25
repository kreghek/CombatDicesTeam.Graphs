using JetBrains.Annotations;

namespace CombatDicesTeam.Graphs;

/// <summary>
/// The graph interface.
/// </summary>
/// <typeparam name="TNodePayload">Type of node data.</typeparam>
[PublicAPI]
public interface IGraph<TNodePayload>
{
    /// <summary>
    /// Gets list of all nodes in the graph. Include isolated nodes.
    /// </summary>
    /// <returns> Returns list of nodes. </returns>
    IReadOnlyCollection<IGraphNode<TNodePayload>> GetAllNodes();

    /// <summary>
    /// Gets connected nodes of specified node.
    /// </summary>
    /// <param name="node">A node for which connected node was requested.</param>
    /// <returns>List of connected nodes.</returns>
    IReadOnlyCollection<IGraphNode<TNodePayload>> GetNext(IGraphNode<TNodePayload> node);

    /// <summary>
    /// Add node to the graph.
    /// </summary>
    /// <param name="node">Node to add.</param>
    /// <remarks>
    /// Added node is not connected with other. Use <see cref="ConnectNodes"/> to make node relations. 
    /// </remarks>
    void AddNode(IGraphNode<TNodePayload> node);

    /// <summary>
    /// Connect to nodes in the graph.
    /// </summary>
    /// <remarks>
    /// Both nodes must be added in the graph early.
    /// </remarks>
    /// <param name="sourceNode">Node from which connection was created.</param>
    /// <param name="targetNode">Node to which connection was created.</param>
    void ConnectNodes(IGraphNode<TNodePayload> sourceNode, IGraphNode<TNodePayload> targetNode);
}