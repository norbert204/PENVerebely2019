using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {
    
    [Header("Properties")]
    public bool isSpell;
    public bool targetedSpell;
    public bool targetedSpellOnEnemy;
    public int manaCost;
    public int strength;
    public int shieldBreak = 1;

    [Header("Misc")]
    public bool enemy;
    [HideInInspector] public sbyte sleepState;
    [SerializeField] Card cardToHit;
    public SpriteRenderer sprite;
    public Sprite[] sprites;
    [HideInInspector] public bool drag = false;
    public bool onField = false;
    bool toField = false;
    bool toFace = false;
    [SerializeField] bool zoom = false;
    BoxCollider2D coll;

    Vector3 posOfScreen;
    Vector3 offset;
    
    [HideInInspector] public bool spawnAnim = false;
    [HideInInspector] public byte spawnAnimState = 0; 

    [HideInInspector] public float sizeLerp;

    float currLerpTime;
    float lerpTime = 1;
    bool lastTurnState;


    private void Start()
    {
        currLerpTime = 0;
        sleepState = 0;
        lastTurnState = GameManager.turn;
        coll = GetComponent<BoxCollider2D>();

        if (enemy) sprite.sprite = CardManager.instance.cardBack;
        else sprite.sprite = sprites[LanguageText.GetLangID()];
    }

    private void OnMouseDown()
    {
        if (!enemy)
        {
            if (!onField)
            {
                if (GameManager.turn && GameManager.mana >= manaCost)
                {
                    drag = true;
                    if (isSpell && targetedSpell) GameManager.ShowCrosshair();
                }

            }
            else
            {
                if (!GameManager.turn || sleepState == 0)
                {
                    zoom = true;
                    CardManager.StartCardView(sprite.sprite, transform.position);
                }
                else if (GameManager.turn)
                {
                    if (sleepState == 1)
                    {
                        drag = true;
                        GameManager.ShowCrosshair();
                    }
                }
            }
        }
        else
        {
            zoom = true;
            CardManager.StartCardView(sprite.sprite, transform.position);
        }
    }

    void Update()
    {
        if (lastTurnState != GameManager.turn)
        {
            currLerpTime = 0;
            lastTurnState = !lastTurnState;
        }
        currLerpTime += Time.deltaTime;
        if (currLerpTime > lerpTime) currLerpTime = lerpTime;

        if ((sleepState < 1 && onField) || (GameManager.mana < manaCost && !onField && !enemy))
        {
            if (sprite.color != Color.gray) sprite.color = Color.gray;
        }
        else if ((sleepState == 1 && onField) || (GameManager.mana >= manaCost && !onField) || !lastTurnState)
        {
            if (sprite.color != Color.white) sprite.color = Color.white;
        }

        if (!onField && !drag)
        {
            if ((lastTurnState && !enemy) || (enemy && !lastTurnState))
            {
                sizeLerp = Mathf.Lerp(sizeLerp, 2, (currLerpTime / lerpTime));
                sprite.transform.localScale = new Vector3(sizeLerp, sizeLerp, 1);
            }
            else if ((!lastTurnState && !enemy) || (enemy && lastTurnState))
            {
                sizeLerp = Mathf.Lerp(sizeLerp, 1.6f, (currLerpTime / lerpTime));
                sprite.transform.localScale = new Vector3(sizeLerp, sizeLerp, 1);
            }
        }
        else if (onField)
        {
            sizeLerp = Mathf.Lerp(sizeLerp, 1.4f, (currLerpTime / lerpTime));
            sprite.transform.localScale = new Vector3(sizeLerp, sizeLerp, 1);
        }
        


        if (drag)
        {
            sprite.transform.localScale = new Vector3(4, 4, 1);
            sprite.sortingLayerName = "InHand";

            if (onField || (isSpell && targetedSpell))
            {
                sprite.transform.localPosition = new Vector3(-3, 0);
            }


            var screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            transform.position = new Vector3(screenPos.x, screenPos.y, 0);

            if (Input.GetMouseButtonUp(0))
            {
                drag = false;
                if (!onField)
                {
                    if (toField)
                    {
                        if (!isSpell)
                        {
                            if (CardManager.instance.field.childCount < 5)
                            {
                                onField = true;
                                CardManager.AddOrBackToField(gameObject, enemy);
                                GameManager.DereaseMana(manaCost);
                            }
                            else CardManager.BackToHand(gameObject, enemy);
                        }
                        else
                        {
                            GameManager.DereaseMana(manaCost);
                            OnCast();
                        }
                    }
                    else
                    {
                        if (!isSpell)
                            CardManager.BackToHand(gameObject, enemy);
                        else
                        {
                            if (targetedSpell)
                            {
                                if (cardToHit != null)
                                {
                                    GameManager.DereaseMana(manaCost);
                                    OnCast(cardToHit);
                                }
                                else
                                {
                                    CardManager.BackToHand(gameObject, enemy);
                                }
                            }
                            else
                            {
                                CardManager.BackToHand(gameObject, enemy);
                            }
                        }
                    }
                }
                else
                {
                    if (!toFace)
                    {
                        if (cardToHit != null)
                        {
                            CardManager.Fight(this, cardToHit);
                        }
                        else
                        {
                            CardManager.AddOrBackToField(gameObject, enemy);
                            GameManager.HideCrosshair();
                        }
                    }
                    else
                    {
                        GameManager.HitFace(gameObject, false);
                    }
                }
                GameManager.HideCrosshair();
            }
        }
        else
        {
            if (onField)
            {
                sprite.transform.localPosition = new Vector3(0, 0, 0);
            }

            sprite.sortingLayerName = "Card";
        }


        if (zoom)
        {
            if (Input.GetMouseButtonUp(0))
            {
                zoom = false;
                CardManager.EndCardView();
            }
        }

        if (onField) coll.size = new Vector2(1, 1.5f);
        else coll.size = new Vector2(2, 3);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!onField)
        {
            if (!isSpell || (isSpell && !targetedSpell))
                if (col.tag == "Field") toField = true;
        }
        else
        {
            if (col.tag == "Face") toFace = true;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (onField || (isSpell && targetedSpell))
        {
            if (col.tag == "Card")
            {
                var card = col.GetComponent<Card>();
                if (card.onField && ((!isSpell && card.enemy) || ((isSpell && targetedSpellOnEnemy) && card.enemy) || (isSpell && !targetedSpellOnEnemy && !card.enemy)))
                {
                    if (cardToHit == null) cardToHit = col.GetComponent<Card>();
                    else if (Vector3.Distance(transform.position, card.transform.position) < Vector3.Distance(transform.position, cardToHit.transform.position)) cardToHit = card;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!onField)
        {
            if (!isSpell || (isSpell && !targetedSpell))
            {
                if (col.tag == "Field") toField = false;
            }
        }
        else
        {
            if (col.tag == "Card")
            {
                if (cardToHit == col.GetComponent<Card>())
                    cardToHit = null;
            }
            else if (col.tag == "Face") toFace = false;
        }
    }

    //
    //      Methods to override
    //
    public virtual void OnCast()
    {
        print("OnCast - override me :(");
    }
    public virtual void OnCast(Card card)
    {
        print("OnCast - override me :(");
    }
}
