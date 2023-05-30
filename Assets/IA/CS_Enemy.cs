using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class CS_Enemy : MonoBehaviour
{
    protected float timeStopStunning = 0;
    protected Rigidbody _rigidbody;
    protected bool touched = false;
    protected bool isStun = false;
    protected bool lastIsStun = false;
    protected CS_Perception perceptron;
    protected Vector3 startPosition;
    protected Transform playerTransform;
    protected Animator animator;

    [SerializeField] protected int PV = 3;
    [SerializeField] protected GameObject prefab_DeathFX;

    protected virtual void Start()
    {
            _rigidbody = GetComponent<Rigidbody>();
            perceptron = GetComponent<CS_Perception>();
            startPosition = transform.position;
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            perceptron.Initialisation(playerTransform);
            animator = GetComponent<Animator>();
    }
    protected virtual void StunUpdate()
    {
        if (Time.time > timeStopStunning)
        {
            isStun = false;
        }
        else
        {
            isStun = true;
        }

        if (lastIsStun != isStun)
        {
            if (isStun) OnStartStunning();
            else OnStopStunning();
        }

        lastIsStun = isStun;
    }

    protected virtual void OnStartStunning()
    {
        animator.SetBool("Stun", true);
    }

    protected virtual void OnStopStunning()
    {
        animator.SetBool("Stun", false);
    }

    virtual public void ShareMessage(List<CS_Enemy> ennemiesMessaged) { }

    [Button]
    virtual public void Push(Vector3 force)
    {
        //GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
        //_rigidbody.AddForce(force, ForceMode.Force);

        _rigidbody.velocity = force;
    }

    virtual public void Stun(float duration)
    {
        timeStopStunning = Time.time + duration;
    }

    virtual public void TakeDamage(int damage)
    {
        touched = true;
        PV -= damage;
        Stun(0.5f);
        Push(GameObject.FindGameObjectWithTag("Player").transform.forward * 15);
        if (PV <= 0)
            Death();
    }

    virtual protected void Death()
    {
        GameObject temp = Instantiate(prefab_DeathFX);
        temp.transform.position = gameObject.transform.position;
        Destroy(gameObject);
    }
}
