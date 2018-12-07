using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mosquito : MonoBehaviour {

    Rigidbody2D rb;

    [SerializeField] float health = 80;
    public float hungerMultiplier = 1.5f;
    bool draining = false;
    public Human humanToDrain;

    [SerializeField] Transform sprite;

    [Header("Movement")]
    [SerializeField] float speed = 2;
    Vector2 joyOffset;

    [Header("Joystick")]
    bool pressed = false;
    [SerializeField] Transform innerCircle;
    [SerializeField] Transform circleParent;

    [Header("UI")]
    [SerializeField] Image hpBar;
    [SerializeField] Button drainButton;
	
    void Start () {
        rb = GetComponent<Rigidbody2D>();
	}

    void Update()
    {
        /*if (Input.GetMouseButtonDown(0) && Input.mousePosition.x < Screen.width * .5f) TouchBegin();
        else*/ 
        if (Input.GetMouseButton(0) && Input.mousePosition.x < Screen.width * .5f) Touching();
        else TouchEnd();

        if (Input.touches.Length > 0)
        {
            Touch touch = Input.GetTouch(0);
            /*if (touch.phase == TouchPhase.Began && touch.position.x < Screen.width * .5f) TouchBegin();
            else*/ 
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved && touch.position.x < Screen.width * .5f) Touching();
            else TouchEnd();
        }

        if (!draining)
            health -= Time.deltaTime * hungerMultiplier;
        if (health <= 0)
            GameManager.Die("Éhenhaltál!");

        hpBar.fillAmount = health / 100f;

        if (!drainButton.interactable && humanToDrain != null)
        {
            drainButton.interactable = humanToDrain.hasVisibleSkin;
        }
        else if (drainButton.interactable && humanToDrain == null) 
            drainButton.interactable = false;

        if (draining && humanToDrain != null) 
        {
            health += 4f * Time.deltaTime;
            if (health >= 100) {
                GameManager.Die("Túl etted magad!");
            }
            humanToDrain.Drain();
            if (humanToDrain.aware) 
            {
                if (humanToDrain.awareness >= 10) {
                    GameManager.Die("Le lettél csapva!");
                }
            }
        }
	}

    void FixedUpdate()
    {
        if (pressed && !GameManager.dead) {
            Vector2 offset = joyOffset - (Vector2)circleParent.position;
            Vector2 direction = Vector2.ClampMagnitude(offset, .75f);
            Move(direction);

            innerCircle.transform.position = new Vector2(circleParent.position.x + direction.x, circleParent.position.y + direction.y);
            sprite.transform.rotation = Quaternion.Euler(direction);

            sprite.transform.rotation = Quaternion.Euler(0, 0, (direction.x < 0) ? 360 - (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg) : Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg * -1);
        }
        else {
            Move(Vector2.zero);
        }
    }


    void Move(Vector2 direction) {
        rb.velocity = direction * speed * 1.5f;
    }


    //
    //      Drain
    //
    public void StartDrain() {
        if (!humanToDrain.aware && humanToDrain.hasVisibleSkin)
            draining = true;
    }

    public void StopDrain() {
        draining = false;
    }


    //
    //      Joystick
    //
    void Touching()
    {
        pressed = true;
        joyOffset = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
    }
    void TouchEnd()
    {
        pressed = false;
        innerCircle.transform.position = circleParent.position;
    }
}
