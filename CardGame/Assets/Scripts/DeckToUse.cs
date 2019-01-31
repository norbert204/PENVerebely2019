using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckToUse : MonoBehaviour {

    public List<GameObject> deckFire;
    public List<GameObject> deckForest;
    public List<GameObject> deckWater;

    public List<GameObject> deckToUse;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
