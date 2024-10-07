using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    private Node start;
    private Node end;
    private float length = 0;
    public Edge(Node s, Node e, float l) {
        if (s == null) {
            s = new Node();
        }
        if (e == null) {
            e = new Node();
        }
        start = s;
        end = e;
        length = l;
        start.AddEdge(this);
    }
    public Node GetEndNode()
    {
        return end;
    }
    public Node GetStartNode()
    {
        return start;
    }
    public float GetLength()
    {
        return length;
    }
}
