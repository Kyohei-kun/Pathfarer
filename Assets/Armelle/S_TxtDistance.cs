using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class S_TxtDistance : MonoBehaviour
{
    [SerializeField] Transform playerTr;
    [SerializeField] TMP_Text text;

    float dist;
    Vector3 playerLook;
    Vector3 ennemyRelativePos;
    float targetingWeight;

    void Update()
    {
        dist = Vector3.Distance(transform.position, playerTr.position);
        playerLook = playerTr.forward;
        ennemyRelativePos = transform.position - playerTr.position;
        targetingWeight = Vector3.Dot(ennemyRelativePos, playerLook) - (dist * 1.5f);

        text.text = $"{Mathf.Round(targetingWeight * 100)/100}";
    }

    public float GetTargetingWeight()
    {
        return targetingWeight;
    }
}
