using NaughtyAttributes;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable_VitesseUp : CS_Pickable
{
    [Header("Special Values")]
    [MinValue(0)][SerializeField] float newSpeed = 6.5f;

    public override void PickEffect()
    {
        base.PickEffect();

        player.GetComponent<ThirdPersonController>().moveSpeed = newSpeed;
    }
}
