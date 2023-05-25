using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class CS_SeeThrough : MonoBehaviour
{
    public static int posID = Shader.PropertyToID("_PlayerPosition");
    public static int sizeID = Shader.PropertyToID("_Size");

    [SerializeField] List<Material> wallMaterials;
    [SerializeField] LayerMask mask;
    Camera cam;

    GameObject player;

    float size = 0f;
    [MinMaxSlider(0f, 50f)][SerializeField] Vector2 sizeRange;
    [MinValue(0)][SerializeField] float speed = 2;
    [CurveRange(0, 0.01f, 1, 1)] [SerializeField] AnimationCurve curveSize;

    private void Start()
    {
        cam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < wallMaterials.Count; i++)
        {
            wallMaterials[i].SetFloat(sizeID, sizeRange.x);
        }
    }

    void Update()
    {
        var dir = cam.transform.position - (player.transform.position + Vector3.up);
        var ray = new Ray((player.transform.position + Vector3.up), dir.normalized);

        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask) && wallMaterials.Contains(hit.transform.GetComponent<Renderer>().sharedMaterial))
        {
            if (size < sizeRange.y)
            {
                size += Time.deltaTime * (speed * curveSize.Evaluate(size.Remap(sizeRange.x, sizeRange.y, 0, 1)));
                size = Mathf.Clamp(size, sizeRange.x, sizeRange.y);

                for (int i = 0; i < wallMaterials.Count; i++)
                {
                    wallMaterials[i].SetFloat(sizeID, size);
                }
            }
        }
        else
        {
            if (size > sizeRange.x)
            {
                size -= Time.deltaTime * (speed * curveSize.Evaluate(size.Remap(sizeRange.x, sizeRange.y, 0, 1)));
                size = Mathf.Clamp(size, sizeRange.x, sizeRange.y);

                for (int i = 0; i < wallMaterials.Count; i++)
                {
                    wallMaterials[i].SetFloat(sizeID, size);
                }
            }
        }

        var view = cam.WorldToViewportPoint(transform.position + Vector3.up);

        for (int i = 0; i < wallMaterials.Count; i++)
        {
            wallMaterials[i].SetVector(posID, view);
        }
    }
}
