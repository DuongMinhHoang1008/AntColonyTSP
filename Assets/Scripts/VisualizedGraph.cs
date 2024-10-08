using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Quaternion = UnityEngine.Quaternion;

public class VisualizedGraph : MonoBehaviour
{
    [SerializeField] GameObject NodePref;
    [SerializeField] GameObject AntPref;
    [SerializeField] int numNodes;
    [SerializeField] int numAnts;
    [SerializeField] bool twoWay;
    List<VisualizedNode> visualizedNodes;
    float minDistance;
    Graph graph;
    public static VisualizedGraph instance {get; private set;}
    private void Awake() {
        if (instance == null) {
            graph = CompleteGraphGenerator.GenerateCompleteGraph(numNodes, twoWay);
            visualizedNodes = new List<VisualizedNode>();
            minDistance = numNodes * 100;
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
            visualizedNodes.Add(node.GetComponent<VisualizedNode>());
        }
        for (int i = 0; i < numAnts; i++) {
            StartAnt();
        } 
        Time.timeScale = 10;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(VisualizedNode visualNode in visualizedNodes) {
            Node n = visualNode.node;
            foreach (Edge e in n.edges) {
                Vector2 direction = (e.end.position - n.position).normalized * e.length;
                Vector2 middle = (e.end.position + n.position) / 2;
                // DrawArrow.ForDebug(n.position, direction, color: Color.red, arrowHeadLength: 1f, arrowHeadAngle: 60);
                if (!twoWay) {
                    Debug.DrawLine(n.position, middle, Color.red);
                    Debug.DrawLine(middle, e.end.position, Color.blue);
                } else {
                    Debug.DrawLine(n.position, e.end.position, Color.blue);
                }
            }
        }
    }

    void StartAnt() {
        Node randStart = graph.nodes[Random.Range(0, graph.nodes.Count)];
        Ant ant = new Ant(randStart, graph.nodes.Count);
        if (AntPref != null && randStart != null) {
            GameObject antObj = Instantiate(AntPref, randStart.position, Quaternion.identity);
            antObj.GetComponent<VisualizedAnt>().SpawnAnt(ant);
        }
    }

    public void SpawnNewAnt() {
        Invoke("StartAnt", 0.5f);
    }

    public void UpdateNewRecord(float dis) {
        if (dis < minDistance) {
            Debug.Log("New Record: " + dis);
            minDistance = dis;
        }
    }
}
