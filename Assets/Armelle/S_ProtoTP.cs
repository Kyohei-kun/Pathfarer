using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ProtoTP : MonoBehaviour, CS_I_Subscriber
{
    [SerializeField] GameObject preview;
    GameObject previewInstance;
    bool previewON;
    GameObject actualTarget;

    [MinValue(0)] [SerializeField] float cdTP = 0.5f;
    float actualTime = 0;

    private void Start()
    {
        Invoke(nameof(SubscribeTargeting), 1);
    }

    void StartPreviewTP()
    {
        previewInstance = Instantiate(preview, PreviewPosition(), Quaternion.identity);
        previewON = true;
    }

    void EndPreviewTP()
    {
        previewON = false;
        Destroy(previewInstance);
    }

    private void Update()
    {
        actualTime += Time.deltaTime;

        if (previewON)
        {
            if (Input.GetKeyDown(KeyCode.E) && actualTime > cdTP)
            {
                GetComponent<CharacterController>().enabled = false;

                Vector3 playerPos = transform.position;
                Vector3 previewPos = previewInstance.transform.position;

                previewInstance.transform.position = playerPos;
                transform.position = previewPos;

                actualTime = 0;

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
        Vector3 center = actualTarget.transform.position;
        Vector3 symPos = (center - playerPos) + center;
        symPos = new Vector3(symPos.x, playerPos.y, symPos.z);

        return symPos;
    }

    [InfoBox("En attendant que les niveaux soient gérés via le Manager.", EInfoBoxType.Normal)]
    [Dropdown("tpLevels")] public int tpLevel;
    int[] tpLevels = new int[] { 0, 1, 2 };

    #region Interface Targeting
    public void SubscribeTargeting()
    {
        GetComponent<CS_F_Targeting>().AddSubscriber(this);
    }

    public void UpdateTarget(GameObject target)
    {
        actualTarget = target;

        if (actualTarget != null && ((tpLevel == 1 && !target.CompareTag("Ennemy")) || tpLevel == 2))
        {
            StartPreviewTP();
        }
        else
        {
            EndPreviewTP();
        }
    }
    #endregion
}
