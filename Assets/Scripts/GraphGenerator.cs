using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class CompleteGraphGenerator
{
    // Hàm sinh đồ thị hoàn chỉnh với n đỉnh (đồ thị đầy đủ, có trọng số ngẫu nhiên)
    static public Graph GenerateCompleteGraph(int numNodes, bool twoWay)
    {
        Random random = new Random();;
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

        if (!twoWay) {
            // Tạo 1 vòng nối giữa các đỉnh
            for (int i = 0; i < numNodes; i++)
            {
                if (i < numNodes - 1) {
                    graph.AddEdge(nodes[i], nodes[i + 1]);
                } else {
                    graph.AddEdge(nodes[i], nodes[0]);
                }
            }
        }

        // Tạo các cạnh giữa mỗi cặp đỉnh (tạo đồ thị hoàn chỉnh)
        for (int i = 0; i < numNodes; i++)
        {
            for (int j = i + 1; j < numNodes; j++)
            {
                if (!twoWay) {
                    //Kiểm tra xem i có phải là j không, i có nối đến j rồi không và ngược lại
                    if (
                        !nodes[i].CheckHasEndNode(nodes[j])
                        && !nodes[j].CheckHasEndNode(nodes[i])
                    ) {
                        int rand = random.Next(0, 3);
                        if (j == i + 1 || rand != 0) {
                            graph.AddEdge(nodes[i], nodes[j]);
                        } else {
                            graph.AddEdge(nodes[j], nodes[i]);
                        }
                    }
                } else {
                    graph.AddEdge(nodes[i], nodes[j]);
                    graph.AddEdge(nodes[j], nodes[i]);
                }
            }
        }

        return graph;
    }
}
