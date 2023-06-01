using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable_CapaNail : CS_Pickable
{
    public override void PickEffect()
    {
        base.PickEffect();

        if (scriptFeatures.State_Nail_Lvl4)
        {
            Debug.LogWarning($"{gameObject.name} pris sans nécessité : State_Nail_Lvl4 = {scriptFeatures.State_Nail_Lvl4}.");
        }
        else if (scriptFeatures.State_Nail_Lvl3)
        {
            scriptFeatures.State_Nail_Lvl4 = true;
        }
        else if (scriptFeatures.State_Nail_Lvl2)
        {
            scriptFeatures.State_Nail_Lvl3 = true;
        }
        else if (scriptFeatures.State_Nail_Lvl1)
        {
            scriptFeatures.State_Nail_Lvl2 = true;
        }
        else if (!scriptFeatures.State_Nail_Lvl1)
        {
            scriptFeatures.State_Nail_Lvl1 = true;
        }
    }
}
