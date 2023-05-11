using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ProtoTP : MonoBehaviour, CS_I_Subscriber
{
    [SerializeField] GameObject preview;
    GameObject previewInstance;

    bool previewON;

    /// <summary>
    /// Début de la prévisualisation de la TP.
    /// </summary>
    public void StartPreviewTP()
    {
        previewInstance = Instantiate(preview, PreviewPosition(), Quaternion.identity);
        previewON = true;
    }

    /// <summary>
    /// Arrête la preview de la TP.
    /// </summary>
    public void EndPreviewTP()
    {
        previewON = false;
        Destroy(previewInstance);
    }

    private void Start()
    {
        Invoke(nameof(SubscribeTargeting), 1);
    }

    private void Update()
    {
        if (previewON)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<CharacterController>().enabled = false;

                Vector3 playerPos = transform.position;
                Vector3 previewPos = previewInstance.transform.position;

                previewInstance.transform.position = playerPos;
                transform.position = previewPos;

                GetComponent<CharacterController>().enabled = true;
            }
            else
            {
                previewInstance.transform.SetPositionAndRotation(PreviewPosition(), Quaternion.identity);
            }
        }
    }

    Vector3 PreviewPosition()
    {
        Vector3 playerPos = transform.position;
        Vector3 center = GetComponent<CS_F_Targeting>().GetActualTarget().transform.position;
        Vector3 symPos = (center - playerPos) + center;
        symPos = new Vector3(symPos.x, playerPos.y, symPos.z);

        return symPos;
    }

    public void SubscribeTargeting()
    {
        GetComponent<CS_F_Targeting>().AddSubscriber(this);
    }

    public void UpdateTarget(GameObject target)
    {
        throw new System.NotImplementedException();
        // a effeacer
        // si null fait ça
        // si pas numll fait autre chose ...
    }
}
