using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ProjectileNail : MonoBehaviour
{
    [MinValue(0)][SerializeField] float speed = 5;
    [MinValue(0)][SerializeField] float lifeTime = 10;
    float currentLife = 0;

    GameObject target;

    float autoTargetLerp = 0;
    [MinValue(0)][SerializeField] float autoTargetForce = 6;
    Vector3 autoTargetStart = Vector3.zero;
    Vector3 autoTargetEnd = Vector3.zero;

    [MinValue(0)][SerializeField] float damage = 2;
    [SerializeField] LayerMask enemiesMask;

    void Start()
    {
        Collider[] enearmies = Physics.OverlapSphere(transform.position, 5f, enemiesMask);
        if (enearmies.Length > 0) target = enearmies[Random.Range(0, enearmies.Length)].gameObject; else target = null;

        if (target != null) transform.LookAt(target.transform.position + Vector3.up);

        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    private void Update()
    {
        currentLife += Time.deltaTime;

        if (currentLife >= lifeTime || (target != null && Vector3.Distance(transform.position, target.transform.position) <= 0.1f))
        {
            DestroyMe();
        }

        if (target == null) return;

        autoTargetLerp += Time.deltaTime * (autoTargetForce / 100);

        autoTargetStart = transform.position + transform.forward;
        autoTargetEnd = target.transform.position;

        transform.LookAt(Vector3.Lerp(autoTargetStart, autoTargetEnd, autoTargetLerp));
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Herb")) return; // Pas les herbes

        CS_I_Attackable attackableObject;

        if (other.transform.parent != null && other.transform.parent.GetComponent<CS_I_Attackable>() != null) // Ennemy de cédric
        {
            attackableObject = other.transform.parent.GetComponent<CS_I_Attackable>();
            attackableObject.TakeDamage(damage, CS_F_HeavyAttack.PlayerAttackType.Simple);

            DestroyMe();
        }
        else if (other.GetComponent<CS_I_Attackable>() != null) // Object ou ennemy de Armelle
        {
            attackableObject = other.GetComponent<CS_I_Attackable>();
            attackableObject.TakeDamage(damage, CS_F_HeavyAttack.PlayerAttackType.Simple);

            DestroyMe();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        DestroyMe();
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
