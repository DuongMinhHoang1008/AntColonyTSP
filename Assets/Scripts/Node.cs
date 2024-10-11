using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Node
{
    public List<Edge> edges { get; private set; }
    public Vector2 position { get; private set; }
    public Node() {
        Random rand = new Random();
        edges = new List<Edge>();
        position = new Vector2(
            (float) rand.NextDouble() * 7 - 7,
            (float) rand.NextDouble() * 8 - 4
        );
    }

    public void AddEdge(Edge e) {
        edges.Add(e);
    }

    public Edge GetEdge(int index) {
        return edges[index];
    }

    public bool CheckHasEndNode(Node node) {
        foreach (Edge e in edges) {
            if (e.end == node) return true;
        }
        return false;
    }

    public Vector2 GetPosition() {
        return position;
    }
}
