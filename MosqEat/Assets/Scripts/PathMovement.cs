using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMovement : MonoBehaviour {
    [SerializeField] Transform[] nodes;
    [SerializeField] float speed = .5f;
    [SerializeField] Transform sprite;
    Vector3 startPosition;
    int currentNode = 0;
    float pathTimer = 0;
	
    // Use this for initialization
	void Start () {
        gameObject.transform.position = nodes[0].position;
        CheckNode();
	}
	
    void CheckNode()
    {
        pathTimer = 0;
        startPosition = transform.position;
        if (transform.position.x < nodes[currentNode].position.x)
            sprite.rotation = Quaternion.Euler(0, 0, 0);
        else
            sprite.rotation = Quaternion.Euler(0, 180, 0);
    }

	// Update is called once per frame
	void Update () {
        pathTimer += Time.deltaTime * speed;
        if (transform.position != nodes[currentNode].position)
        {
            transform.position = Vector3.Lerp(startPosition, nodes[currentNode].position, pathTimer);
        }
        else
        {
            if (currentNode < nodes.Length - 1)
                currentNode++;
            else
                currentNode = 0;
            CheckNode();
        }
	}
}
