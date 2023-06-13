using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class CS_F_Mentor : MonoBehaviour
{
    [HideIf("drawDebugGizmo")] [Button] private void DrawGizmo() { drawDebugGizmo = true; }
    [ShowIf("drawDebugGizmo")] [Button] private void HideGizmo() { drawDebugGizmo = false; }
    bool drawDebugGizmo;

    [ReorderableList] [SerializeField] List<CS_Enemy> enemies = new();
    GameObject targetTir;
    [MinValue(0)][SerializeField] float targetingRange = 10;
    [SerializeField] LayerMask targetableMask;

    [BoxGroup("Parameters")] [SerializeField] float cadence;
    [BoxGroup("Parameters")] [SerializeField] Transform socketShoot;
    [BoxGroup("Parameters")] [SerializeField] GameObject pref_Projectile;

    [BoxGroup("FeedBack")] [SerializeField] VisualEffect fx_mentor;
    [BoxGroup("FeedBack")] [SerializeField] VisualEffect fx_explodeMentor;
    [BoxGroup("FeedBack")] [SerializeField] GameObject eyes;
    [BoxGroup("FeedBack")] [SerializeField] GameObject bodySphere;
    [BoxGroup("FeedBack")] [SerializeField] Light light;

    [BoxGroup("LifeVisuel")] [SerializeField] Gradient gradient_live1;
    [BoxGroup("LifeVisuel")] [SerializeField] Gradient gradient_live2;
    [BoxGroup("LifeVisuel")] [SerializeField] Gradient gradient_live3;
    [Space]
    [BoxGroup("LifeVisuel")] [SerializeField] Material mt_live1;
    [BoxGroup("LifeVisuel")] [SerializeField] Material mt_live2;
    [BoxGroup("LifeVisuel")] [SerializeField] Material mt_live3;
    [Space]
    [BoxGroup("LifeVisuel")] [SerializeField] Color lightColor_live1;
    [BoxGroup("LifeVisuel")] [SerializeField] Color lightColor_live2;
    [BoxGroup("LifeVisuel")] [SerializeField] Color lightColor_live3;

    private float currentCooldown = 0;
    private bool isCooldown = true;

    private void Start()
    {
        fx_mentor.SetGradient("_Gradient", gradient_live3);

        fx_mentor.Play();
        eyes.SetActive(false);
    }

    private void Update()
    {
        if (isCooldown)
        {
            currentCooldown += Time.deltaTime;

            if (currentCooldown >= cadence)
            {
                isCooldown = false;
            }
        }
        else
        {
            CheckEnemiesNear();
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

    private void Shoot()
    {
        GameObject projo = Instantiate(pref_Projectile, transform.position, Quaternion.identity);
        projo.GetComponent<CS_Projectil_Mentor>().StartProjoMentor(targetTir);

        currentCooldown = 0;
        isCooldown = true;

        eyes.SetActive(true);
        Invoke("ActivateFX", cadence / 2);
        fx_mentor.Stop();
    }

    private void ActivateFX()
    {
        fx_mentor.Play();
        fx_explodeMentor.Play();
        eyes.SetActive(false);
    }

    /// <summary>
    /// Update fx, fx_Explode, light, material
    /// </summary>
    /// <param name="newLife"></param>
    public void UpdateVisuel_Life(int newLife)
    {
        switch (newLife)
        {
            case 1:
                fx_mentor.SetGradient("_Gradient", gradient_live1);
                fx_explodeMentor.SetGradient("_Gradient", gradient_live1);
                bodySphere.GetComponent<Renderer>().material = mt_live1;
                light.color = lightColor_live1;
                break;
            case 2:
                fx_mentor.SetGradient("_Gradient", gradient_live2);
                fx_explodeMentor.SetGradient("_Gradient", gradient_live2);
                bodySphere.GetComponent<Renderer>().material = mt_live2;
                light.color = lightColor_live2;
                break;
            case 3:
                fx_mentor.SetGradient("_Gradient", gradient_live3);
                fx_explodeMentor.SetGradient("_Gradient", gradient_live3);
                bodySphere.GetComponent<Renderer>().material = mt_live3;
                light.color = lightColor_live3;
                break;
            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (drawDebugGizmo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>().radius);

            for (int i = 0; i < enemies.Count; i++)
            {
                if (i == 0)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.white;

                Gizmos.DrawWireCube(enemies[i].transform.position, Vector3.one);
            }
        }
    }
}
