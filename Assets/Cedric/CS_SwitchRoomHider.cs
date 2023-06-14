using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class CS_SwitchRoomHider : MonoBehaviour
{
    [SerializeField] List<GameObject> toHide = new List<GameObject>();
    [SerializeField] List<GameObject> toShow = new List<GameObject>();

    List<Renderer> renderersToHide = new List<Renderer>();
    List<Renderer> renderersToShow = new List<Renderer>();

    void Start()
    {
        foreach (var item in toHide)
        {
            renderersToHide.AddRange(item.GetComponentsInChildren<Renderer>().ToList<Renderer>());
        }
        foreach (var item in toShow)
        {
            renderersToShow.AddRange(item.GetComponentsInChildren<Renderer>().ToList<Renderer>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            foreach (Renderer r in renderersToHide)
            {
                r.enabled = false;
            }
            foreach (Renderer r in renderersToShow)
            {
                r.enabled = true;
            }
        }
    }
}
