using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition.Attributes;
using UnityEngine.VFX;

public class CS_Nail : MonoBehaviour
{
    CS_FeatureUnlocker gameManager;

    [SerializeField] float timebeforeCanTake = 2f;
    float timeStart;
    VisualEffect fx;

    RaycastHit hit;

    [MinValue(0)][OnValueChanged("MajRange")][SerializeField] float rangeProjoTank = 4;
    [MinValue(0)][SerializeField] int nbMaxProjoTank = 3;
    int nbActualProjoTank = 0;

    [SerializeField] GameObject myProjo;
    [SerializeField] Material[] v1Mat = new Material[3];
    [SerializeField] Material[] v2Mat = new Material[3];
    [SerializeField] GameObject fxTir;
    bool waitForShoot;

    GameObject targetTir;
    [MinValue(0)][SerializeField] float targetingRange = 5;
    [SerializeField] LayerMask targetableMask;

    public RaycastHit Hit { get => hit; set => hit = value; }
    void MajRange() {GetComponentInChildren<SphereCollider>().radius = rangeProjoTank; }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CS_FeatureUnlocker>();

        fx = GetComponentInChildren<VisualEffect>();
        timeStart = Time.time;
        fx.SetVector4("_Color", CS_ColorTriangle.GetPixelColor(hit));

        if (gameManager.State_Nail_V2)
        {
            GetComponentInChildren<MeshRenderer>().materials = v2Mat;
        }
    }

    private void Update()
    {
        if (waitForShoot)
        {
            CheckEnemiesNear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && timeStart + timebeforeCanTake < Time.time)
        {
            other.GetComponent<CS_F_Nail>().RetakeNail(this);
            Destroy(gameObject);
        }
        else
        {
            CS_I_NailInteract interactObject = other.GetComponent<CS_I_NailInteract>();

            if (interactObject != null)
            {
                interactObject.InteractNail(this);
            }
        }
    }

    /// <summary>
    /// True si le Nail peux encore rediriger des projectiles.
    /// </summary>
    /// <returns>nbActualProjoTank plus petit (strict) nbMaxProjoTank</returns>
    public bool CanRedirect ()
    {
        return nbActualProjoTank < nbMaxProjoTank;
    }

    /// <summary>
    /// Incrémente le nombre de projectiles absorbés par le clou. (nbActualProjoTank++)
    /// </summary>
    public void AbsorbeProjo()
    {
        nbActualProjoTank++;

        if (gameManager.State_Nail_V2 && !CanRedirect())
        {
            waitForShoot = true;
        }
    }

    void CheckEnemiesNear()
    {
        Collider[] enearmies = Physics.OverlapSphere(transform.position, targetingRange, targetableMask);

        if (enearmies.Length > 0)
        {
            // Premier set pour pas de NullReference
            for (int i = 0; i < enearmies.Length; i++)
            {
                if (enearmies[i].CompareTag("Enemy"))
                {
                    targetTir = enearmies[i].gameObject;
                }
                else if (enearmies[i].transform.parent && enearmies[i].transform.parent.CompareTag("Enemy"))
                {
                    targetTir = enearmies[i].transform.parent.gameObject;
                }
            }

            // Si aucun ennemis dans les colliders
            if (targetTir == null) return;

            // Vrai set avec distance
            for (int i = 0; i < enearmies.Length; i++)
            {
                float distNearest = Vector3.Distance(targetTir.transform.position, transform.position);
                float distList = Vector3.Distance(enearmies[i].transform.position, transform.position);

                if (distList < distNearest && enearmies[i].CompareTag("Enemy"))
                {
                    targetTir = enearmies[i].gameObject;
                }
                else if (distList < distNearest && enearmies[i].transform.parent && enearmies[i].transform.parent.CompareTag("Enemy"))
                {
                    targetTir = enearmies[i].transform.parent.gameObject;
                }
            }
            
            Shoot();
        }
        else targetTir = null;
    }

    void Shoot()
    {
        GameObject projo = Instantiate(myProjo, transform.position, Quaternion.identity);
        GetComponentInChildren<MeshRenderer>().materials = v1Mat;
        Instantiate(fxTir, transform.position, Quaternion.identity);

        projo.GetComponent<CS_ProjectileNail>().StartProjoNail(targetTir);

        targetTir = null;
        waitForShoot = false;
    }
}