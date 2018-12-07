using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour {

    public bool hasVisibleSkin = false;
    [SerializeField] GameObject detectMark;
    [Header("Sprites")]
    [SerializeField] Transform spriteParent;
    [SerializeField] GameObject[] sprites;
    [SerializeField] GameObject preferedSprite;
    [Header("Awareness values")]
    public float awareness = 0;
    public float awareAfter = 7f;
    public bool aware = false;
    public float awMultiplier = 1;

    void Start()
    {
        awareAfter = Random.Range(6, 8);
        awMultiplier = Random.Range(1.5f, 3f);

        if (preferedSprite == null) {
            sprites[0].SetActive(false);
            sprites[Random.Range(0, sprites.Length)].SetActive(true);
        }
        else {
            sprites[0].SetActive(false);
            preferedSprite.SetActive(true);
        }
    }

    void Update () {
        if (aware) {
            awareness -= Time.deltaTime / 4;
            if (awareness <= 0)
                aware = false;
        }

        awareness = Mathf.Clamp(awareness, 0, 10);

        detectMark.SetActive(aware);
	}
    public void Drain() {
        awareness += Time.deltaTime * awMultiplier;
        if (awareness >= awareAfter)
            aware = true;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        var mosq = col.GetComponent<Mosquito>();
        if (mosq != null)  
        {
            if (mosq.humanToDrain == null)
                mosq.humanToDrain = this;    
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var mosq = col.GetComponent<Mosquito>();
        if (mosq != null)
        {
            if (mosq.humanToDrain == this)
                mosq.humanToDrain = null;
        }
    }
}
