using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CS_TriggerDarkZone : MonoBehaviour
{
    [ReadOnly][SerializeField] bool playerIN = false;

    [Button][HideIf("drawGizmo")] public void DrawGizmo() { drawGizmo = true; }
    [Button][ShowIf("drawGizmo")] public void HideGizmo() { drawGizmo = false; }
    static bool drawGizmo = true;

    public bool PlayerIN { get => playerIN; }


    private void Start()
    {
        CS_TriggerMerger.AddTrigger(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIN = true;
            CS_TriggerMerger.UpdateMerger();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIN = false;
            CS_TriggerMerger.UpdateMerger();
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            Gizmos.color = new Color(0, 1, 0, 0.2f);
            Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().size);
        }
    }
}
