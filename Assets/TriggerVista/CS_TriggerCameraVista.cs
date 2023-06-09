using Cinemachine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CS_TriggerCameraVista : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    static bool drawGizmo;

    [Button][HideIf("drawGizmo")] public void DrawGizmo() { drawGizmo = true; }
    [Button][ShowIf("drawGizmo")] public void HideGizmo() { drawGizmo = false; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            virtualCamera.Follow = other.gameObject.transform;
            virtualCamera.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            virtualCamera.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}
