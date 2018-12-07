using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour {

    [SerializeField] GameObject prefab;
    [SerializeField] bool left;
    float time = 0;

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0) {
            Instantiate(prefab, transform.position, Quaternion.Euler(0, 180 * ((left) ? 1 : 0), 0));

            time = Random.Range(3, 5);
        }
    }
}
