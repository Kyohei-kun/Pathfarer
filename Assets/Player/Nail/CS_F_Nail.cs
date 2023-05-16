using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CS_F_Nail : MonoBehaviour
{
    bool _lastInput;
    [SerializeField] GameObject pref_Nail;
    [SerializeField] int nbMaxNail = 4;
    [ProgressBar("Nails", "nbMaxNail", EColor.Gray)] [SerializeField] int nbCurrentNail = 0;

    [SerializeField] List<CS_Nail> nails = new();
    [SerializeField] LayerMask layerMask;

    private void PutNail()
    {
        if (nbCurrentNail < nbMaxNail)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.down), out hit, 6f, layerMask))
            {
                GameObject temp = GameObject.Instantiate(pref_Nail);
                temp.transform.position = hit.point;
                nails.Insert(0,temp.GetComponent<CS_Nail>());
                nails[0].Hit = hit;
                nbCurrentNail++;
            }
        }
    }

    public void RetakeNail(CS_Nail nail)
    {
        nails.Remove(nail);
        nbCurrentNail--;
    }

    public void OnNail(CallbackContext context)
    {
        if (context.ReadValueAsButton() == true && _lastInput == false)
            PutNail();

        _lastInput = context.ReadValueAsButton();
    }

   
}
