using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Ant
{
    public Node startNode { get; private set; }
    public Node currentNode { get; private set; }
    public int destinationRate { get; private set; }
    public int pheromoneRate { get; private set; }
    public HashSet<Node> passedNode { get; private set; }
    public float pheromoneEvaporation { get; private set; }
    public float pheromoneEdgeValue { get; private set; }
    public int nodePassed { get; private set; }
    public int graphNodes { get; private set; }
    public float distance  { get; private set; }
    Random random = new Random();
    public Ant(Node start, int graphNodesNum, int destinationRating = 3, int pheromoneRating = 1, float pheromoneEvaporationRate = 0.25f, float pheromoneEdgeValueRate = 1) {
        startNode = start;
        currentNode = startNode;
        destinationRate = destinationRating;
        pheromoneRate = pheromoneRating;
        passedNode = new HashSet<Node>{
            startNode
        };
        pheromoneEvaporation = pheromoneEvaporationRate;
        pheromoneEdgeValue = pheromoneEdgeValueRate;
        nodePassed = 0;
        graphNodes = graphNodesNum;
        distance = 0;
    }

    Edge CalculateNextEdge() {
        List<Edge> edges = currentNode.edges;
        List<float> edgesRate = new List<float>();
        float totalValue = 0;
        foreach (Edge e in edges) {
            float value = Mathf.Pow(e.pheromone, pheromoneRate) * Mathf.Pow(1 / e.length, destinationRate);
            if (passedNode.Contains(e.end)) {
                value = 0;
            }
            edgesRate.Add(value + totalValue);
            totalValue += value;
            if (e.end == startNode && nodePassed == graphNodes - 1) {
                return e;
            }
            if (totalValue > 0) {
                continue;
            }
        }
        if (totalValue > 0) {
            float randomRate = (float)(random.NextDouble() * totalValue);
            for (int i = 0; i < edges.Count; i++) {
                float floor = 0;
                if (i != 0) {
                    floor = edgesRate[i - 1];
                }
                if (
                    randomRate > floor && randomRate <= edgesRate[i] && !passedNode.Contains(edges[i].end) 
                ) {
                    return edges[i];
                }
            }
        }
        return null;
    }

    float CalculatePheromone(Edge edge) {
        return (1 - pheromoneEvaporation) * edge.pheromone + edge.antPassed * (pheromoneEdgeValue / edge.length);
    }

    public Node MoveToNextNode() {
        Edge nextEdge = CalculateNextEdge();
        if (nextEdge != null) {
            currentNode = nextEdge.end;
            nextEdge.SetPheromone(CalculatePheromone(nextEdge));
            nodePassed++;
            distance += nextEdge.length;
            passedNode.Add(nextEdge.end);
            return currentNode;
        }
        return null;
    }

    public float FinishMove() {
        if (currentNode == startNode && passedNode.Count == graphNodes) {
            return distance;
        } else {
            return 0;
        }
    }
}
