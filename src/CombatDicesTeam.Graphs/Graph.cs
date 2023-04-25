namespace CombatDicesTeam.Graphs;

public sealed class Graph<TValueData>: IGraph<TValueData>
{
    private readonly IDictionary<IGraphNode<TValueData>, IList<IGraphNode<TValueData>>> _dict;

    public Graph()
    {
        _dict = new Dictionary<IGraphNode<TValueData>, IList<IGraphNode<TValueData>>>();
    }

    public void AddNode(IGraphNode<TValueData> node)
    {
        var next = new List<IGraphNode<TValueData>>();
        _dict[node] = next;
    }

    public void ConnectNodes(IGraphNode<TValueData> sourceNode, IGraphNode<TValueData> targetNode)
    {
        if (!_dict.TryGetValue(sourceNode, out var next))
        {
            next = new List<IGraphNode<TValueData>>();
            _dict[sourceNode] = next;
        }
        
        next.Add(targetNode);
    }

    public IReadOnlyCollection<IGraphNode<TValueData>> GetAllNodes()
    {
        return _dict.Keys.ToArray();
    }

    public IReadOnlyCollection<IGraphNode<TValueData>> GetNext(IGraphNode<TValueData> node)
    {
        return _dict[node].ToArray();
    }
}