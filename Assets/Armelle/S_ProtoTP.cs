using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ProtoTP : MonoBehaviour
{
    [SerializeField] GameObject cubePreview;

    bool previewON;

    /// <summary>
    /// Début de la prévisualisation de la TP.
    /// </summary>
    public void StartPreviewTP()
    {
        previewON = true;
    }

    /// <summary>
    /// Arrête la preview de la TP.
    /// </summary>
    public void EndPreviewTP()
    {
        previewON = false;
    }

    private void Update()
    {
        if (previewON)
        {
            Vector3 playerPos = transform.position;
            Vector3 center = GetComponent<CS_F_Targeting>().GetActualTarget().transform.position;
            Vector3 symPos = (center - playerPos) + center;
            cubePreview.transform.SetPositionAndRotation(symPos, Quaternion.identity);
        }
    }
}
