using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Enemy : MonoBehaviour
{
    [SerializeField] protected int PV = 0;
    virtual public void ShareMessage(List<CS_Enemy> ennemiesMessaged)
    {
    }

    virtual public void Push(Vector3 force)
    {
        GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
    }
}
