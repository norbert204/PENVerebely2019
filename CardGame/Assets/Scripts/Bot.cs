using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour {



	public IEnumerator Turn()
    {
        yield return new WaitForSeconds(2);
        //
        //      Kártyát kirakni
        //
        while (CanPut())
        {
            var g = GetFirstPlacableCard();
            CardManager.AddOrBackToField(g, true);
            GameManager.DereaseMana(g.GetComponent<Card>().manaCost);
            yield return new WaitForSeconds(.5f);
        }

        //
        //      Tud-e ütni
        //
        if (!CanGoForFace())
        {
            var cardForAttack = CardForAttack();
            Card cardToAttack = null;
            int i = 0;
            while (cardToAttack == null && i < CardManager.instance.field.childCount)
            {
                var t = CardManager.instance.field.GetChild(i);
                var card = t.GetComponent<Card>();
                if (card.sleepState == 1)
                {
                    if (card.strength <= cardForAttack.strength)
                        cardToAttack = t.GetComponent<Card>();
                }
                i++;
            }
            if (cardForAttack != null && cardToAttack != null)
            {
                CardManager.Fight(cardForAttack, cardToAttack);
                yield return new WaitForSeconds(2f);
            }
        }

        //
        //      Megnézni hogy lehet-e menni arcra
        //
        if (CanGoForFace())
        {
            var forFace = CardForAttack();

            if (forFace != null)
                GameManager.HitFace(forFace.gameObject, true);
        }
        if (!GameManager.turn)
            GameManager.EndTurn();
    }

    Card CardForAttack()
    {
        Card card = null;
        int i = 0;

        while (card == null && i < CardManager.instance.enemyField.childCount)
        {
            var t = CardManager.instance.enemyField.GetChild(i);
            if (t.GetComponent<Card>().sleepState == 1) card = t.GetComponent<Card>();
            i++;
        }
        return card;
    }

    bool CanGoForFace()
    {
        if (CardManager.instance.field.childCount == 0) return true;

        foreach (var t in CardManager.instance.field)
        {
            if ((t as Transform).GetComponent<Card>().sleepState == 1) return false;
        }
        return true;
    }

    bool CanPut()
    {
        if (GameManager.mana == 0) return false;

        foreach (var t in CardManager.instance.enemyHand)
        {
            var c = (t as Transform).GetComponent<Card>();
            if (c.manaCost <= GameManager.mana)
                return true;
        }
        return false;
    }

    GameObject GetFirstPlacableCard()
    {
        foreach (var t in CardManager.instance.enemyHand)
        {
            var c = (t as Transform).GetComponent<Card>();
            if (c.manaCost <= GameManager.mana)
                return c.gameObject;
        }
        return null;
    }
}
