using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class CS_PerceptionCone : CS_Perception
{
    //Gizmo parameter
    float timerShowTrigger = 5;
    float currentimerShowTrigger;
    bool showPermanentGizmo;

    //Parameters
    [OnValueChanged("OnTriggerChange")][SerializeField] float radiusCone = 3;
    [OnValueChanged("OnTriggerChange")][Range(0.0f, 180f)][SerializeField] float angleCone = 45;

    bool playerIsInTriger;

    private void Update()
    {
        if (playerIsInTriger)
        {
            //Debug.DrawRay(transform.position, IAtoPlayer*10, Color.yellow, 5);
            //Debug.DrawRay(transform.position, transform.forward * 10, Color.magenta, 5);

            Vector3 IAtoPlayer = Vector3.ProjectOnPlane((playerTransform.position - transform.position).normalized, Vector3.up);

            if (Vector3.Angle(IAtoPlayer, transform.forward)*2 < angleCone || Vector3.Distance(transform.position, playerTransform.position) < 1.3f)
                playerIsVisible = true;
            else
                playerIsVisible = false;
        }
        else
            playerIsVisible = false;

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            playerIsInTriger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsInTriger = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (currentimerShowTrigger < timerShowTrigger || showPermanentGizmo)
        {
            currentimerShowTrigger += Time.deltaTime;
            if (playerIsVisible)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.blue;

            for (int i = 0; i < 10; i++)
            {
                GizmosExtensions.DrawWireArc(transform.position + ((Vector3.up * 0.2f) * i), transform.forward, angleCone, radiusCone);
            }
        }
    }

   [Button]
    private void ShowGizmos() {showPermanentGizmo = !showPermanentGizmo;} 

    private void OnTriggerChange()
    {
        currentimerShowTrigger = 0;
        try
        {
            GetComponent<SphereCollider>().radius = radiusCone;
        }
        catch (System.Exception)
        {
        }
    }
}
