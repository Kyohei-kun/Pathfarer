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
    [Space][BoxGroup("LifeVisuel")] [SerializeField] Material mt_live1;
    [BoxGroup("LifeVisuel")] [SerializeField] Material mt_live2;
    [BoxGroup("LifeVisuel")] [SerializeField] Material mt_live3;
    [Space][BoxGroup("LifeVisuel")] [SerializeField] Color lightColor_live1;
    [BoxGroup("LifeVisuel")] [SerializeField] Color lightColor_live2;
    [BoxGroup("LifeVisuel")] [SerializeField] Color lightColor_live3;

    private float currentCooldown = 0;
    private bool isCooldown = false;
    private bool canAttack = true;

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
                canAttack = true;
                isCooldown = false;
            }
        }

        if (enemies.Count != 0)
        {
            SortEnemies();
            if (canAttack)
            {
                Shoot(enemies[0]);
                currentCooldown = 0;
                canAttack = false;
                isCooldown = true;

                eyes.SetActive(true);
                Invoke("ActivateFX", cadence / 2);
                fx_mentor.Stop();
            }
        }
    }

    private void ActivateFX()
    {
        fx_mentor.Play();
        fx_explodeMentor.Play();
        eyes.SetActive(false);
    }

    private void Shoot(CS_Enemy target)
    {
        GameObject projectil = Instantiate(pref_Projectile);
        projectil.transform.localPosition = socketShoot.position;
        projectil.transform.LookAt(target.transform.position);
        projectil.GetComponent<CS_Projectil_Mentor>().Target = target.transform;
    }

    private void SortEnemies()
    {
        foreach (CS_Enemy item in enemies.ToList()) //Clean dead IA
        {
            if(item == null)
                enemies.Remove(item);
        }
        enemies = enemies.OrderBy(go => Vector3.Distance(transform.position, go.transform.position)).ToList<CS_Enemy>();
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

    private void OnTriggerEnter(Collider other)
    {
        CS_Enemy tempEnemy = other.GetComponent<CS_Enemy>();

        if (tempEnemy != null)
        {
            enemies.Add(tempEnemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CS_Enemy tempEnemy = other.GetComponent<CS_Enemy>();

        if (tempEnemy != null && enemies.Contains(tempEnemy))
        {
            _ = enemies.Remove(tempEnemy);
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
