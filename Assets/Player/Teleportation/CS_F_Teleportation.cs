using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEngine.InputSystem.InputAction;
using Color = UnityEngine.Color;

public class CS_F_Teleportation : MonoBehaviour, CS_I_Subscriber
{
    bool inputState;
    bool lastInputState;

    [Foldout("Feedbacks preview")][SerializeField] GameObject preview;
    GameObject previewInstance;
    bool previewON;
    GameObject actualTarget;

    enum PreviewMods { tpPossible, tpBloque };
    VisualEffect fxCD;
    bool tpPossible;
    [Foldout("Feedbacks preview")][SerializeField][ColorUsage(true, true)] Color colorPossible;
    [Foldout("Feedbacks preview")][SerializeField][ColorUsage(true, true)] Color colorBloque;
    public static int colorID = Shader.PropertyToID("_MainColor");

    [MinValue(0)][SerializeField] float cdTP = 0.5f;
    float actualTime = 0;

    [Foldout("Valeurs marges TP")][MinValue(0.25f / 2)][SerializeField] float margeHauteurEscaliers = 1;
    [Foldout("Valeurs marges TP")][MinValue(0)][SerializeField] float margeDistanceMurs = 1;
    [ValidateInput("GreaterThan", "Hauteur Vide OK doit être supérieure ou égale à Marge Hauteur Escaliers !")]
    [Foldout("Valeurs marges TP")][MinValue(0)][SerializeField] float hauteurVideOK = 1;
    bool GreaterThan(float value) { return value >= margeHauteurEscaliers; }

    float hauteurTP;

    int tpLevel;
    public int TpLevel { get => tpLevel; set => tpLevel = Mathf.Clamp(value, 0, 2); }

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

            if (inputState && !lastInputState && tpPossible)
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
                // Preview Position false pour les escaliers
                previewInstance.transform.SetPositionAndRotation(PreviewPosition(false), Quaternion.identity);
            }
        }

        lastInputState = inputState;
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
            hauteurTP = playerPos.y + 0.2f;
        }

        return wall || empty;
    }

    void SetPreviewMod(PreviewMods newMod)
    {
        if (newMod == PreviewMods.tpPossible)
        {
            fxCD.Play();
            previewInstance.GetComponentInChildren<MeshRenderer>().material.SetColor(colorID, colorPossible);
            tpPossible = true;
        }
        else if (newMod == PreviewMods.tpBloque)
        {
            previewInstance.GetComponentInChildren<MeshRenderer>().material.SetColor(colorID, colorBloque);
            tpPossible = false;
        }
    }

    #region Interface Targeting
    public void SubscribeTargeting()
    {
        GetComponent<CS_F_Targeting>().AddSubscriber(this);
    }

    public void UpdateTarget(GameObject target)
    {
        actualTarget = target;

        if (actualTarget != null && ((tpLevel == 1 && target.GetComponent<CS_Enemy>() != null) || tpLevel == 2))
        {
            StartPreviewTP();
        }
        else
        {
            EndPreviewTP();
        }
    }
    #endregion

    public void OnTeleport(CallbackContext context)
    {
        inputState = context.ReadValueAsButton();
    }
}
