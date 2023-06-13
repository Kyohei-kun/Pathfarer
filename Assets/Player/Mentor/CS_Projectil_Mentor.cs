using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CS_Projectil_Mentor : MonoBehaviour
{
    [MinValue(0)][SerializeField] float speed = 50;
    [MinValue(0)][SerializeField] float lifeTime = 10;
    float currentLife = 0;

    GameObject target;
    public GameObject Target { get => target; set => target = value; }

    //float autoTargetLerp = 0;
    //[MinValue(0)][SerializeField] float autoTargetForce = 6;
    //Vector3 autoTargetStart = Vector3.zero;
    //Vector3 autoTargetEnd = Vector3.zero;

    [MinValue(0)][SerializeField] float targetingRange = 5;

    [MinValue(0)][SerializeField] float damage = 2;
    [SerializeField] LayerMask targetableMask;

    bool startEnd;


    /// <summary>
    /// Initialise les valeurs du projectile.
    /// </summary>
    /// <param name="cible">Cible du projectile. Annule l'initialisation si null.</param>
    public void StartProjoMentor(GameObject cible)
    {
        if (cible == null) { Debug.LogError($"La target de {gameObject.name} est null !"); return; }

        target = cible;
        transform.LookAt(target.transform.position + (Vector3.up * 0.1f));

        GetComponent<Rigidbody>().velocity = transform.forward * speed;

        startEnd = true;
    }

    private void Update()
    {
        if (!startEnd) return;

        currentLife += Time.deltaTime;

        if (currentLife >= lifeTime || (target != null && Vector3.Distance(transform.position, target.transform.position) <= 0.1f))
        {
            DestroyMe();
        }

        if (target == null) return;

        //autoTargetLerp += Time.deltaTime * (autoTargetForce / 100);

        //autoTargetStart = transform.position + transform.forward;
        //autoTargetEnd = target.transform.position;

        //transform.LookAt(Vector3.Lerp(autoTargetStart, autoTargetEnd, autoTargetLerp));
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!startEnd) return;
        if (other.gameObject.name.Contains("Herb")) return; // Pas les herbes

        CS_Enemy potentielleCible;

        if (other.transform.parent != null && other.transform.parent.GetComponent<CS_Enemy>() != null) // Ennemy de cédric
        {
            potentielleCible = other.transform.parent.GetComponent<CS_Enemy>();
            potentielleCible.FreezeMentor();

            DestroyMe();
        }
        else if (other.GetComponent<CS_Enemy>() != null) // Object ou ennemy de Armelle
        {
            potentielleCible = other.GetComponent<CS_Enemy>();
            potentielleCible.FreezeMentor();

            DestroyMe();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!startEnd) return;
        DestroyMe();
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
