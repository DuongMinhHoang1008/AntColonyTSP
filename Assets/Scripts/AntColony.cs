using System.Collections;
using System.Collections.Generic;
using System;

public class AntColony
{
    public float minDistance { get; private set; } = 0;
    Graph graph;
    Random random = new Random();
    public AntColony(Graph graph) {
        this.graph = graph;
    }
    public Ant SpawnAnt() {
        List<Node> nodes = graph.nodes;
        Node randStart = nodes[random.Next(0, nodes.Count)];
        Ant ant = new Ant(randStart, nodes.Count);
        return ant;
    }

    public void MoveAnt(Ant ant) {
        if (ant == null) {
            float distance = ant.FinishMove();
            if (distance > 0) {
                if (distance < minDistance) {
                    minDistance = distance;
                }
            } else {
                ant.MoveToNextNode();
            }
        }
    }
}
