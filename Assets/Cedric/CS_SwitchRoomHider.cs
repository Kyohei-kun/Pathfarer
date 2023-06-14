using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class CS_SwitchRoomHider : MonoBehaviour
{
    [SerializeField] GameObject toHide;
    [SerializeField] GameObject toShow;

    List<Renderer> renderersToHide = new List<Renderer>();
    List<Renderer> renderersToShow = new List<Renderer>();

    void Start()
    {
        renderersToHide = toHide.GetComponentsInChildren<Renderer>().ToList<Renderer>();
        renderersToShow = toShow.GetComponentsInChildren<Renderer>().ToList<Renderer>();
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
