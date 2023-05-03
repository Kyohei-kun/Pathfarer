using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_TestTargeting : MonoBehaviour
{
float cdDuration = 2;
    float actualTime = 0;

    List<GameObject> targets = new List<GameObject>();

    // Voir avec Eric pour faire en fonction de ses gabaris, et de sa hauteur de plafond
    float hauteurMaxTargeting = 10;
    [SerializeField] SphereCollider targetingRange;

    void Update()
    {
        actualTime += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(actualTime > cdDuration)
            {
                MakeTargetsList();
            }
            
            actualTime = 0;
        }
    }

    void MakeTargetsList()
    {
        // Prendre les objets dans Collider
        // Trier ceux en dessous et trop haut (+- une marge de manoeuvre ?)
        // Activer leur script de targeting weight (ou le faite OnTriggerEnter ?)
        // Faire une liste en fonction de Targeting weight (du grd au ptt)
    }
}
