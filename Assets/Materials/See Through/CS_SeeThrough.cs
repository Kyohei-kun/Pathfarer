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

    [SerializeField] Material wallMaterial;
    [SerializeField] LayerMask mask;
    Camera cam;

    float size = 0f;
    [MinMaxSlider(0f, 50f)][SerializeField] Vector2 sizeRange;
    [MinValue(0)][SerializeField] float speed = 2;
    [CurveRange(0, 0.01f, 1, 1)] [SerializeField] AnimationCurve curveSize;

    private void Start()
    {
        cam = Camera.main;
        wallMaterial.SetFloat(sizeID, sizeRange.x);
    }

    void Update()
    {
        var dir = cam.transform.position - transform.position;
        var ray = new Ray(transform.position, dir.normalized);

        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask) && hit.transform.GetComponent<Renderer>().sharedMaterial == wallMaterial)
        {
            if (size < sizeRange.y)
            {
                size += Time.deltaTime * (speed * curveSize.Evaluate(size));
                size = Mathf.Clamp(size, sizeRange.x, sizeRange.y);
                wallMaterial.SetFloat(sizeID, size);
            }
        }
        else
        {
            if (size > sizeRange.x)
            {
                size -= Time.deltaTime * (speed * curveSize.Evaluate(size));
                size = Mathf.Clamp(size, sizeRange.x, sizeRange.y);
                wallMaterial.SetFloat(sizeID, size);
            }
        }

        var view = cam.WorldToViewportPoint(transform.position + Vector3.up);
        wallMaterial.SetVector(posID, view);
    }
}
