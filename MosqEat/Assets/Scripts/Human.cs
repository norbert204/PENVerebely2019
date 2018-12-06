using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour {

    public bool hasVisibleSkin = false;
    [SerializeField] GameObject detectMark;
    [Header("Awareness values")]
    public float awareness = 0;
    public float awareAfter = 7f;
    public bool aware = false;
    public float awMultiplier = 1;

	void Update () {
        if (aware) {
            awareness -= Time.deltaTime / 2;
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
