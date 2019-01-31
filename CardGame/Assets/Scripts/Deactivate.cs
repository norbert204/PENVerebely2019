using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivate : MonoBehaviour {

    [SerializeField] float deactivateTime;
    float curTime;

	void Start () {
        curTime = deactivateTime;
	}
	
	void Update () {
        curTime -= Time.deltaTime;
        if (curTime <= 0) gameObject.SetActive(false);
	}
}
