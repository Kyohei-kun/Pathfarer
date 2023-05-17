using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class CS_TriggerMerger
{
    static List<CS_TriggerDarkZone> triggerLights = new List<CS_TriggerDarkZone>();
    static bool playerIn;
    static bool lastPlayerIn = false;
    static private CS_LightManager lightManager;
    static private CS_GolbalVolumePilote volumePilot;

    public static CS_LightManager LightManager { get => lightManager; set => lightManager = value; }
    public static CS_GolbalVolumePilote VolumePilot { get => volumePilot; set => volumePilot = value; }

    public static void AddTrigger(CS_TriggerDarkZone newTrigger)
    {
        triggerLights.Add(newTrigger);
    }

    public static void UpdateMerger()
    {
        playerIn = false;
        foreach (var item in triggerLights)
        {
            if (item.PlayerIN)
            {
                playerIn = true;
                break;
            }
        }

        if(lastPlayerIn != playerIn)
        {
            if(playerIn)
            {
                volumePilot.FadeToDarkProfil();
            }
            else
            {
                volumePilot.FadeToStandardProfil();
            }
            //lightManager.ChangeSate(playerIn);
        }

        lastPlayerIn = playerIn;
    }
}
