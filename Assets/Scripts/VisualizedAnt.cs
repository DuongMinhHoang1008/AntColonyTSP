using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizedAnt : MonoBehaviour
{
    float speed = 5;
    Ant ant;
    bool IsMoving = false;
    Rigidbody2D rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        CalculateNextNode();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMoving) {
            float distanceToDest = (ant.currentNode.position - rigidbody2D.position).magnitude;
            if (distanceToDest < 0.1f) {
                IsMoving = false;
                float distance = ant.FinishMove();
                
                if (distance > 0) {
                    VisualizedGraph.instance.AntFinish(distance, ant.way, ant);
                    return;
                }

                if (ant.MoveToNextNode() != null) {
                    Invoke("CalculateNextNode", 0.1f);
                } else {
                    VisualizedGraph.instance.SpawnNewAnt();
                    Destroy(gameObject);
                }
            }
        }
    }
    private void FixedUpdate() {
        if (IsMoving && ant != null) {
            Vector2 direction = (ant.currentNode.position - (Vector2) transform.position).normalized;
            rigidbody2D.MovePosition(rigidbody2D.position + direction * speed * Time.deltaTime);
        }
    }
    public void SpawnAnt(Ant a) {
        ant = a;
        transform.position = ant.startNode.position;
    }
    void CalculateNextNode() {
        if (!IsMoving) {
            IsMoving = true;
        }
    }
}
