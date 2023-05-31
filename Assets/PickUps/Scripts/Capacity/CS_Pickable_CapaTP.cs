using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable_CapaTP : CS_Pickable
{
    public override void PickEffect()
    {
        base.PickEffect();

        if (!scriptFeatures.State_Teleportation_Lvl1 && !scriptFeatures.State_Teleportation_Lvl2)
        {
            scriptFeatures.State_Teleportation_Lvl1 = true;
        }
        else if (scriptFeatures.State_Teleportation_Lvl1 && !scriptFeatures.State_Teleportation_Lvl2)
        {
            scriptFeatures.State_Teleportation_Lvl2 = true;
        }
        else if (scriptFeatures.State_Teleportation_Lvl1 && scriptFeatures.State_Teleportation_Lvl2)
        {
            Debug.LogWarning($"{gameObject.name} pris sans nécessité : State_Teleportation_Lvl2 = {scriptFeatures.State_Teleportation_Lvl2}.");
        }
        else
        {
            Debug.LogWarning("Unexpected value of State_Teleportation_Lvl1 and State_Teleportation_Lvl2 !");
        }
    }
}
