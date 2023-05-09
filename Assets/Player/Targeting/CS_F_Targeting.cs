using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CS_F_Targeting : MonoBehaviour
{
    [Foldout("Valeurs Gameplay")]
    [MinValue(0.0f)]
    [SerializeField] float cdDuration = 1;
    float actualTime = 0;

    Dictionary<GameObject, float> targetableObjects = new Dictionary<GameObject, float>();
    int actualIndex = 0;
    GameObject actualTarget;

    [Foldout("Valeurs LD")]
    [MinValue(0.0f)] [Label("Target H Max")]
    [SerializeField] float targetingMaxHauteur = 10;
    [Foldout("Valeurs LD")]
    [MinValue(0.0f)] [Label("Target H Marge")]
    [SerializeField] float targetingMargeHauteur = 2;
    [Foldout("Valeurs LD")]
    [MinValue(0.0f)] [Label("Untargeting Marge")]
    [SerializeField] float targetingMargeRange = 2;

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
            g.TryGetComponent(out CS_Targetable scriptWright);

            if (scriptWright != null)
            {
                scriptWright.enabled = false;
            }
        }

        targetableObjects.Clear();
    }

    /*[SerializeField] Mesh cube;
    Vector3 pos;
    Vector3 size;
    Quaternion rot;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawMesh(cube, pos, rot, size);
    }*/

    void SetTargetableList()
    {
        #region ChatGPT
        // Get the camera component
        Camera mainCamera = Camera.main;

        // Calculate the dimensions of the viewing frustum
        float height = 2f * mainCamera.orthographicSize;
        float width = height * mainCamera.aspect;

        // Create the overlap box centered on the camera
        Vector3 center = mainCamera.transform.position;
        Vector3 extents = new Vector3(width / 2, height / 2, 100f);
        Quaternion orientation = mainCamera.transform.rotation;

        // Perform the overlap check
        Collider[] colliders = Physics.OverlapBox(center, extents, orientation);
        /*pos = center;
        size = new Vector3(width, height, 50f);
        rot = orientation;*/

        #endregion

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].TryGetComponent(out CS_Targetable scriptWright);

            if (scriptWright != null && OnSameLevel(colliders[i].gameObject) && !targetableObjects.ContainsKey(colliders[i].gameObject))
            {
                scriptWright.enabled = true;
                targetableObjects.Add(colliders[i].gameObject, scriptWright.GetTargetingWeight());
            }
        }
    }

    void SortTargetableList()
    {
        // MaJ les poids de targeting
        foreach (GameObject g in targetableObjects.Keys.ToList())
        {
            g.TryGetComponent(out CS_Targetable c);
            targetableObjects[g] = c.GetTargetingWeight();
        }

        Dictionary<GameObject, float> sortedTargetableObjects = targetableObjects.OrderByDescending(key => key.Value).ToDictionary(x => x.Key, x => x.Value);
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
            if (actualTarget.transform.Find("Target"))
            {
                actualTarget.transform.Find("Target").gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"Untargeting {actualTarget.name}.\n" +
                    $"Ce message s'affiche car le child 'Target' n'a pas été trouvé dans {actualTarget.name}.\n" +
                    $"Pour régler ce problème, ajouter un child 'Target', ou changer le mode d'affichage du targeting.");
            }
        }

        actualTarget = g;

        if (actualTarget.transform.Find("Target"))
        {
            actualTarget.transform.Find("Target").gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Targeting {actualTarget.name}.\n" +
                    $"Ce message s'affiche car le child 'Target' n'a pas été trouvé dans {actualTarget.name}.\n" +
                    $"Pour régler ce problème, ajouter un child 'Target', ou changer le mode d'affichage du targeting.");
        }
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
    /// A appeler lors de la mort d'un ennemis par exemple. Enlève l'objet de la liste de cibles potentielles. Rafraichis le targeting.
    /// </summary>
    /// <param name="g"> GameObject à enlever de la liste.</param>
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
    /// Renvois la range de targeting + la marge de dé-targeting. Actuellement appelé par le script qui gère le poids de targeting sur les ennemis, pour qu'ils s'auto-gèrent.
    /// </summary>
    /// <returns>float targetingRange + targetingMargeRange</returns>
    public float GetRangeUntargeting()
    {
        return targetingMargeRange;
    }
}
