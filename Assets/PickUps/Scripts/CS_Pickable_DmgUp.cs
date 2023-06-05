using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable_DmgUp : CS_Pickable
{
    [Header("Special Values")]
    [SerializeField] float bonus = 0.5f;

    public override void PickEffect()
    {
        base.PickEffect();

        player.GetComponentInChildren<CS_PlayerSword>().BonusDmg += bonus;
    }
}
