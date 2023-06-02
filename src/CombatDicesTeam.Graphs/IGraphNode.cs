using JetBrains.Annotations;

namespace CombatDicesTeam.Graphs;

/// <summary>
/// Graph node interface.
/// </summary>
/// <typeparam name="TNodePayload">Main data of the node</typeparam>
[PublicAPI]
public interface IGraphNode<out TNodePayload>
{
    /// <summary>
    /// Main data of the node.
    /// </summary>
    public TNodePayload Payload { get; }
}