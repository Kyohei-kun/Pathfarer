using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_GameManager : MonoBehaviour
{
    private void OnDestroy()
    {
        CS_TriggerMerger.Initialisation();
    }
}
