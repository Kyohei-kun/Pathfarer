using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_MoveSimiliPlayer : MonoBehaviour
{
    Vector3 place;
    RaycastHit hit;
    bool rotating;

    private void Update()
    {
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            place = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);

            if(hit.transform.tag == "terrain" && Input.GetMouseButtonDown(0))
            {
                gameObject.SetActive(false);
                transform.position = place;
                rotating = true;
                gameObject.SetActive(true);
            }

            if (Input.GetMouseButtonUp(0))
            {
                rotating = false;
            }

            if(rotating)
            {
                transform.LookAt(place);
            }
        }
    }
}
