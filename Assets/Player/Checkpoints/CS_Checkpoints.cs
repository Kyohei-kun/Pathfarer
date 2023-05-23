using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CS_Checkpoints : MonoBehaviour
{
    public static List<CS_Checkpoints> checkpointsConnus = new();
    public static CS_Checkpoints actualCheckpoint;

    List<CS_Torche> myTorches = new();
    GameObject player;
    MeshRenderer myDalle;

    [SerializeField] Material mtON;
    [SerializeField] Material mtOFF;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myTorches = GetComponentsInChildren<CS_Torche>().ToList();
        myDalle = transform.Find("Socle/Dalle").GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            if (FindObjectOfType<CS_FeatureUnlocker>().GetComponent<CS_FeatureUnlocker>().State_Mentor)
            {
                ActivationTorches();
            }

            UseCheckpoint();
        }
    }

    void ActivationTorches()
    {
        bool any = false;

        for (int i = 0; i < myTorches.Count; i++)
        {
            if (!myTorches[i].GetState())
            {
                myTorches[i].SetState(true);
                any = true;
            }
        }

        if (any)
        {
            // animation allumage Mnetor
        }
    }

    void UseCheckpoint()
    {
        Debug.LogWarning($"Reset du monde.\n" +
            $"Il faut encore l'implémenter !\n" +
            $"Penser a reset les ennemis, levels et tout, mais pas les portes, checkpoints, etc ...");

        if (actualCheckpoint != null)
        {
            actualCheckpoint.myDalle.material = mtOFF;
        }
        
        myDalle.material = mtON;
        actualCheckpoint = this;
    }

    /// <summary>
    /// Regarde si toutes les torches sont allumées.
    /// </summary>
    public void CheckTorches()
    {
        int nbT = 0;

        for (int i = 0;i < myTorches.Count;i++)
        {
            if (myTorches[i].GetState())
            {
                nbT++;
            }
        }

        if (nbT >= 4)
        {
            if (!checkpointsConnus.Contains(this))
            {
                checkpointsConnus.Add(this);
            }
        }
    }
}
