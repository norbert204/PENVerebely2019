using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    [SerializeField] GameObject[] sprites;
    [SerializeField] float speed = 3;

    void Start() {
        sprites[0].SetActive(false);
        sprites[Random.Range(0, sprites.Length)].SetActive(true);
    }

	void Update () {
        gameObject.transform.position += transform.right * speed * Time.deltaTime;
        if (gameObject.transform.position.x < -11 || gameObject.transform.position.x > 11)
            Destroy(gameObject);
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        GameManager.Die("Elütött egy autó!");
    }
}
