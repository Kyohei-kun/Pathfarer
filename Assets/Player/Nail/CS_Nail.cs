using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CS_Nail : MonoBehaviour
{
    float timebeforeCanTake = 2f;
    float timeStart;
    Color fxColor;
    VisualEffect fx;

    RaycastHit hit;

    [MinValue(0)][SerializeField] int nbMaxProjoTank = 3;
    int nbActualProjoTank = 0;

    public RaycastHit Hit { get => hit; set => hit = value; }

    private void Start()
    {
        fx = GetComponentInChildren<VisualEffect>();
        timeStart = Time.time;
        fx.SetVector4("_Color", CS_ColorTriangle.GetPixelColor(hit));
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
    }
}
