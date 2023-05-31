using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable_Heal : CS_Pickable
{
    public override void PickEffect()
    {
        base.PickEffect();

        player.GetComponent<CS_PlayerLife>().GainLife();
    }
}
