using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimStop : MonoBehaviour {

	public void StopAnim()
    {
        GameManager.EndAttackAnim();
    }
}
