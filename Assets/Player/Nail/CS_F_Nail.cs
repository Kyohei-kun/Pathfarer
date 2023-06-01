using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CS_F_Nail : MonoBehaviour
{
    int nailLevel = 0;
    bool _lastInput;
    [SerializeField] GameObject pref_Nail;
    [ProgressBar("Nails", "nbMaxNail", EColor.Gray)][MinValue(0)][SerializeField] int nbCurrentNail = 0;

    [SerializeField] List<CS_Nail> nails = new();
    [SerializeField] LayerMask layerMask;

    public int NailLevel { get => nailLevel; set => nailLevel = value; }

    private void PutNail()
    {
        if (nbCurrentNail < nailLevel && !NearNail())
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.down), out hit, 6f, layerMask))
            {
                GameObject temp = GameObject.Instantiate(pref_Nail);
                temp.transform.position = hit.point;
                nails.Insert(0, temp.GetComponent<CS_Nail>());
                nails[0].Hit = hit;
                nbCurrentNail++;
            }
        }
    }

    private bool NearNail()
    {
        foreach (var item in nails)
        {
            if (Vector3.Distance(transform.position, item.transform.position) < 1)
            {
                return true;
            }
        }
        return false;
    }

    public void RetakeNail(CS_Nail nail)
    {
        nails.Remove(nail);
        nbCurrentNail--;
    }

    public void OnNail(CallbackContext context)
    {
        if (nailLevel != 0)
        {
            if (context.ReadValueAsButton() == true && _lastInput == false)
                PutNail();

            _lastInput = context.ReadValueAsButton();
        }
    }
}
