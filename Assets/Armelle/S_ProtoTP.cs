using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class S_ProtoTP : MonoBehaviour, CS_I_Subscriber
{
    [Foldout("Feedbacks preview")][SerializeField] GameObject preview;
    GameObject previewInstance;
    bool previewON;
    GameObject actualTarget;

    enum PreviewMods {tpPossible, tpBloque };
    VisualEffect fxCD;
    bool tpPossible;
    [Foldout("Feedbacks preview")][SerializeField] Color colorPossible;
    [Foldout("Feedbacks preview")][SerializeField] Color colorBloque;

    [MinValue(0)] [SerializeField] float cdTP = 0.5f;
    float actualTime = 0;

    [Foldout("Valeurs marges TP")][MinValue(0.25f / 2)][SerializeField] float margeHauteurEscaliers = 1;
    [Foldout("Valeurs marges TP")][MinValue(0)][SerializeField] float margeDistanceMurs = 1;
    [ValidateInput("GreaterThan", "Hauteur Vide OK doit être supérieure ou égale à Marge Hauteur Escaliers !")]
    [Foldout("Valeurs marges TP")][MinValue(0)][SerializeField] float hauteurVideOK = 1;
    bool GreaterThan (float value) {return value >= margeHauteurEscaliers;}

    float hauteurTP;

    private void Start()
    {
        Invoke(nameof(SubscribeTargeting), 1);
    }

    void StartPreviewTP()
    {
        previewInstance = Instantiate(preview, PreviewPosition(true), Quaternion.identity);
        fxCD = previewInstance.GetComponentInChildren<VisualEffect>();
        previewON = true;

        if (actualTime > cdTP && !CheckSiWallOrEmpty())
        {
            SetPreviewMod(PreviewMods.tpPossible);
        }
        else
        {
            SetPreviewMod(PreviewMods.tpBloque);
        }
    }

    void EndPreviewTP()
    {
        previewON = false;
        Destroy(previewInstance);
        previewInstance = null;
        fxCD = null;
    }

    private void Update()
    {
        actualTime += Time.deltaTime;

        if (previewON)
        {
            if (actualTime > cdTP && !CheckSiWallOrEmpty())
            {
                if (!tpPossible)
                {
                    SetPreviewMod(PreviewMods.tpPossible);
                }
            }
            else if (tpPossible)
            {
                SetPreviewMod(PreviewMods.tpBloque);
            }

            if (Input.GetKeyDown(KeyCode.E) && tpPossible)
            {
                GetComponent<CharacterController>().enabled = false;

                Vector3 playerPos = transform.position;

                previewInstance.transform.position = playerPos;
                transform.position = PreviewPosition(false);

                actualTime = 0;

                GetComponent<CharacterController>().enabled = true;
            }
            else
            {
                previewInstance.transform.SetPositionAndRotation(PreviewPosition(false), Quaternion.identity);
            }
        }
    }

    Vector3 PreviewPosition(bool playerLevel)
    {
        Vector3 playerPos = transform.position;
        Vector3 center = actualTarget.transform.position;
        Vector3 symPos = (center - playerPos) + center;

        if (playerLevel)
        {
            symPos = new Vector3(symPos.x, playerPos.y, symPos.z);
        }
        else
        {
            symPos = new Vector3(symPos.x, hauteurTP, symPos.z);
        }

        return symPos;
    }

    bool CheckSiWallOrEmpty()
    {
        Vector3 playerPos = transform.position;
        Vector3 trans = PreviewPosition(true) - playerPos;
        Vector3 dir = trans.normalized;
        float dist = Vector3.Distance(playerPos, PreviewPosition(true));

        Vector3 margeH = Vector3.up * margeHauteurEscaliers;

        LayerMask layer = LayerMask.GetMask("Ground");

        bool wall;
        bool empty;

        Debug.DrawRay(playerPos + margeH, dir * (dist + margeDistanceMurs), Color.blue);
        if (Physics.Raycast(playerPos + margeH, dir, dist + margeDistanceMurs, layer))
        {
            wall = true;
        }
        else
        {
            wall = false;
        }
        
        Debug.DrawRay(PreviewPosition(true) + margeH, Vector3.down * hauteurVideOK, Color.red);
        if (Physics.Raycast(PreviewPosition(true) + margeH, Vector3.down, out RaycastHit hit, hauteurVideOK, layer))
        {
            empty = false;
            hauteurTP = hit.point.y;
        }
        else
        {
            empty = true;
        }

        return wall || empty;
    }
    
    void SetPreviewMod(PreviewMods newMod)
    {
        if (newMod == PreviewMods.tpPossible)
        {
            fxCD.Play();
            previewInstance.GetComponentInChildren<MeshRenderer>().material.color = colorPossible;
            tpPossible = true;
        }
        else if (newMod == PreviewMods.tpBloque)
        {
            previewInstance.GetComponentInChildren<MeshRenderer>().material.color = colorBloque;
            tpPossible = false;
        }
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

        if (actualTarget != null && ((tpLevel == 1 && target.GetComponent<CS_Enemy>( ) != null) || tpLevel == 2))
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
