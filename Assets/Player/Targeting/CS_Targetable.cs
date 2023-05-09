using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Targetable : MonoBehaviour
{
    Transform playerTr;

    float dist;
    Vector3 playerLook;
    Vector3 ennemyRelativePos;
    float targetingWeight;

    private void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        dist = Vector3.Distance(transform.position, playerTr.position);
        playerLook = playerTr.forward;
        ennemyRelativePos = transform.position - playerTr.position;
        targetingWeight = Vector3.Dot(ennemyRelativePos, playerLook) - (dist * 1.5f);

        if (!GetComponent<MeshRenderer>().isVisible)
        {
            playerTr.GetComponent<CS_F_Targeting>().RemoveFromTargetableList(gameObject);
            enabled = false;
        }
    }

    /// <summary>
    /// Renvois le poids de piorité de targeting.
    /// </summary>
    /// <returns>float targetingWeight</returns>
    public float GetTargetingWeight()
    {
        return targetingWeight;
    }
}
