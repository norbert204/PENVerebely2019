using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calculate : MonoBehaviour {

    /*
     *      Extrák:
     *      - képlet animálása
     *      - reset gomb
     * 
     */

    [Header("UI")]
    [SerializeField] InputField aSpeedField;
    [SerializeField] InputField bSpeedField;
    [SerializeField] InputField flySpeedField;
    [SerializeField] InputField distanceField;
    [SerializeField] InputField abDistanceField;
    [SerializeField] Text result;
    [SerializeField] Text animSpeedText;
    [Header("Misc")]
    [SerializeField] Transform mosquito;
    [SerializeField] Transform human1;
    [SerializeField] Transform human2;

    float aSpeed = 0;
    float bSpeed = 0;
    float flySpeed = 0;
    float abDist = 0;
    float dist = 0;
    float res = 0;

    float timeInAnim = 0;
    float endTime = 0;

    int animSpeed = 1;

    bool anim = false;

    float flyMultipier = 1;

    private void Update()
    {
        animSpeedText.text = animSpeed + "x";
        if (anim)
        {
            if (timeInAnim <= endTime)
            {
                human1.position += (human1.right * (6f * (aSpeed / abDist))) * Time.deltaTime * animSpeed;
                human2.position += (human2.right * (6f * (bSpeed / abDist))) * Time.deltaTime * animSpeed;

                if (res >= timeInAnim)
                    mosquito.position = human1.position;
                else
                {
                    if (mosquito.position.x >= human2.position.x && flyMultipier > 0)
                        flyMultipier = -1;
                    else if (mosquito.position.x <= human1.position.x && flyMultipier < 0)
                        flyMultipier = 1;

                    mosquito.position += ((mosquito.right * (6f * (flySpeed / dist))) * Time.deltaTime) * flyMultipier * animSpeed;
                    mosquito.GetChild(0).localScale = new Vector2(2, flyMultipier * 2);
                }
                timeInAnim += Time.deltaTime * animSpeed;
            }
            else anim = false;
        }
    }

    public void AddSpeed() {
        if (animSpeed < 10)
            animSpeed++;
    }

    public void ReduceSpeed() {
        if (animSpeed > 1)
            animSpeed--;
    }

    public void Reset()
    {
        aSpeedField.text = "";
        bSpeedField.text = "";
        flySpeedField.text = "";
        abDistanceField.text = "";
        distanceField.text = "";

        aSpeed = 0;
        bSpeed = 0;
        flySpeed = 0;
        abDist = 0;
        dist = 0;
        res = 0;
    }

    public void Calc() {
        try
        {
            abDist = float.Parse(abDistanceField.text);
            dist = float.Parse(distanceField.text);
            aSpeed = float.Parse(aSpeedField.text);
            bSpeed = float.Parse(bSpeedField.text);
            flySpeed = float.Parse(flySpeedField.text);
        }
        catch {
            result.text = "Hibás vagy hiányzó adatok";
            return;
        }

        if (abDist <= 0)
            result.text = "A és B távolsága nem lehet 0 vagy annál kisebb";
        else if (dist <= 0)
            result.text = "A repülni kívánt távolság nem lehet 0 vagy annál kisebb";
        else if (dist > abDist)
            result.text = "A repülni kívánt távolságnak kisebbnek kell lennie A és B ember távolságánál";
        else if (aSpeed < 0)
            result.text = "A sebessége nem lehet negatív szám";
        else if (bSpeed < 0)
            result.text = "B sebessége nem lehet negatív szám";
        else if (flySpeed <= 0)
            result.text = "A szúnyog sebessége nem lehet 0 vagy annál kisebb";
        else if (aSpeed <= 0 && bSpeed <= 0)
            result.text = "Legalább az egyik embernek 0-nál nagyobb kell legyen a sebessége!";
        else
        {
            res = (abDist - (dist + (bSpeed * (flySpeed / abDist)))) / (aSpeed + bSpeed);

            endTime = abDist / (aSpeed + bSpeed);

            if (res > 0)
            {
                result.text = string.Format("Eredmény: {0:0.0000} másodperc", res);

                human1.position = new Vector2(-3, human1.position.y);
                human2.position = new Vector2(3, human2.position.y);
                mosquito.position = new Vector2(-3, mosquito.position.y);

                flyMultipier = 1;

                mosquito.GetChild(0).localScale = new Vector2(2, flyMultipier * 2);

                timeInAnim = 0;
                anim = true;
            }
            else
            {
                result.text = "Hibás adatok";
            }
        }
    }
}
