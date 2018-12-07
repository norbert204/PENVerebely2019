using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    static GameManager instance;
    float survived = 0;
    public static bool dead = false;

    [SerializeField] GameObject joystick;
    [SerializeField] GameObject[] UIs;
    [SerializeField] Mosquito mosquito;
    [Header("HUD")]
    [SerializeField] Text survivedText;
    [SerializeField] Button restart;
    [SerializeField] Button menu;
    [Header("Game Over")]
    [SerializeField] Text reasonText;

	// Use this for initialization
    void Start () {
        dead = false;
        survived = 0;

        instance = this;
        restart.onClick.AddListener(() => {
            dead = false;
            instance.survived = 0;
            SceneManager.LoadScene(1, LoadSceneMode.Single); 
        });
        menu.onClick.AddListener(() => { SceneManager.LoadScene(0, LoadSceneMode.Single); });
	}

    void Update()
    {
        if (!dead)
        {
            survived += Time.deltaTime;
            survivedText.text = string.Format("Túlélési idő: {0:0}", survived);
        }

        if (survived >= 15 && survived < 35)
            mosquito.hungerMultiplier = 2;
        else if (survived >= 35 && survived < 50)
            mosquito.hungerMultiplier = 2.5f;
        else if (survived >= 50)
            mosquito.hungerMultiplier = 3;
    }

    public static void Die(string message) {
        dead = true;
        instance.joystick.SetActive(false);
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
