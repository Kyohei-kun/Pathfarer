using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class S_TestTargeting : MonoBehaviour
{
    [Header("Valeurs Gameplay")]
    [SerializeField] float cdDuration = 1;
    float actualTime = 0;

    Dictionary<GameObject, float> targetableObjects = new Dictionary<GameObject, float>();
    int actualIndex = 0;

    // Voir avec Eric pour faire en fonction de ses gabaris, et de sa hauteur de plafond
    [Header("Valeurs LD")]
    [SerializeField] float targetingMaxHauteur = 10;
    [SerializeField] float targetingMarge = 2;
    [SerializeField] float targetingRange = 10;

    void Update()
    {
        actualTime += Time.deltaTime;
        Debug.LogWarning(actualTime);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Tab");
            SetTarget(true);
        }
    }

    void SetTarget(bool viaInput)
    {
        if(targetableObjects.Count > 0)
        {
            targetableObjects.ElementAt(actualIndex).Key.transform.Find("Target").gameObject.SetActive(false);
            Debug.Log("Désactive le visu de " + targetableObjects.ElementAt(actualIndex).Key.name);
        }

        // Peut être refaire la liste même si viaInput = false ?
        // A voir en jeu si joueur passe au suivant plus proche ou pas ...

        if (viaInput) 
        {
            if (actualTime > cdDuration)
            {
                ClearTargetableList();
                SetTargetableList();
                SortTargetableList();

                actualIndex = 0;
            }
            else
            {
                actualIndex++;
            }

            actualTime = 0;
        }

        if (targetableObjects.Count > 0)
        {
            targetableObjects.ElementAt(actualIndex).Key.transform.Find("Target").gameObject.SetActive(true);
            Debug.Log("Active le visu de " + targetableObjects.ElementAt(actualIndex).Key.name);
        }
    }

    void ClearTargetableList()
    {
        Debug.Log("Clear");
        foreach (GameObject g in targetableObjects.Keys)
        {
            g.TryGetComponent(out S_TxtDistance scriptWright);

            if (scriptWright != null)
            {
                scriptWright.enabled = false;
            }
        }

        targetableObjects.Clear();
    }

    void SetTargetableList()
    {
        Debug.Log("Set");
        Collider[] others = Physics.OverlapSphere(transform.position, targetingRange);

        for (int i = 0; i < others.Length; i++)
        {
            others[i].TryGetComponent(out S_TxtDistance scriptWright);

            if (scriptWright != null)
            {
                scriptWright.enabled = true;
                targetableObjects.Add(others[i].gameObject, scriptWright.GetTargetingWeight());
            }
        }
    }

    void SortTargetableList()
    {
        Debug.Log("Sort");
        // MaJ les poids de targeting
        foreach (GameObject g in targetableObjects.Keys)
        {
            g.TryGetComponent(out S_TxtDistance c);
            targetableObjects[g] = c.GetTargetingWeight();
        }

        targetableObjects.OrderBy(to => to.Value);
    }

    public void RemoveFromTargetableList(GameObject g)
    {
        targetableObjects.Remove(g);
        SetTarget(false);
    }
}
