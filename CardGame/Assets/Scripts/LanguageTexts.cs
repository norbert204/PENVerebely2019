using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageText {

    public static string[] back = { "Back", "Vissza" };

    public static string[] language = { "Language", "Nyelv" };
    public static string[] newGame = { "New Game", "Új játék" };
    public static string[] tutorial = { "Tutorial", "Tutoriál" };

    public static string[] fire = { "Fire", "Tűz" };
    public static string[] forest = { "Forest", "Erdő" };
    public static string[] water = { "Water", "Víz" };

    public static string[] english = { "English", "Angol" };
    public static string[] hungarian = { "Hungarian", "Magyar" };

    public static string[] endTurn = { "End Turn", "Kör Vége" };
    public static string[] victory = { "Victory", "Győzelem" };
    public static string[] defeat = { "Defeat", "Vereség" };
    public static string[] backToMainMenu = { "Back to Main Menu", "Vissza a főmenübe" };

    public static string[] yourTurn = { "Your Turn", "Ide kell vmi.." };

    public static int GetLangID()
    {
        return PlayerPrefs.GetInt("Language");
    }
}
