using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class CompleteGraphGenerator
{
    private Random random;

    public CompleteGraphGenerator()
    {
        random = new Random();
    }

    // Hàm sinh đồ thị hoàn chỉnh với n đỉnh (đồ thị đầy đủ, có trọng số ngẫu nhiên)
    public Graph GenerateCompleteGraph(int numNodes)
    {
        Graph graph = new Graph();
        List<Node> nodes = new List<Node>();

        // Tạo các node
        for (int i = 0; i < numNodes; i++)
        {
            nodes.Add(new Node());
        }

        // Thêm tất cả các node vào đồ thị
        foreach (var node in nodes)
        {
            graph.AddNode(node);
        }

        // Tạo các cạnh giữa mỗi cặp đỉnh (tạo đồ thị hoàn chỉnh)
        for (int i = 0; i < numNodes; i++)
        {
            for (int j = 0; j < numNodes; j++)
            {
                //Kiểm tra xem i có phải là j không, i có nối đến j rồi không và ngược lại
                if (
                    nodes[i] != nodes[j]
                    && !nodes[i].CheckHasEndNode(nodes[j]) 
                    && !nodes[j].CheckHasEndNode(nodes[i])
                ) {
                    float randomLength = (float)(random.NextDouble() * 10 + 1); // Sinh trọng số ngẫu nhiên cho cạnh
                    graph.AddEdge(nodes[i], nodes[j], randomLength);
                }
            }
        }

        return graph;
    }
}
