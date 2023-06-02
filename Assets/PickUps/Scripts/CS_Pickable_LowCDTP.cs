using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable_LowCDTP : CS_Pickable
{
    [Header("Special Values")]
    [MinValue(0)][SerializeField] float newCdTp = 0.5f;

    public override void PickEffect()
    {
        base.PickEffect();

        player.GetComponent<CS_F_Teleportation>().CdTP = newCdTp;
    }
}
