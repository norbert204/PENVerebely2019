using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mosquito : MonoBehaviour {

    Rigidbody2D rb;

    [SerializeField] float health = 80;
    bool draining = false;
    public Human humanToDrain;

    [SerializeField] Transform sprite;

    [Header("Movement")]
    [SerializeField] float speed = 2;
    Vector3 joyOriginScreen;
    Vector2 joyOrigin;
    Vector2 joyOffset;

    [Header("Joystick")]
    bool pressed = false;
    [SerializeField] Transform innerCircle;
    [SerializeField] Transform outerCircle;

    [Header("UI")]
    [SerializeField] Image hpBar;
	
    void Start () {
        rb = GetComponent<Rigidbody2D>();
	}

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) TouchBegin();
        else if (Input.GetMouseButton(0)) Touching();
        else TouchEnd();

        if (Input.touches.Length > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) TouchBegin();
            else if (touch.phase == TouchPhase.Moved) Touching();
            else TouchEnd();
        }

        if (!draining)
            health -= Time.deltaTime * 1.5f;
        if (health <= 0)
            GameManager.Die("Éhenhaltál!");

        hpBar.fillAmount = health / 100f;

        if (draining) 
        {
            health += 2f * Time.deltaTime;
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

        innerCircle.GetComponent<SpriteRenderer>().enabled = (pressed && !draining);
        outerCircle.GetComponent<SpriteRenderer>().enabled = (pressed && !draining);
	}

    void FixedUpdate()
    {
        if (pressed && !draining) {
            Vector2 offset = joyOffset - joyOrigin;
            Vector2 direction = Vector2.ClampMagnitude(offset, .5f);
            Move(direction);

            innerCircle.transform.position = new Vector2(joyOrigin.x + direction.x, joyOrigin.y + direction.y);
            sprite.transform.rotation = Quaternion.Euler(direction);

            sprite.transform.rotation = Quaternion.Euler(0, 0, (direction.x < 0) ? 360 - (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg) : Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg * -1);
        }
        else {
            Move(Vector2.zero);
        }
    }


    void Move(Vector2 direction) {
        rb.velocity = direction * speed * 2;
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
    void TouchBegin()
    {
        joyOriginScreen = Input.mousePosition;
        joyOrigin = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        innerCircle.transform.position = joyOrigin;
        outerCircle.transform.position = joyOrigin;
    }
    void Touching()
    {
        pressed = true;
        joyOrigin = Camera.main.ScreenToWorldPoint(new Vector2(joyOriginScreen.x, joyOriginScreen.y));
        joyOffset = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
    }
    void TouchEnd()
    {
        pressed = false;
    }
}
