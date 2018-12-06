using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] Transform objectToFollow;
    [SerializeField] float minX, maxX, minY, maxY;
    [Header("Lock angles")]
    [SerializeField] bool lockCamX;
    [SerializeField] bool lockCamY;

	void Update () {
        gameObject.transform.position = new Vector3((lockCamX) ? 0 : (objectToFollow.position.x <= minX) ? minX : (objectToFollow.position.x >= maxX) ? maxX : objectToFollow.position.x, (lockCamY) ? 0 : (objectToFollow.position.y <= minY) ? minY : (objectToFollow.position.y >= maxY) ? maxY : objectToFollow.position.y, -10);	
	}
}
