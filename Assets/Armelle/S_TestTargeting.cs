using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class S_TestTargeting : MonoBehaviour
{
    readonly float cdDuration = 1;
    float actualTime = 0;

    Dictionary<GameObject, float> targetableObjects = new Dictionary<GameObject, float>();
    int actualIndex = 0;
    GameObject actualTarget;

    readonly float targetingMaxHauteur = 10;
    readonly float targetingMargeHauteur = 2;
    readonly float targetingRange = 10;
    readonly float targetingMargeRange = 2;

    void Update()
    {
        actualTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetTarget(true);
        }
    }

    void SetTarget(bool viaInput)
    {
        if (actualTime > cdDuration)
        {
            ClearTargetableList();
            SetTargetableList();
            SortTargetableList();

            if (viaInput)
            {
                actualIndex = 0;
            }
        }
        else
        {
            SetTargetableList();

            if (viaInput)
            {
                SetActualIndex(true);
            }                
        }

        actualTime = 0;

        if (targetableObjects.Count > 0)
        {
            SetActualTarget(targetableObjects.ElementAt(actualIndex).Key);
        }
    }

    void ClearTargetableList()
    {
        foreach (GameObject g in targetableObjects.Keys.ToList())
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
        Collider[] others = Physics.OverlapSphere(transform.position, targetingRange);

        for (int i = 0; i < others.Length; i++)
        {
            others[i].TryGetComponent(out S_TxtDistance scriptWright);

            if (scriptWright != null && OnSameLevel(others[i].gameObject) && !targetableObjects.ContainsKey(others[i].gameObject))
            {
                scriptWright.enabled = true;
                targetableObjects.Add(others[i].gameObject, scriptWright.GetTargetingWeight());
            }
        }
    }

    void SortTargetableList()
    {
        // MaJ les poids de targeting
        foreach (GameObject g in targetableObjects.Keys.ToList())
        {
            g.TryGetComponent(out S_TxtDistance c);
            targetableObjects[g] = c.GetTargetingWeight();
        }

        Dictionary<GameObject, float>  sortedTargetableObjects = targetableObjects.OrderByDescending(key => key.Value).ToDictionary(x => x.Key, x => x.Value);
        targetableObjects.Clear();
        targetableObjects = sortedTargetableObjects;
    }

    bool OnSameLevel(GameObject g)
    {
        bool isNotUnder = g.transform.position.y > gameObject.transform.position.y - targetingMargeHauteur;
        bool isNotUpper = g.transform.position.y < gameObject.transform.position.y + targetingMaxHauteur;

        if (isNotUnder && isNotUpper)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void SetActualIndex(bool plus)
    {
        if (plus)
        {
            actualIndex++;
        }

        if (targetableObjects.Count > 0)
        {
            actualIndex %= (targetableObjects.Count);
        }
    }

    void SetActualTarget(GameObject g)
    {
        if (actualTarget != null)
        {
            actualTarget.transform.Find("Target").gameObject.SetActive(false);
        }
        
        actualTarget = g;
        actualTarget.transform.Find("Target").gameObject.SetActive(true);
    }

    /// <summary>
    /// Renvois la target actuelle du joueur.
    /// </summary>
    /// <returns>GameObject actualTarget</returns>
    public GameObject GetActualTarget()
    {
        return actualTarget;
    }

    /// <summary>
    /// A appeler lors de la mort d'un ennemis par exemple. Enl�ve l'objet de la liste de cibles potentielles. Rafraichis le targeting.
    /// </summary>
    /// <param name="g"> GameObject � enlever de la liste.</param>
    public void RemoveFromTargetableList(GameObject g)
    {
        if (targetableObjects.ContainsKey(g))
        {
            targetableObjects.Remove(g);
            SetActualIndex(false);
            SetTarget(false);
        }
    }

    /// <summary>
    /// Renvois la range de targeting + la marge de d�-targeting. Actuellement appel� par le script qui g�re le poids de targeting sur les ennemis, pour qu'ils s'auto-g�rent.
    /// </summary>
    /// <returns>float targetingRange + targetingMargeRange</returns>
    public float GetRangeUntargeting()
    {
        return targetingRange + targetingMargeRange;
    }
}
