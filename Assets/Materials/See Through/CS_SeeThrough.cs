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

    [SerializeField] List<Material> wallMaterials01;
    [SerializeField] List<Material> wallMaterials02;
    [SerializeField] List<Material> wallMaterials03;
    [SerializeField] List<Material> wallMaterials04;
    [SerializeField] List<Material> wallMaterials05;
    [SerializeField] LayerMask mask;
    Camera cam;

    GameObject player;
    int seeThroughLevel;

    float size = 0f;
    [MinMaxSlider(0f, 50f)][SerializeField] Vector2 sizeRange;
    [MinValue(0)][SerializeField] float speed = 2;
    [CurveRange(0, 0.01f, 1, 1)] [SerializeField] AnimationCurve curveSize;

    private void Start()
    {
        cam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < wallMaterials01.Count; i++)
        {
            wallMaterials01[i].SetFloat(sizeID, sizeRange.x);
        }

        for (int i = 0; i < wallMaterials02.Count; i++)
        {
            wallMaterials02[i].SetFloat(sizeID, sizeRange.x);
        }

        for (int i = 0; i < wallMaterials03.Count; i++)
        {
            wallMaterials03[i].SetFloat(sizeID, sizeRange.x);
        }

        for (int i = 0; i < wallMaterials04.Count; i++)
        {
            wallMaterials04[i].SetFloat(sizeID, sizeRange.x);
        }

        for (int i = 0; i < wallMaterials05.Count; i++)
        {
            wallMaterials05[i].SetFloat(sizeID, sizeRange.x);
        }
    }

    void Update()
    {
        var dir = cam.transform.position - (player.transform.position + Vector3.up);
        var ray = new Ray((player.transform.position + Vector3.up), dir.normalized);

        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask))
        {
            if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "SeeThrough01")
            {
                seeThroughLevel = 1;
                UpSeeThroughSize(wallMaterials01);
            }
            else if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "SeeThrough02")
            {
                seeThroughLevel = 2;
                UpSeeThroughSize(wallMaterials01);
                UpSeeThroughSize(wallMaterials02);
            }
            else if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "SeeThrough03")
            {
                seeThroughLevel = 3;
                UpSeeThroughSize(wallMaterials01);
                UpSeeThroughSize(wallMaterials02);
                UpSeeThroughSize(wallMaterials03);
            }
            else if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "SeeThrough04")
            {
                seeThroughLevel = 4;
                UpSeeThroughSize(wallMaterials01);
                UpSeeThroughSize(wallMaterials02);
                UpSeeThroughSize(wallMaterials03);
                UpSeeThroughSize(wallMaterials04);
            }
            else if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "SeeThrough05")
            {
                seeThroughLevel = 5;
                UpSeeThroughSize(wallMaterials01);
                UpSeeThroughSize(wallMaterials02);
                UpSeeThroughSize(wallMaterials03);
                UpSeeThroughSize(wallMaterials04);
                UpSeeThroughSize(wallMaterials05);
            }
            else
            {
                if (seeThroughLevel == 1)
                {
                    DownSeeThroughSize(wallMaterials01);
                }
                else if (seeThroughLevel == 2)
                {
                    DownSeeThroughSize(wallMaterials01);
                    DownSeeThroughSize(wallMaterials02);
                }
                else if (seeThroughLevel == 3)
                {
                    DownSeeThroughSize(wallMaterials01);
                    DownSeeThroughSize(wallMaterials02);
                    DownSeeThroughSize(wallMaterials03);
                }
                else if (seeThroughLevel == 4)
                {
                    DownSeeThroughSize(wallMaterials01);
                    DownSeeThroughSize(wallMaterials02);
                    DownSeeThroughSize(wallMaterials03);
                    DownSeeThroughSize(wallMaterials04);
                }
                else
                {
                    DownSeeThroughSize(wallMaterials01);
                    DownSeeThroughSize(wallMaterials02);
                    DownSeeThroughSize(wallMaterials03);
                    DownSeeThroughSize(wallMaterials04);
                    DownSeeThroughSize(wallMaterials05);
                }
            }
        }
        else
        {
            if (seeThroughLevel == 1)
            {
                DownSeeThroughSize(wallMaterials01);
            }
            else if (seeThroughLevel == 2)
            {
                DownSeeThroughSize(wallMaterials01);
                DownSeeThroughSize(wallMaterials02);
            }
            else if (seeThroughLevel == 3)
            {
                DownSeeThroughSize(wallMaterials01);
                DownSeeThroughSize(wallMaterials02);
                DownSeeThroughSize(wallMaterials03);
            }
            else if (seeThroughLevel == 4)
            {
                DownSeeThroughSize(wallMaterials01);
                DownSeeThroughSize(wallMaterials02);
                DownSeeThroughSize(wallMaterials03);
                DownSeeThroughSize(wallMaterials04);
            }
            else
            {
                DownSeeThroughSize(wallMaterials01);
                DownSeeThroughSize(wallMaterials02);
                DownSeeThroughSize(wallMaterials03);
                DownSeeThroughSize(wallMaterials04);
                DownSeeThroughSize(wallMaterials05);
            }
        }

        var view = cam.WorldToViewportPoint(transform.position + Vector3.up);

        SetSeeThroughPos(wallMaterials01, view);
        SetSeeThroughPos(wallMaterials02, view);
        SetSeeThroughPos(wallMaterials03, view);
        SetSeeThroughPos(wallMaterials04, view);
        SetSeeThroughPos(wallMaterials05, view);
    }

    void UpSeeThroughSize(List<Material> materialList)
    {
        if (size < sizeRange.y)
        {
            size += Time.deltaTime * (speed * curveSize.Evaluate(size.Remap(sizeRange.x, sizeRange.y, 0, 1)));
            size = Mathf.Clamp(size, sizeRange.x, sizeRange.y);

            for (int i = 0; i < materialList.Count; i++)
            {
                materialList[i].SetFloat(sizeID, size);
            }
        }
    }

    void DownSeeThroughSize(List<Material> materialList)
    {
        if (size > sizeRange.x)
        {
            size -= Time.deltaTime * (speed * curveSize.Evaluate(size.Remap(sizeRange.x, sizeRange.y, 0, 1)));
            size = Mathf.Clamp(size, sizeRange.x, sizeRange.y);

            for (int i = 0; i < materialList.Count; i++)
            {
                materialList[i].SetFloat(sizeID, size);
            }
        }
    }

    void SetSeeThroughPos(List<Material> materialList, Vector4 view)
    {
        for (int i = 0; i < materialList.Count; i++)
        {
            materialList[i].SetVector(posID, view);
        }
    }
}
