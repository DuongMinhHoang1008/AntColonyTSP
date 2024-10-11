using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Ant
{
    //Node bắt đầu
    public Node startNode { get; private set; }
    //Node hiện tại
    public Node currentNode { get; private set; }
    //Tham số đánh giá điểm đến
    public int destinationRate { get; private set; }
    //Tham số đánh giá pheromone
    public int pheromoneRate { get; private set; }
    //Những node đã đi qua
    public HashSet<Node> passedNode { get; private set; }
    //Độ bay hơi của pheromone
    public float pheromoneEvaporation { get; private set; }
    //Đếm số node đi qua
    public int nodePassed { get; private set; }
    //Số node của đồ thị
    public int graphNodes { get; private set; }
    //Quãng đường đi được
    public float distance  { get; private set; }
    //Đường đi của kiến
    public List<Node> way { get; private set; }
    //Đường đi nhưng theo cạnh
    public List<Edge> edgePassed { get; private set; }
    Random random = new Random();
    public Ant(Node start, int graphNodesNum, int destinationRating = 5, int pheromoneRating = 2, float pheromoneEvaporationRate = 0.2f) {
        startNode = start;
        currentNode = startNode;
        destinationRate = destinationRating;
        pheromoneRate = pheromoneRating;
        passedNode = new HashSet<Node>{
            startNode
        };
        pheromoneEvaporation = pheromoneEvaporationRate;
        nodePassed = 0;
        graphNodes = graphNodesNum;
        distance = 0;
        way = new List<Node>();
        edgePassed = new List<Edge>();
    }

    //Tính toán cạnh tiếp theo mà kiến cần đi qua
    Edge CalculateNextEdge() {
        List<Edge> edges = currentNode.edges;
        List<float> edgesRate = new List<float>();
        float totalValue = 0;
        foreach (Edge e in edges) {
            float value = Mathf.Pow(e.pheromone, pheromoneRate) * Mathf.Pow(100 / e.length, destinationRate);
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
                    !passedNode.Contains(edges[i].end) && randomRate > floor && randomRate <= edgesRate[i] 
                ) {
                    return edges[i];
                }
            }
        }
        return null;
    }

    //Tính toán lượng pheromone thay đổi trên cạnh edge vừa đi qua
    float CalculatePheromone(Edge edge) {
        float max = 1 / (pheromoneEvaporation * distance);
        float min = max / 10;

        float pheromoneDelta = Math.Min(max, Math.Max(min, edge.pheromone));
        //Debug.Log(max + " " + min + " " + edge.pheromone);
        // if (edge.antPassed > 500) {
        //     edge.ResetAntPassed();
        //     return (float) random.NextDouble() * (max - min) + min;
        // }

        return (1 - pheromoneEvaporation) * edge.pheromone + pheromoneDelta;
    }

    //Di chuyển tới Node cuối của cạnh tiếp theo
    public Node MoveToNextNode() {
        Edge nextEdge = CalculateNextEdge();
        if (nextEdge != null) {
            way.Add(currentNode);
            currentNode = nextEdge.end;
            //nextEdge.SetPheromone(CalculatePheromone(nextEdge));
            nextEdge.Passed();
            nodePassed++;
            distance += nextEdge.length;
            passedNode.Add(nextEdge.end);
            edgePassed.Add(nextEdge);
            return currentNode;
        }
        return null;
    }

    //Xác định xem đã đi hết chưa nếu rồi thì trả về distance, chưa thì trả về 0
    public float FinishMove() {
        if (currentNode == startNode && passedNode.Count == graphNodes) {
            return distance;
        } else {
            return 0;
        }
    }

    //Tính toán lượng pheromone của con kiến có đường đi tốt nhất
    public void BestAntPheromone() {
        foreach (Edge edge in edgePassed) {
            edge.SetPheromone(CalculatePheromone(edge));
        }
    }

    //Kiến di chuyển
    public float Move() {
        if (FinishMove() > 0) {
            return FinishMove();
        }
        Node node = MoveToNextNode();
        if (node != null) {
            return Move();
        } else {
            return 0;
        }
    }
}
