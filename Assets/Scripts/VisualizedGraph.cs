using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Quaternion = UnityEngine.Quaternion;
using System;
using Random = UnityEngine.Random;
using TMPro;
using System.Diagnostics;

public class VisualizedGraph : MonoBehaviour
{
    [SerializeField] GameObject NodePref;
    [SerializeField] GameObject AntPref;
    [SerializeField] GameObject LinePref;
    [SerializeField] bool twoWay;

    [SerializeField] int speed;
    [SerializeField] int numNodes;
    [SerializeField] int numAnts;
    [SerializeField] int pheromoneRating;
    [SerializeField] int edgeRating;
    [SerializeField] float pheromoneEvaporation;
    [SerializeField] int repeatTime;

    [SerializeField] TextMeshProUGUI time;
    [SerializeField] TextMeshProUGUI distance;
    List<GameObject> visualizedNodes;
    List<Node> bestWay;
    List<GameObject> antList;
    List<GameObject> lineList;
    int countAntFinished = 0;
    Ant bestAntInList = null;
    List<Node> bestListWay;
    float minListDistance;
    float minDistance;
    Graph graph;
    bool visualize = true;
    float startTime = 0;
    public static VisualizedGraph instance {get; private set;}
    private void Awake() {
        if (instance == null) {
            graph = CompleteGraphGenerator.GenerateCompleteGraph(numNodes, twoWay);
            visualizedNodes = new List<GameObject>();
            antList = new List<GameObject>();
            minDistance = numNodes * 100;
            bestWay = new List<Node>();
            lineList = new List<GameObject>();
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (Node n in graph.nodes) {
            GameObject node = Instantiate(NodePref, n.position, Quaternion.identity);
            node.GetComponent<VisualizedNode>().SetNode(n);
            node.transform.SetParent(gameObject.transform);
            visualizedNodes.Add(node);
        }
        // for (int i = 0; i < numAnts; i++) {
        //     StartAnt();
        // } 
        //Time.timeScale = 10;
    }

    // Update is called once per frame
    void Update()
    {
        // foreach(GameObject visualNode in visualizedNodes) {
        //     Node n = visualNode.GetComponent<VisualizedNode>().node;
        //     foreach (Edge e in n.edges) {
        //         // Vector2 middle = (e.end.position + n.position) / 2;
        //         // if (!twoWay) {
        //         //     Debug.DrawLine(n.position, middle, Color.red);
        //         //     Debug.DrawLine(middle, e.end.position, Color.blue);
        //         // } else {
        //         //     Debug.DrawLine(n.position, e.end.position, Color.blue);
        //         // }
        //         // Debug.Log(e.pheromone);
        //         Debug.DrawLine(n.position, e.end.position, new Color(0, 0, 0, e.pheromone / 3));
        //         //DrawArrow.ForDebug(n.position, e.end.position - n.position, color: new Color(0, 0, 0, e.pheromone), type: ArrowType.Triple);
        //     }
        // }
        // for(int i = 0; i < bestWay.Count; i++) {
        //     int num = 1;
        //     if (i == bestWay.Count - 1) num = -i;
        //     Debug.DrawLine(bestWay[i].position, bestWay[i + num].position, Color.blue);
        // }
    }

    void StartAnt() {
        Node randStart = graph.nodes[Random.Range(0, graph.nodes.Count)];
        Ant ant = new Ant(randStart, graph.nodes.Count, edgeRating, pheromoneRating, pheromoneEvaporation);
        if (AntPref != null && randStart != null) {
            GameObject antObj = Instantiate(AntPref, randStart.position, Quaternion.identity);
            antObj.GetComponent<VisualizedAnt>().SpawnAnt(ant);
            antList.Add(antObj);
        }
    }

    public void SpawnNewAnt() {
        Invoke("StartAnt", 0.5f);
    }

    public void AntFinish(float dis, List<Node> way, Ant ant) {
        countAntFinished++;
        if (dis < minListDistance) {
            minListDistance = dis;
            bestAntInList = ant;
            bestListWay = way;
        }
        if (countAntFinished == antList.Count) {
            bestAntInList.BestAntPheromone();
            if (minListDistance < minDistance) {
                minDistance = minListDistance;
                bestWay = bestListWay;
                time.text = "Time: " + (int) ((Time.time - startTime) / Time.timeScale) + " s";
                distance.text = "Distance: " + Math.Round(minDistance, 3);
            }
            DrawLine();
            Invoke("VisulizeTSP", 0.5f);
        }
    }

    public void ChangeSpeed(string str) {
        speed = int.Parse(str);
        Time.timeScale = speed;
    }

    public void ChangeNumNodes(string str) {
        numNodes = int.Parse(str);
    }

    public void ChangeNumAnt(string str) {
        numAnts = int.Parse(str);
    }

    public void ChangePheromoneRating(string str) {
        pheromoneRating = int.Parse(str);
    }

    public void ChangeEdgeRating(string str) {
        edgeRating = int.Parse(str);
    }

    public void ChangePheromoneEvaporation(string str) {
        pheromoneEvaporation = float.Parse(str);
    }

    public void ChangeRepeatTime(string str) {
        repeatTime = int.Parse(str);
    }

    public void CreateGraph() {
        foreach(GameObject node in visualizedNodes) {
            Destroy(node);
        }

        graph = CompleteGraphGenerator.GenerateCompleteGraph(numNodes, twoWay);
        bestWay.Clear();
        visualizedNodes.Clear();

        minDistance = numNodes * 100000;
        minListDistance = minDistance;

        foreach (Node n in graph.nodes) {
            GameObject node = Instantiate(NodePref, n.position, Quaternion.identity);
            node.GetComponent<VisualizedNode>().SetNode(n);
            node.transform.SetParent(gameObject.transform);
            visualizedNodes.Add(node);
        }

        foreach(GameObject ant in antList) {
            Destroy(ant);
        }
        antList.Clear();

        countAntFinished = 0;
        minListDistance = minDistance;

        ClearLine();
    }

    public void StartTSP() {
        startTime = Time.time;
        if (visualize) {
            foreach(GameObject ant in antList) {
                Destroy(ant);
            }
            antList.Clear();
            for (int i = 0; i < numAnts; i++) {
                StartAnt();
            }
            countAntFinished = 0;
            minListDistance = minDistance;
        } else {
            StartUnVisualize();
        }
    }

    void VisulizeTSP() {
        if (visualize) {
            foreach(GameObject ant in antList) {
                Destroy(ant);
            }
            antList.Clear();
            for (int i = 0; i < numAnts; i++) {
                StartAnt();
            }
            countAntFinished = 0;
            minListDistance = minDistance;
        }
    }

    public void ChangeVisualize(bool b) {
        visualize = b;
        //Debug.Log(b);
    }

    public void StartUnVisualize() {
        if (!visualize) {
            Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            AntColony antColony = new AntColony(repeatTime, numAnts, pheromoneRating, edgeRating, pheromoneEvaporation);
            //CreateGraph();
            antColony.StartNewAntList(graph);
            minDistance = antColony.minDistance;
            bestWay = antColony.bestWay;
            watch.Stop();
            time.text = "Time: " + watch.ElapsedMilliseconds + " ms";
            distance.text = "Distance: " + Math.Round(minDistance, 3);
            DrawLine();
        }
    }

    void DrawLine() {
        ClearLine();
        for(int i = 0; i < bestWay.Count; i++) {
            int num = 1;
            if (i == bestWay.Count - 1) num = -i;
            //Debug.DrawLine(bestWay[i].position, bestWay[i + num].position, Color.blue);
            GameObject line = Instantiate(LinePref, bestWay[i].position, Quaternion.identity);
            LineRenderer l = line.GetComponent<LineRenderer>();
            l.SetPosition(0, bestWay[i].position);
            l.SetPosition(1, bestWay[i + num].position);
            l.startColor = Color.blue;
            l.endColor = Color.blue;
            lineList.Add(line);
        }
    }

    void ClearLine() {
        foreach(GameObject l in lineList) {
            Destroy(l);
        }
    }
}
