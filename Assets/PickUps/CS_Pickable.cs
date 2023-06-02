using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable : MonoBehaviour
{
    [SerializeField] bool dontMove;
    [SerializeField] bool isCapacity;

    [Header("FXs")]
    [SerializeField] GameObject pickLittleFX;
    [SerializeField] GameObject pickBigFX;

    [HideInInspector] public GameObject player;
    [HideInInspector] public CS_FeatureUnlocker scriptFeatures;

    Transform visuPickUp;
    float time = 0;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scriptFeatures = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CS_FeatureUnlocker>();
        visuPickUp = GetComponentInChildren<Transform>();
    }

    private void Update()
    {
        if (!dontMove)
        {
            time += Time.deltaTime;
            visuPickUp.SetPositionAndRotation(visuPickUp.position + (Vector3.up * Mathf.Cos(time) * Time.deltaTime / 4), Quaternion.identity);
            visuPickUp.Rotate(30 * time * Vector3.up, Space.Self);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            PickEffect();

            if (isCapacity)
            {
                scriptFeatures.ManualUpdate();
            }
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

    virtual public void PickEffect() 
    {
        PickGraph();
        Destroy(gameObject, 0.2f);
    }
}
