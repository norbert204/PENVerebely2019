using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    [SerializeField] Text record;
    [SerializeField] Button play;
    [SerializeField] Button tutorial;
    [SerializeField] GameObject tutorialParent;
    [SerializeField] GameObject[] tutorialPages;
    int curPage = 0;
    [Header("Tutorial")]
    [SerializeField] Button back;
    [SerializeField] Button next;

    void Start() {
        record.text += string.Format("{0:0}s",PlayerPrefs.GetFloat("maxtime"));
        play.onClick.AddListener(() => { SceneManager.LoadScene(1, LoadSceneMode.Single); });
        tutorial.onClick.AddListener(() =>
        {
            tutorialParent.SetActive(true);
            next.interactable = true;
            curPage = 0;
            SwitchPage(0);
        });
        next.onClick.AddListener(() =>
        {
            if (curPage < tutorialPages.Length - 1)
            {
                curPage++;
                SwitchPage(curPage);
            }
        });
        back.onClick.AddListener(() =>
        {
            tutorialParent.SetActive(false);
        });
    }

    void SwitchPage(int page) {
        for (int i = 0; i < tutorialPages.Length; i++) {
            tutorialPages[i].SetActive(i == page);
        }
        if (page == tutorialPages.Length - 1)
            next.interactable = false;
    }
}
