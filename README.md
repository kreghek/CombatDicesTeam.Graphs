# CombatDicesTeam.Graphs

The simplest library to work with graph.

## Usage

```c#
using CombatDicesTeam.Graphs;

// 1. Create a instance of the graph using one of implementations. You can create base graph from out-of-box.

var graph = new Graph<int>(); // the graph of integers.

// 2. Create multiple nodes.

var startNode = new GraphNode<int>(1);
graph.AddNode(startNode);

var endNode = new GraphNode<int>(2);
graph.AddNode(endNode);

// 3. Connect nodes.

graph.ConnectNodes(startNode, endNode);

```