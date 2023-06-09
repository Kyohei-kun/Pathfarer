using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ProjectileTower : MonoBehaviour
{
    [MinValue(0)][SerializeField] float speed = 5;
    [MinValue(0)][SerializeField] float lifeTime = 5;
    float currentLife = 0;

    GameObject player;
    GameObject target;

    float autoTargetLerp = 0;
    [MinValue(0)][SerializeField] float autoTargetForce = 1;
    Vector3 autoTargetStart = Vector3.zero;
    Vector3 autoTargetEnd = Vector3.zero;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player;
        transform.LookAt(target.transform.position + Vector3.up);

        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    private void Update()
    {
        currentLife += Time.deltaTime;

        if (currentLife >= lifeTime || Vector3.Distance(transform.position, target.transform.position) <= 0.1f)
        {
            DestroyMe();
        }

        if (target != player) // Pour un auto target constant enlever le if()
        {
            autoTargetLerp += Time.deltaTime * (autoTargetForce / 100);

            if (target != player) autoTargetStart = transform.position + transform.forward; else autoTargetStart = transform.position;
            if (target != player) autoTargetEnd = target.transform.position; else autoTargetEnd = target.transform.position + Vector3.up;

            transform.LookAt(Vector3.Lerp(autoTargetStart, autoTargetEnd, autoTargetLerp));
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
        }
    }

    void ChangeTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            player.GetComponent<CS_PlayerLife>().LoseLife();
        }
        else if (other.gameObject.name.Contains("Vacuum"))
        {
            ChangeTarget(other.gameObject);
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
