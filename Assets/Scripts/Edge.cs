using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Node start { get; private set; }
    public Node end { get; private set; }
    public float length { get; private set; }
    public int antPassed { get; private set; }
    public float pheromone { get; private set; }
    public Edge(Node s, Node e) {
        if (s == null) {
            s = new Node();
        }
        if (e == null) {
            e = new Node();
        }
        start = s;
        end = e;
        length = Vector2.Distance(s.position, e.position);
        start.AddEdge(this);
        pheromone = 1;
        antPassed = 0;
    }
    public void SetPheromone(float p) {
        pheromone = p;
    }
    public void Passed() {
        antPassed++;
    }
}
