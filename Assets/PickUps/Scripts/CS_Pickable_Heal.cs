using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable_Heal : CS_Pickable
{
    public override void PickEffect()
    {
        if(!player.GetComponent<CS_PlayerLife>().FullLife())
        {
            base.PickEffect();

            player.GetComponent<CS_PlayerLife>().GainLife();
        }
    }
}
