using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    private List<Node> nodes;

    public Graph()
    {
        nodes = new List<Node>();
    }

    public void AddNode(Node node)
    {
        nodes.Add(node);
    }

    public void AddEdge(Node start, Node end, float length)
    {
        Edge edge = new Edge(start, end, length);
    }
}
