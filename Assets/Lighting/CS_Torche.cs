using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Torche : MonoBehaviour, CS_I_Allumage
{
    GameObject fire;
    bool state;
    [SerializeField] bool inCheckpoint;

    private void Start()
    {
        fire = transform.Find("Pillar/Fire").gameObject;
    }

    void ON()
    {
        fire.SetActive(true);

        if (inCheckpoint)
        {
            gameObject.GetComponentInParent<CS_Checkpoints>().CheckTorches();
        }
    }

    void OFF()
    {
        fire.SetActive(false);
    }

    /// <summary>
    /// Retourne si la torche est allumée ou éteinte.
    /// </summary>
    /// <returns>bool state</returns>
    public bool GetState()
    {
        return state;
    }

    #region Interface allumable
    public void SetState(bool on)
    {
        state = on;

        if (on)
        {
            ON();
        }
        else
        {
            OFF();
        }
    }
    #endregion
}
