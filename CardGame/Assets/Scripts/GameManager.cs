using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    [SerializeField] Bot bot;

    public static int mana;
    public static int maxMana;
    public static bool turn;

    [SerializeField] GameObject manaPrefab;
    [SerializeField] Transform manaParent;

    [SerializeField] Button endTurnButton;
    
    [SerializeField] public List<GameObject> deck;
    [SerializeField] public List<GameObject> botDeck;

    [SerializeField] GameObject crosshair;

    [SerializeField] GameObject attack;
    [SerializeField] Animator attackAnim1;
    [SerializeField] Animator attackAnim2;
    [SerializeField] Image attackImage1;
    [SerializeField] Image attackImage2;

    [SerializeField] GameObject yourTurn;

    [SerializeField] GameObject shieldPrefab;
    public static int shield;
    [SerializeField] SpriteRenderer shieldSprite;
    [SerializeField] Transform shieldParent;
     public static int enemyShield;
    [SerializeField] SpriteRenderer enemyShieldSprite;
    [SerializeField] Transform enemyShieldParent;

    [SerializeField] GameObject gameOver;
    [SerializeField] Text gameOverText;
    [SerializeField] Button gameOverBackToMain;

    void Start()
    {
        maxMana = 1;
        mana = 1;
        UpdateManaUI();
        turn = true;
        instance = this;

        shield = 5;
        enemyShield = 5;

        endTurnButton.onClick.AddListener(EndTurn);
        yourTurn.GetComponentInChildren<Text>().text = LanguageText.yourTurn[LanguageText.GetLangID()];

        var _deck = FindObjectOfType<DeckToUse>();
        deck = _deck.deckToUse;
        Destroy(_deck.gameObject);

        for (int i = 0; i < 3; i++) AddCardToHand(false, false);
        for (int i = 0; i < 2; i++) AddCardToHand(false, true);

        UpdateShield();

        gameOverBackToMain.onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        });
    }

    void Update()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        crosshair.transform.position = new Vector3(pos.x, pos.y, 0);
    }

    public void GameOver(bool win)
    {
        gameOver.SetActive(true);
        if (win)
            gameOverText.text = LanguageText.victory[LanguageText.GetLangID()];
        else
            gameOverText.text = LanguageText.defeat[LanguageText.GetLangID()];
        gameOverBackToMain.GetComponentInChildren<Text>().text = LanguageText.backToMainMenu[LanguageText.GetLangID()];
    }

    public static void EndTurn()
    {
        turn = !turn;
        instance.endTurnButton.interactable = turn;
        if (turn)
        {
            instance.yourTurn.SetActive(true);
            if (maxMana < 10) maxMana++;
            mana = maxMana;
            instance.UpdateManaUI();

            AddCardToHand(true, false);
        }
        else
        {
            AddCardToHand(true, true);
            mana = maxMana;
            instance.UpdateManaUI();
            instance.EnemyTurn();
        }
        CardManager.UpdateSleepState(!turn);
    }

    void EnemyTurn()
    {
        StartCoroutine(bot.Turn());
    }

    public static void AddCardToHand(bool spawnAnim, bool enemy)
    {
        var _hand = ((enemy) ? CardManager.instance.enemyHand : CardManager.instance.hand);
        var _deck = ((enemy) ? instance.botDeck : instance.deck);

        if (_deck.Count > 0 && _hand.childCount < 5)
        {
            var g = _deck[Random.Range(0, _deck.Count)];
            var tmp = (GameObject)Instantiate(g, _hand);
            _deck.Remove(g);
            CardManager.RearrangeHand();
            tmp.GetComponent<Card>().enemy = enemy;
            if (spawnAnim)
            {
                tmp.GetComponent<Card>().sprite.transform.position = ((enemy) ? CardManager.instance.enemyStack.position : CardManager.instance.stack.position);
                tmp.GetComponent<Card>().spawnAnim = true;
                tmp.GetComponent<Card>().sizeLerp = 1.6f;
            }
            else
                tmp.GetComponent<Card>().sizeLerp = 2f;
        }
    }

    public static void DereaseMana(int amount)
    {
        mana -= amount;
        instance.UpdateManaUI();
    }

    void UpdateManaUI()
    {
        foreach (var g in manaParent) Destroy((g as Transform).gameObject);
        for (int i = 0; i < mana; i++)
        {
            Instantiate(manaPrefab, manaParent);
        }
    }

    public static void ShowCrosshair()
    {
        instance.crosshair.SetActive(true);
    }
    public static void HideCrosshair()
    {
        instance.crosshair.SetActive(false);
    }

    public static void HitFace(GameObject card, bool enemy)
    {
        bool canHit = true;
        int i = 0;
        var _field = ((enemy) ? CardManager.instance.field : CardManager.instance.enemyField);

        while (i < _field.childCount && canHit)
        {
            var c = _field.GetChild(i).GetComponent<Card>();
            if (c.sleepState == 1) canHit = false;
            i++;
        }

        if (canHit)
        {
            if (enemy) shield -= card.GetComponent<Card>().shieldBreak;
            else enemyShield -= card.GetComponent<Card>().shieldBreak;

            var _shield = ((enemy) ? shield : enemyShield);

            StartAttackAnim(false, (_shield <= 0), card.GetComponent<Card>().sprite.sprite, ((enemy) ? instance.shieldSprite.sprite : instance.enemyShieldSprite.sprite));
            if (_shield <= 0) instance.GameOver(!enemy);
            card.GetComponent<Card>().sleepState = -1;
            UpdateShield();
            EndTurn();
        }
        HideCrosshair();
        CardManager.AddOrBackToField(card, card.GetComponent<Card>().enemy);
    }

    public static void UpdateShield()
    {
        //
        //      Player
        //
        if (instance.shieldParent.childCount != shield)
        {
            foreach (var t in instance.shieldParent)
            {
                Destroy((t as Transform).gameObject);
            }
            float startX = -((shield / 2) * .25f) + ((shield % 2 == 0) ? .125f : 0);
            for (int i = 0; i < shield; i++)
            {
                var tmp = (GameObject)Instantiate(instance.shieldPrefab, new Vector3(startX + (.25f * i), instance.shieldParent.position.y), Quaternion.identity, instance.shieldParent);
                tmp.GetComponentInChildren<SpriteRenderer>().sortingOrder = i + 1;
            }
        }

        //
        //      Enemy
        //
        if (instance.enemyShieldParent.childCount != enemyShield)
        {
            foreach (var t in instance.enemyShieldParent)
            {
                Destroy((t as Transform).gameObject);
            }
            float startX = -((enemyShield / 2) * .25f) + ((enemyShield % 2 == 0) ? .125f : 0);
            for (int i = 0; i < enemyShield; i++)
            {
                var tmp = (GameObject)Instantiate(instance.shieldPrefab, new Vector3(startX + (.25f * i), instance.enemyShieldParent.position.y), Quaternion.identity, instance.enemyShieldParent);
                tmp.GetComponentInChildren<SpriteRenderer>().sortingOrder = i + 1;
            }
        }
    }

    public static void EndAttackAnim()
    {
        instance.attack.SetActive(false);
    }

    public static void StartAttackAnim(bool die1, bool die2, Sprite spr1, Sprite spr2)
    {
        instance.attack.SetActive(true);

        instance.attackImage1.sprite = spr1;
        instance.attackImage2.sprite = spr2;

        instance.attackAnim1.SetBool("die", die1);
        instance.attackAnim2.SetBool("die", die2);
    }
}
