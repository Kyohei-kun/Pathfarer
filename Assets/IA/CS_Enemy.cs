using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CS_F_HeavyAttack;

public class CS_Enemy : MonoBehaviour , CS_I_Attackable
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

    [SerializeField] protected float PV = 3;
    [SerializeField] protected GameObject prefab_DeathFX;

    [SerializeField] Material stunningMaterial;
    [SerializeField] Renderer currentRenderer;
    Material normalMaterial;

    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        perceptron = GetComponent<CS_Perception>();
        startPosition = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        perceptron.Initialisation(playerTransform);
        animator = GetComponent<Animator>();
        normalMaterial = currentRenderer.sharedMaterial;
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
        currentRenderer.sharedMaterial = stunningMaterial;
    }

    protected virtual void OnStopStunning()
    {
        animator.SetBool("Stun", false);
        currentRenderer.sharedMaterial = normalMaterial;
    }

    virtual public void ShareMessage(List<CS_Enemy> ennemiesMessaged) { }

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

    virtual protected void Death()
    {
        GameObject temp = Instantiate(prefab_DeathFX);
        temp.transform.position = gameObject.transform.position;
        playerTransform.GetComponent<CS_F_Targeting>().RemoveFromTargetableList(gameObject, true);
        Destroy(gameObject);
    }

    public void TakeDamage(float damage, PlayerAttackType type)
    {
        touched = true;
        PV -= damage;
        Stun(0.5f);
        switch (type)
        {
            case PlayerAttackType.Simple:
                Push(GameObject.FindGameObjectWithTag("Player").transform.forward * 15);
                break;
            case PlayerAttackType.Heavy:
                Push(GameObject.FindGameObjectWithTag("Player").transform.forward * 60);
                break;
            case PlayerAttackType.Pilon:
                Push((transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).normalized * 15);
                break;
            case PlayerAttackType.Epines:
                Push((transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).normalized * 30);
                break;
            default:
                break;
        }
        if (PV <= 0)
            Death();
    }
}
