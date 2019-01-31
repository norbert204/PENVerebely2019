using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour {

    public static CardManager instance;

    public Transform hand;
    public Transform enemyHand;

    public Image preview;

    float currLerpTime;
    float spawnCurrLerpTime;
    float lerpTime = 1;
    bool lastTurnState;

    public Transform field;
    public Transform handStack;
    public Transform stack;

    public Transform enemyField;
    public Transform enemyHandStack;
    public Transform enemyStack;

    public Transform graveyard;
    public Transform enemyGraveyard;

    public Sprite cardBack;

    void Start()
    {
        instance = this;
        RearrangeHand();
        RearrangeField();
        lastTurnState = GameManager.turn;
    }

    void Update()
    {
        if (lastTurnState != GameManager.turn)
        {
            currLerpTime = 0;
            spawnCurrLerpTime = 0;
            lastTurnState = !lastTurnState;

            //
            //  Transform c in hand nem működik
            //
            foreach (var c in hand)
            {
                Transform t = (Transform)c;
                if (t.GetComponent<Card>().spawnAnimState < 2)
                    t.GetComponent<Card>().spawnAnimState++;
            }
        }

        currLerpTime += Time.deltaTime;
        if (currLerpTime > lerpTime) currLerpTime = lerpTime;

        spawnCurrLerpTime += Time.deltaTime / 4;
        if (spawnCurrLerpTime > lerpTime) spawnCurrLerpTime = lerpTime;


        if (!GameManager.turn)
        {

            //
            //      Player
            //

            float startPos = handStack.position.x/* - (.2f * hand.childCount)*/;
            for (int i = 0; i < hand.childCount; i++)
            {
                var card = hand.GetChild(i).GetComponent<Card>();
                var pos = new Vector3(startPos + .2f * i, handStack.position.y);
                if (card.sprite.transform.position != pos)
                {
                    if (!card.onField)
                    {
                        card.sprite.transform.position = Vector3.Lerp(card.sprite.transform.position, pos, (currLerpTime / lerpTime));
                    }
                }
            }

            //
            //      Enemy
            //

            for (int i = 0; i < enemyHand.childCount; i++)
            {
                var card = enemyHand.GetChild(i).GetComponent<Card>();
                if (card.sprite.transform.localPosition != Vector3.zero)
                {
                    if (!card.onField)
                    {
                        card.sprite.transform.localPosition = Vector3.Lerp(card.sprite.transform.localPosition, Vector3.zero, ((card.spawnAnim && card.spawnAnimState < 2) ? spawnCurrLerpTime : currLerpTime / lerpTime));
                    }
                }
            }
        }
        else
        {
            //
            //      Player
            //

            for (int i = 0; i < hand.childCount; i++)
            {
                var card = hand.GetChild(i).GetComponent<Card>();
                if (card.sprite.transform.localPosition != Vector3.zero)
                {
                    if (!card.onField)
                    {
                        card.sprite.transform.localPosition = Vector3.Lerp(card.sprite.transform.localPosition, Vector3.zero, ((card.spawnAnim && card.spawnAnimState < 2) ? spawnCurrLerpTime : currLerpTime / lerpTime));
                    }
                }
            }

            //
            //      Enemy
            //

            float startPos = enemyHandStack.position.x/* - (.2f * hand.childCount)*/;
            for (int i = 0; i < enemyHand.childCount; i++)
            {
                var card = enemyHand.GetChild(i).GetComponent<Card>();
                var pos = new Vector3(startPos + .2f * i, enemyHandStack.position.y);
                if (card.sprite.transform.position != pos)
                {
                    if (!card.onField)
                    {
                        card.sprite.transform.position = Vector3.Lerp(card.sprite.transform.position, pos, (currLerpTime / lerpTime));
                    }
                }
            }
        }
    }

    public static void AddOrBackToField(GameObject card, bool enemy)
    {
        var _parent = ((enemy) ? instance.enemyField : instance.field);

        card.transform.parent = _parent;
        card.GetComponent<Card>().onField = true;

        if (enemy)
        {
            var c = card.GetComponent<Card>();
            c.sprite.sprite = c.sprites[LanguageText.GetLangID()];
        }

        RearrangeField();
    }

    public static void BackToHand(GameObject card, bool enemy)
    {
        var _parent = ((enemy) ? instance.enemyHand : instance.hand);

        card.GetComponent<Card>().onField = false;
        card.transform.parent = _parent;
        RearrangeHand();
    }
    /*public static void BackToField(GameObject card, bool enemy)
    {
        var _parent = ((enemy) ? instance.enemyField : instance.field);

        card.transform.parent = instance.field;
        RearrangeField();
    }*/

    public static void RearrangeField()
    {
        //
        //      Player
        //

        float startX = -Mathf.Floor(instance.field.childCount / 2) * 1.5f;

        if (instance.field.childCount % 2 == 0) startX += 0.75f;

        for (int i = 0; i < instance.field.childCount; i++)
        {
            instance.field.GetChild(i).GetComponentInChildren<SpriteRenderer>().sortingOrder = -1;
            instance.field.GetChild(i).localPosition = new Vector3(startX + i * 1.5f, 0, 0);
        }

        //
        //      Enemy
        //

        startX = -Mathf.Floor(instance.enemyField.childCount / 2) * 1.5f;

        if (instance.enemyField.childCount % 2 == 0) startX += 0.75f;

        for (int i = 0; i < instance.enemyField.childCount; i++)
        {
            instance.enemyField.GetChild(i).localPosition = new Vector3(startX + i * 1.5f, 0, 0);
            instance.enemyField.GetChild(i).GetComponentInChildren<SpriteRenderer>().sortingOrder = -1;
        }

        RearrangeHand();
    }

    public static void RearrangeHand()
    {

        //
        //      Player
        //

        float startX = -(instance.hand.childCount / 2) * 2 + ((instance.hand.childCount % 2 == 0) ? 1 : 0);

        for (int i = 0; i < instance.hand.childCount; i++)
        {
            instance.hand.GetChild(i).localPosition = new Vector3(startX + i*2, 0, 0);
            instance.hand.GetChild(i).GetComponentInChildren<SpriteRenderer>().sortingOrder = i;
        }

        //
        //      Enemy
        //

        startX = -(instance.enemyHand.childCount / 2) * 2 + ((instance.enemyHand.childCount % 2 == 0) ? 1 : 0);

        for (int i = 0; i < instance.enemyHand.childCount; i++)
        {
            instance.enemyHand.GetChild(i).localPosition = new Vector3(startX + i * 2, 0, 0);
            instance.enemyHand.GetChild(i).GetComponentInChildren<SpriteRenderer>().sortingOrder = i;
        }
    }

    public static void EndCardView()
    {
        instance.preview.gameObject.SetActive(false);
    }
    public static void StartCardView(Sprite sprite, Vector3 pos)
    {
        instance.preview.gameObject.SetActive(true);
        instance.preview.sprite = sprite;
        var tmp = Camera.main.WorldToScreenPoint(pos);
        instance.preview.rectTransform.position = new Vector3(tmp.x - 205 * ((pos.x < 0) ? -1 : 1), tmp.y);
    }

    public static void UpdateSleepState(bool enemy)
    {
        if (!enemy)
        {
            foreach (var c in instance.field)
            {
                var card = (c as Transform).GetComponent<Card>();
                if (card.sleepState < 1) card.sleepState++;
            }
        }
        else
        {
            foreach (var c in instance.enemyField)
            {
                var card = (c as Transform).GetComponent<Card>();
                if (card.sleepState < 1) card.sleepState++;
            }
        }
    }

    public static void Fight(Card a, Card b)
    {
        if (a.strength < b.strength)
        {
            CardToGraveyard(a, a.enemy);
            GameManager.StartAttackAnim(true, false, a.sprite.sprite, b.sprite.sprite);
        }
        else if (a.strength > b.strength)
        {
            CardToGraveyard(b, b.enemy);
            a.sleepState = -1;
            GameManager.StartAttackAnim(false, true, a.sprite.sprite, b.sprite.sprite);
        }
        else
        {
            CardToGraveyard(a, a.enemy);
            CardToGraveyard(b, b.enemy);
            GameManager.StartAttackAnim(true, true, a.sprite.sprite, b.sprite.sprite);
        }

        GameManager.HideCrosshair();
        RearrangeField();
    }

    public static void CardToGraveyard(Card card, bool enemy)
    {
        var _parent = ((enemy) ? instance.enemyGraveyard : instance.graveyard);

        card.transform.parent = _parent;
        card.transform.position = _parent.position;
        card.gameObject.SetActive(false);
    }
}
