using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [SerializeField] DeckToUse deck;
    [SerializeField] GameObject[] menus;
    [SerializeField] Button[] backButtons;
    [Header("Backgrounds")]
    [SerializeField] GameObject mainMenuBG;
    [SerializeField] GameObject subMenuBG;
    [Header("Main menu")]
    [SerializeField] Button newGameButton;
    [SerializeField] Button tutorialButton;
    [SerializeField] Button languageButton;
    [Header("New Game")]
    [SerializeField] Button fire;
    [SerializeField] Button forest;
    [SerializeField] Button water;
    [Header("Language")]
    [SerializeField] Button english;
    [SerializeField] Button hungarian;

    void Start() {
        LoadText();
        foreach (var b in backButtons)
        {
            b.onClick.AddListener(() =>
            {
                SwitchMenu("mainmenu");
                MainMenuBG();
            });
        }

        
        newGameButton.onClick.AddListener(() =>
        {
            SwitchMenu("newgame");
            SubMenuBG();
        });
        tutorialButton.onClick.AddListener(() =>
        {
            SwitchMenu("tutorial");
            SubMenuBG();
        });
        languageButton.onClick.AddListener(() =>
        {
            SwitchMenu("language");
            SubMenuBG();
        });


        fire.onClick.AddListener(() =>
        {
            deck.deckToUse = deck.deckFire;
            StartGame();
        });
        forest.onClick.AddListener(() =>
        {
            deck.deckToUse = deck.deckForest;
            StartGame();
        });
        water.onClick.AddListener(() =>
        {
            deck.deckToUse = deck.deckWater;
            StartGame();
        });


        english.onClick.AddListener(() =>
        {
            SwitchLanguage(0);
        });
        hungarian.onClick.AddListener(() =>
        {
            SwitchLanguage(1);
        });
    }

    void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    void SwitchLanguage(int id)
    {
        PlayerPrefs.SetInt("Language", id);
        LoadText();
        SwitchMenu("mainmenu");
        MainMenuBG();
    }

    void LoadText()
    {
        foreach (var b in backButtons)
        {
            b.GetComponentInChildren<Text>().text = LanguageText.back[LanguageText.GetLangID()];
        }

        newGameButton.GetComponentInChildren<Text>().text = LanguageText.newGame[LanguageText.GetLangID()];
        tutorialButton.GetComponentInChildren<Text>().text = LanguageText.tutorial[LanguageText.GetLangID()];
        languageButton.GetComponentInChildren<Text>().text = LanguageText.language[LanguageText.GetLangID()];

        fire.GetComponentInChildren<Text>().text = LanguageText.fire[LanguageText.GetLangID()];
        forest.GetComponentInChildren<Text>().text = LanguageText.forest[LanguageText.GetLangID()];
        water.GetComponentInChildren<Text>().text = LanguageText.water[LanguageText.GetLangID()];

        english.GetComponentInChildren<Text>().text = LanguageText.english[LanguageText.GetLangID()];
        hungarian.GetComponentInChildren<Text>().text = LanguageText.hungarian[LanguageText.GetLangID()];
    }

    void SwitchMenu(string name)
    {
        foreach (var g in menus)
        {
            if (g.name.ToLower() == name.ToLower()) g.SetActive(true);
            else g.SetActive(false);
        }
    }

    void SubMenuBG()
    {
        mainMenuBG.SetActive(false);
        subMenuBG.SetActive(true);
    }

    void MainMenuBG()
    {
        mainMenuBG.SetActive(true);
        subMenuBG.SetActive(false);
    }
}
