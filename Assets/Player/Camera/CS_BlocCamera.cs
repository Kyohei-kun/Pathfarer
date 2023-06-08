using Cinemachine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_BlocCamera : MonoBehaviour
{
    [SerializeField] Collider _collider;
    CinemachineConfiner confiner;
    static bool drawGizmo;

    [Button][HideIf("drawGizmo")] public void DrawGizmo() { drawGizmo = true; }
    [Button][ShowIf("drawGizmo")] public void HideGizmo() { drawGizmo = false; }

    private void Start()
    {
        ICinemachineCamera temp;
        temp = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;
        if (temp is CinemachineVirtualCamera virtualCamera)
        {
            confiner = virtualCamera.GetComponent<CinemachineConfiner>();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            confiner.m_BoundingVolume = _collider;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            confiner.m_BoundingVolume = null;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            Gizmos.color = new Color(0, 1, 0.8f, 0.5f);
            Gizmos.DrawCube(transform.position, transform.localScale);
            Gizmos.color = new Color(1, 0.8f, 0.8f, 0.5f);
            Gizmos.DrawCube(_collider.transform.position, _collider.transform.lossyScale);
        }
    }
}
