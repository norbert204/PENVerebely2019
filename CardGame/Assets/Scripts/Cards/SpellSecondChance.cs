using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSecondChance : Card {

    public override void OnCast(Card card)
    {
        card.sleepState = 1;
        CardManager.CardToGraveyard(this, this.enemy);
        CardManager.RearrangeHand();
    }
}
