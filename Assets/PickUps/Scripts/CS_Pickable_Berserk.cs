using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable_Berserk : CS_Pickable
{
    public override void PickEffect()
    {
        base.PickEffect();

        player.GetComponentInChildren<CS_PlayerSword>().Berserker = true;
    }
}
