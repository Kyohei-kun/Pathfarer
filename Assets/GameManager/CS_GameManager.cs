using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CS_GameManager : MonoBehaviour
{
    private void OnDestroy()
    {
        CS_TriggerMerger.Initialisation();
    }
}
