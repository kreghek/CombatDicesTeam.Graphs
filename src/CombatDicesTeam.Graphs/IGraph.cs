namespace CombatDicesTeam.Graphs;

public interface IGraph<TValueData>
{
    IReadOnlyCollection<IGraphNode<TValueData>> GetAllNodes();
    IReadOnlyCollection<IGraphNode<TValueData>> GetNext(IGraphNode<TValueData> node);
    void AddNode(IGraphNode<TValueData> node);
    void ConnectNodes(IGraphNode<TValueData> sourceNode, IGraphNode<TValueData> targetNode);
}