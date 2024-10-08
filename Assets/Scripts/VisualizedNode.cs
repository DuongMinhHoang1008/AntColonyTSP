using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizedNode : MonoBehaviour
{
    public Node node { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetNode(Node n) {
        node = n;
        transform.position = node.position;
    }
}
