using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TowerIA : MonoBehaviour
{
    [SerializeField] List<CS_Tower_PV> myPV = new();
    [SerializeField] GameObject deathFX;

    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void RemovePV(CS_Tower_PV pv)
    {
        myPV.Remove(pv);

        if (myPV.Count == 0)
        {
            Instantiate(deathFX, transform.position, Quaternion.identity);
            player.GetComponent<CS_F_Targeting>().RemoveFromTargetableList(gameObject, true);

            Destroy(gameObject);
        }
    }
}
