using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CS_F_Targeting : MonoBehaviour
{
    bool inputState;
    bool lastInputState;

    List<CS_I_Subscriber> subscribers = new();

    [MinValue(0)] [SerializeField] float cdDuration = 1; 
    float actualTime = 0;

    Dictionary<GameObject, float> targetableObjects = new();
    int actualIndex = 0;
    GameObject actualTarget;

    [MinValue(0)] [SerializeField] float targetingMaxHauteur = 10;
    [MinValue(0)] [SerializeField] float targetingMargeHauteur = 2;

    float inputStay = 0;
    [MinValue(0)][SerializeField] float cdUntargeting = 0.6f;

    public GameObject ActualTarget { get => actualTarget; set => actualTarget = value; }

    void Update()
    {
        actualTime += Time.deltaTime;

        // Input down
        if (inputState && !lastInputState)
        {
            SetTarget(true);
            inputStay = 0;

        } // Input up
        else if (!inputState && lastInputState)
        {
            if (inputStay > cdUntargeting)
            {
                ClearActualTarget();
                ClearTargetableList();
            }

        } // Input stay
        else if (inputState && lastInputState)
        {
            inputStay += Time.deltaTime;
        }

        lastInputState = inputState;
    }

    void SetTarget(bool viaInput)
    {
        if (actualTime > cdDuration || actualTarget == null)
        {
            ClearTargetableList();
            SetTargetableList();
            SortTargetableList();

            if (viaInput || actualTarget == null)
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
            if (g != null)
            {
                g.TryGetComponent(out CS_Targetable scriptWright);

                if (scriptWright != null)
                {
                    scriptWright.enabled = false;
                }
            }
        }

        targetableObjects.Clear();
    }

    void SetTargetableList()
    {
        #region ChatGPT get all colliders on screen
        // Get the camera component
        Camera mainCamera = Camera.main;

        // Calculate the dimensions of the viewing frustum
        float height = 2f * mainCamera.orthographicSize;
        float width = height * mainCamera.aspect;

        // Create the overlap box centered on the camera
        Vector3 center = mainCamera.transform.position;
        Vector3 halfExtents = new Vector3((width - 1) / 2, (height - 1) / 2, 100f);
        Quaternion orientation = mainCamera.transform.rotation;

        // Perform the overlap check
        Collider[] colliders = Physics.OverlapBox(center, halfExtents, orientation);
        #endregion

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].TryGetComponent(out CS_Targetable scriptWright);

            if (scriptWright != null && OnSameLevel(colliders[i].gameObject) && !targetableObjects.ContainsKey(colliders[i].gameObject))
            {
                scriptWright.enabled = true;
                targetableObjects.Add(colliders[i].gameObject, scriptWright.GetTargetingWeight());
            }
            else if (colliders[i].transform.parent)
            {
                colliders[i].transform.parent.TryGetComponent(out CS_Targetable scriptWrightInParent);

                if (scriptWrightInParent != null && OnSameLevel(colliders[i].transform.parent.gameObject) && !targetableObjects.ContainsKey(colliders[i].transform.parent.gameObject))
                {
                    scriptWrightInParent.enabled = true;
                    targetableObjects.Add(colliders[i].transform.parent.gameObject, scriptWrightInParent.GetTargetingWeight());
                }
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

        return isNotUnder && isNotUpper;
    }

    void SetActualIndex(bool plus)
    {
        if (targetableObjects.Count == 0)
        {
            ClearActualTarget();
            return;
        }

        if (plus)
        {
            actualIndex++;

            if (targetableObjects.Count > 0)
            {
                actualIndex %= (targetableObjects.Count);
            }
        }
        else
        {
            if (targetableObjects.Count > 0 && actualTarget != null)
            {
                for (int i = 0; i < targetableObjects.Count; i++)
                {
                    if (targetableObjects.Keys.ToList()[i] == actualTarget)
                    {
                        actualIndex = i;
                    }
                }
            }
        }
    }

    void SetActualTarget(GameObject g)
    {
        ClearActualTarget();
        actualTarget = g;

        if (actualTarget.transform.Find("PR_Target"))
        {
            actualTarget.transform.Find("PR_Target").gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Targeting {actualTarget.name}.\n" +
                    $"Ce message s'affiche car le child 'Target' n'a pas été trouvé dans {actualTarget.name}.\n" +
                    $"Pour régler ce problème, ajouter un child 'Target', ou changer le mode d'affichage du targeting.");
        }

        MessageUpdateSubscriber();
    }

    void ClearActualTarget()
    {
        if (actualTarget != null)
        {
            if (actualTarget.transform.Find("PR_Target"))
            {
                actualTarget.transform.Find("PR_Target").gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"Untargeting {actualTarget.name}.\n" +
                    $"Ce message s'affiche car le child 'Target' n'a pas été trouvé dans {actualTarget.name}.\n" +
                    $"Pour régler ce problème, ajouter un child 'Target', ou changer le mode d'affichage du targeting.");
            }
        }

        actualTarget = null;
        MessageUpdateSubscriber();
    }

    /// <summary>
    /// A appeler lors de la mort d'un ennemis par exemple. Enlève l'objet de la liste de cibles potentielles. Rafraichis le targeting.
    /// </summary>
    /// <param name="g"> GameObject à enlever de la liste.</param>
    public void RemoveFromTargetableList(GameObject g, bool byDeath)
    {
        if (targetableObjects.ContainsKey(g))
        {
            targetableObjects.Remove(g);

            if (byDeath)
            {
                SetActualIndex(false);
                SetTarget(false);
            }
            else if (actualTarget != g)
            {
                SetActualIndex(false);
            }
            else if (actualTarget == g)
            {
                ClearActualTarget();
            }
        }
    }

    #region Interface Targeting
    public void AddSubscriber(CS_I_Subscriber sub)
    {
        subscribers.Add(sub);
    }

    void MessageUpdateSubscriber()
    {
        foreach (CS_I_Subscriber subscriber in subscribers)
        {
            subscriber.UpdateTarget(actualTarget);
        }
    }
    #endregion

    public void OnTargetting(CallbackContext context)
    {
        inputState = context.ReadValueAsButton();
    }
}
