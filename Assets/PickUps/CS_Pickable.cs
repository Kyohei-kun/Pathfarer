using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable : MonoBehaviour
{
    [SerializeField] bool isCapacity;

    [Header("FXs")]
    [SerializeField] GameObject pickLittleFX;
    [SerializeField] GameObject pickBigFX;

    [HideInInspector] public GameObject player;
    [HideInInspector] public CS_FeatureUnlocker scriptFeatures;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scriptFeatures = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CS_FeatureUnlocker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            PickGraph();
            PickEffect();

            Destroy(gameObject, 0);
        }
    }

    void PickGraph()
    {
        if (isCapacity)
        {
            Instantiate(pickBigFX, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(pickLittleFX, transform.position, Quaternion.identity);
        }
    }

    virtual public void PickEffect() {}
}
