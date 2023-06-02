using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_PassifEpines : MonoBehaviour
{
    List<CS_Enemy> nearEnnemies = new ();
    [SerializeField] float epinesDmg = 1;

    [Button]
    public void Dmg()
    {
        for (int i = 0; i < nearEnnemies.Count; i++)
        {
            nearEnnemies[i].TakeDamage(epinesDmg, CS_F_HeavyAttack.PlayerAttackType.Simple);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CS_Enemy>())
        {
            nearEnnemies.Add (other.gameObject.GetComponent<CS_Enemy>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CS_Enemy>())
        {
            nearEnnemies.Remove(other.gameObject.GetComponent<CS_Enemy>());
        }
    }
}
