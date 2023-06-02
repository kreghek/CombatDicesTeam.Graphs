# CombatDicesTeam.Graphs

The simplest library to work with graph.

## Usage

```c#
using CombatDicesTeam.Graphs;

// 1. Create a instance of the graph using one of implementations.
// You can create directed graph from out-of-box.

var graph = new DirectedGraph<int>(); // the graph of integers.

// 2. Create multiple nodes.

var startNode = new GraphNode<int>(1);
graph.AddNode(startNode);

var endNode = new GraphNode<int>(2);
graph.AddNode(endNode);

// 3. Connect nodes.

graph.ConnectNodes(startNode, endNode);

```

## Motivation

The library was made for the indie game devs, so as not to pull monstrous enterprise solutions for working with graphs into small pet-games.

## Authors and acknowledgment

*  [KregHEk](https://github.com/kreghek)

## Contributing

Feel free to contribute into the project.

## License

You can use it in your free open-source and commercial projects with a link to this repository.
