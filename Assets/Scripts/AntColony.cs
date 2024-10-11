using System.Collections;
using System.Collections.Generic;
using System;
using Debug = UnityEngine.Debug;

public class AntColony
{
    //Quãng đường ngắn nhất đi được
    public float minDistance { get; private set; } = 0;
    //Số lần lặp
    int repeatTime;
    //Số kiến mỗi lần lặp
    int numAnts;
    //Đường đi ngắn nhất
    public List<Node> bestWay { get; private set; } = new List<Node>();
    
    // List<Node> bestListWay = new List<Node>();
    //Con kiến tốt nhất trong lần lặp
    Ant bestAntInList = null;
    // int countAntFinished = 0;
    //Quãng đường ngắn nhất trong vòng lặp
    float minListDistance;
    //
    int pheromoneRating;
    int edgeRating;
    float pheromoneEvaporation;
    Graph graph;
    Random random = new Random();
    public AntColony(int repeat, int antNum, int pheromoneRate, int edgeRate, float pheromoneEva) {
        repeatTime = repeat;
        numAnts = antNum;
        pheromoneRating = pheromoneRate;
        edgeRating = edgeRate;
        pheromoneEvaporation = pheromoneEva;
    }

    //Sinh ra một con kiến trên Node thứ index
    public Ant SpawnAnt(int nodeIndex) {
        // List<Node> nodes = graph.nodes;
        // Node randStart = nodes[random.Next(0, nodes.Count)];
        Ant ant = new Ant(graph.nodes[nodeIndex], graph.nodes.Count, destinationRating: edgeRating, pheromoneRating: pheromoneRating, pheromoneEvaporationRate: pheromoneEvaporation);
        return ant;
    }

    //Di chuyển con kiến 
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

    //Bắt đầu đi
    public void StartNewAntList(Graph graph) {
        this.graph = graph;
        minDistance = graph.nodes.Count * 10000;
        //Tính số lần lặp không hiệu quả (không tìm ra đường mới)
        int notWorkCount = 0;
        for (int i = 0; i < repeatTime; i++) {
            //Bắt đầu vòng lặp thứ i
            minListDistance = minDistance;
            for (int j = 0; j < graph.nodes.Count; j++) {
                Ant ant = SpawnAnt(j);
                float distance = ant.Move();
                if (distance < minListDistance) {
                    minListDistance = distance;
                    bestAntInList = ant;
                }
            }
            //Con kiến tốt nhất tính số pheromone
            bestAntInList.BestAntPheromone();
            //Nếu con kiến tốt nhất đi đường mới -> giảm notWorkCount, nếu không thì tăng lên, đến một khoảng nhất định thì dừng
            if (minListDistance < minDistance) {
                bestWay = bestAntInList.way;
                minDistance = minListDistance;
                notWorkCount /= 2;
                if (notWorkCount < 0) notWorkCount = 0;
            } else {
                notWorkCount++;
                if (notWorkCount > repeatTime / 5) {
                    break;
                }
            }
        }
    }
}
