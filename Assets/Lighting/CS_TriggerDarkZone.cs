using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TriggerDarkZone : MonoBehaviour
{
    [ReadOnly] public bool playerIN = false;

    public bool PlayerIN { get => playerIN;}

    private void Start()
    {
        CS_TriggerMerger.AddTrigger(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MainCamera")
        {
            playerIN = true;
            CS_TriggerMerger.UpdateMerger();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            playerIN = false;
            CS_TriggerMerger.UpdateMerger();
        }
    }
}
