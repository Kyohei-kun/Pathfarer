using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Temporary checkpoint if player fall into void
/// </summary>
public class CS_TemporalCheckpoint : MonoBehaviour
{
    Vector3 currentCheckpoint = Vector3.zero;
    bool drawGizmo;
    CS_PlayerLife playerLife;
    CharacterController characterController;

    [Button][HideIf("drawGizmo")] public void DrawGizmo() { drawGizmo = true; }
    [Button][ShowIf("drawGizmo")] public void HideGizmo() { drawGizmo = false; }

    private void Start()
    {
        playerLife = GetComponent<CS_PlayerLife>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Vector3.Distance(Vector3.ProjectOnPlane(transform.position, Vector3.up), Vector3.ProjectOnPlane(currentCheckpoint, Vector3.up)) > 3)
        {
            currentCheckpoint = transform.position;
        }
    }

    public void PlayerFall()
    {
        playerLife.LoseLife();
        if(playerLife.CurrentLife !=0)
        StartCoroutine(Co_PlayerFall());
    }

    private IEnumerator Co_PlayerFall()
    {
        yield return new WaitForSeconds(0.8f);
        characterController.enabled = false;
        playerLife.transform.position = currentCheckpoint;
        characterController.enabled = true; 
    }


    private void OnDrawGizmos()
    {
        if(drawGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(currentCheckpoint, 0.3f);
            Gizmos.DrawLine(currentCheckpoint, currentCheckpoint + Vector3.up);
        }
    }
}
