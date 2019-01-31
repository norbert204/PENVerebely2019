using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellResurrection : Card {

    public override void OnCast()
    {
        if (CardManager.instance.graveyard.childCount > 0)
        {
            var g = CardManager.instance.graveyard.GetChild(Random.Range(0, CardManager.instance.graveyard.childCount));
            var c = g.GetComponent<Card>();
            g.gameObject.SetActive(true);
            c.onField = false;
            c.enemy = false;
            CardManager.BackToHand(g.gameObject, c.enemy);
            CardManager.CardToGraveyard(this, this.enemy);
            CardManager.RearrangeHand();
        }
        else
        {
            CardManager.BackToHand(gameObject, enemy);
        }
    }
}
