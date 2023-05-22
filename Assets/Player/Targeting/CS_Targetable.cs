using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CS_Targetable : MonoBehaviour
{
    Transform playerTr;
    VisualEffect effectPV;

    float dist;
    Vector3 playerLook;
    Vector3 ennemyRelativePos;
    float targetingWeight;
    [MinValue(0)][SerializeField] float untargetingDist = 18;

    private void Awake()
    {
        effectPV = transform.Find("Target/FX_TargetPV").GetComponent<VisualEffect>();
        effectPV.SetInt("Amount", 5);
        enabled = false;
    }

    private void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        dist = Vector3.Distance(transform.position, playerTr.position);

        if (!GetComponent<MeshRenderer>().isVisible || dist > untargetingDist)
        {
            playerTr.GetComponent<CS_F_Targeting>().RemoveFromTargetableList(gameObject, false);
            enabled = false;
        }
    }

    private void SetTargetingWeight()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        dist = Vector3.Distance(transform.position, playerTr.position);
        playerLook = playerTr.forward;
        ennemyRelativePos = transform.position - playerTr.position;
        targetingWeight = Vector3.Dot(ennemyRelativePos, playerLook) - (dist * 1.5f);
    }

    /// <summary>
    /// Renvois le poids de piorit� de targeting.
    /// </summary>
    /// <returns>float targetingWeight</returns>
    public float GetTargetingWeight()
    {
        SetTargetingWeight();
        return targetingWeight;
    }
}
