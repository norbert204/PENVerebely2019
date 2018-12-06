using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    static GameManager instance;
    float survived = 0;
    public static bool dead = false;

    [SerializeField] GameObject[] UIs;
    [Header("HUD")]
    [SerializeField] Text survivedText;
    [Header("Game Over")]
    [SerializeField] Text reasonText;

	// Use this for initialization
	void Start () {
        instance = this;
	}

    void Update()
    {
        if (!dead)
        {
            survived += Time.deltaTime;
            survivedText.text = string.Format("Túlélési idő: {0:0}", survived);
        }
    }

    public static void Die(string message) {
        dead = true;
        instance.SwicthUI("gameover");
        instance.reasonText.text = message;

        if (PlayerPrefs.GetFloat("maxtime") < instance.survived) {
            PlayerPrefs.SetFloat("maxtime", instance.survived);
        }
    }

    void SwicthUI(string uiName) {
        foreach (var g in UIs) {
            g.SetActive(g.name.ToLower() == uiName.ToLower());
        }
    }
}
