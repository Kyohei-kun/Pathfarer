using NaughtyAttributes;
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
    [MinMaxSlider(0.0f, 70000.0f)][SerializeField] Vector2 intensityFlash;
    [SerializeField] Color flatColor;

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
        //StartCoroutine(FlashLight());
    }

    //IEnumerator FlashLight()
    //{
    //    for (float t = 0; t < 0.5f; t += Time.deltaTime)
    //    {
    //        mtON.color = flatColor * Mathf.Clamp(Mathf.Lerp(intensityFlash.x, intensityFlash.y, t.Remap(0, 0.5f, 0, 1)), intensityFlash.x, intensityFlash.y);
    //        yield return new WaitForSeconds(0f);
    //    }

    //    for (float t = 0; t < 0.5f; t += Time.deltaTime)
    //    {
    //        mtON.color = flatColor * Mathf.Clamp(Mathf.Lerp(intensityFlash.y, intensityFlash.x, t.Remap(0, 0.5f, 0, 1)), intensityFlash.x, intensityFlash.y);
    //        yield return new WaitForSeconds(0f);
    //    }

    //    mtON.color = flatColor * intensityFlash.x;
    //}

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
