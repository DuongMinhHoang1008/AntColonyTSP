using System;
using System.Collections;
using System.Collections.Generic;

public class Graph
{
    public List<Node> nodes { get; private set; }

    public Graph()
    {
        nodes = new List<Node>();
    }

    public void AddNode(Node node)
    {
        nodes.Add(node);
    }

    public void AddEdge(Node start, Node end)
    {
        Edge edge = new Edge(start, end);
    }
}
