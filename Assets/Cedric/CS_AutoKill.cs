using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_AutoKill : MonoBehaviour
{
    [SerializeField] float timer;
    [SerializeField] bool killParent;
    [SerializeField] bool atStart;

    private void Start()
    {
        if(atStart)
        {
            Kill(this.timer);
        }
    }

    public void Kill(float timer = 0)
    {
        this.timer = timer;
        StartCoroutine(Enumerator_Kill());
    }

    IEnumerator Enumerator_Kill()
    {
        yield return new WaitForSeconds(timer);
        if (killParent)
            Destroy(gameObject.transform.parent.gameObject);
        else
            Destroy(gameObject);
    }
}
