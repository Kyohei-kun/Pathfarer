using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

public class S_TxtDistance : MonoBehaviour
{
    Transform playerTr;
    [SerializeField] TMP_Text text;

    float dist;
    Vector3 playerLook;
    Vector3 ennemyRelativePos;
    float targetingWeight;

    [Dropdown("txtValues")] public string txtValue;
    string[] txtValues = new string[] { "Targeting Weight", "Player Distance" };

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

        if (txtValue == txtValues[0]) // Targeting Weight
        {
            text.text = $"{Mathf.Round(targetingWeight * 100) / 100}";
        }
        else if (txtValue == txtValues[1]) // Player Distance
        {
            text.text = $"{Mathf.Round(dist * 100) / 100}";
        }
    }

    public float GetTargetingWeight()
    {
        return targetingWeight;
    }
}
