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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    private void Update()
    {
        currentLife += Time.deltaTime;

        if (currentLife >= lifeTime)
        {
            DestroyMe();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            player.GetComponent<CS_PlayerLife>().LoseLife();
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
