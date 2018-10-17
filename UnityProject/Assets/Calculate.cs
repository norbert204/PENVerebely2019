using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calculate : MonoBehaviour {

    [SerializeField] InputField aSpeed;
    [SerializeField] InputField bSpeed;
    [SerializeField] InputField flySpeed;
    [SerializeField] InputField distance;
    [SerializeField] InputField abDistance;
    [SerializeField] Text result;

    public void Calc() {
        double x = double.Parse(abDistance.text);
        double y = double.Parse(distance.text);
        double V1 = double.Parse(aSpeed.text);
        double V2 = double.Parse(bSpeed.text);
        double V3 = double.Parse(flySpeed.text);

        double res = (x - (y + (V2 * (V3 / x)))) / (V1 + V2);

        result.text = res.ToString();
    }
}
