using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class CS_Mentor : MonoBehaviour
{
    [HideIf("drawDebugGizmo")][Button] private void DrawGizmo() { drawDebugGizmo = true; }
    [ShowIf("drawDebugGizmo")][Button] private void HideGizmo() { drawDebugGizmo = false; }
    bool drawDebugGizmo;

    [ReorderableList][SerializeField] List<CS_Enemy> enemies = new();

    [BoxGroup("Parameters")][SerializeField] float cadence;
    [BoxGroup("Parameters")][SerializeField] Transform socketShoot;
    [BoxGroup("Parameters")][SerializeField] GameObject pref_Projectile;

    [BoxGroup("Visuel")][SerializeField] VisualEffect fx_mentor;
    [BoxGroup("Visuel")][SerializeField] VisualEffect fx_explodeMentor;
    [BoxGroup("Visuel")][SerializeField] GameObject eyes;

    private float currentCooldown = 0;
    private bool isCooldown = false;
    private bool canAttack = true;

    private void Start()
    {
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
        enemies = enemies.OrderBy(go => Vector3.Distance(transform.position, go.transform.position)).ToList<CS_Enemy>();
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
