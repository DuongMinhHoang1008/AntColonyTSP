using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private List<Edge> edges;
    public Node() {
        edges = new List<Edge>();
    }
    public void AddEdge(Edge e) {
        edges.Add(e);
    }
    public Edge GetEdge(int index) {
        return edges[index];
    }
    public List<Edge> GetEdgesList() {
        return edges;
    }

    public bool CheckHasEndNode(Node node) {
        foreach (Edge e in edges) {
            if (e.GetEndNode() == node) return true;
        }
        return false;
    }
}
