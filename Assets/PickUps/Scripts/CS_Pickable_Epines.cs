using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable_Epines : CS_Pickable
{
    public override void PickEffect()
    {
        base.PickEffect();

        player.GetComponent<CS_PassifEpines>().IsActif = true;
    }
}
