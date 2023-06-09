using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CS_Projectil_Mentor : MonoBehaviour
{
    private float speed = 5000;
    private Transform target;
    private Rigidbody _rigidbody;
    private float lifeTime = 5;

    public Transform Target { get => target; set => target = value; }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;

        _rigidbody.velocity = transform.forward * Time.deltaTime * speed ;
        
        if (Target != null) transform.LookAt(Target);

        if (Vector3.Distance(transform.position, target.position) < 1 || lifeTime < 0 || target == null)
        {
            Destroy(gameObject);
        }
    }
}
