using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CS_Nail : MonoBehaviour
{
    float timebeforeCanTake = 2f;
    float timeStart;
    Color fxColor;
    VisualEffect fx;

    RaycastHit hit;

    public RaycastHit Hit { get => hit; set => hit = value; }

    private void Start()
    {
        fx = GetComponentInChildren<VisualEffect>();
        timeStart = Time.time;
         fx.SetVector4("_Color", GetPixelColor());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && timeStart + timebeforeCanTake < Time.time)
        {
            other.GetComponent<CS_F_Nail>().RetakeNail(this);
            Destroy(gameObject);
        }
    }


    public Color GetPixelColor()
    {
        // Vérifier si l'objet touché possède un MeshRenderer
        MeshRenderer meshRenderer = hit.collider.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Material[] materials = meshRenderer.sharedMaterials;

            // Assuming you have a reference to the MeshFilter component
            MeshFilter meshFilter = hit.collider.GetComponent<MeshFilter>();

            // Get the mesh object
            Mesh mesh = meshFilter.sharedMesh;

            int triangleIndex = hit.triangleIndex;

            // Get the triangle's submesh index
            int submeshIndex = GetSubMeshIndex(mesh, triangleIndex);

            // Retrieve the material assigned to the submesh
            Material triangleMaterial = materials[submeshIndex];

            return triangleMaterial.color;
        }

        return Color.black;
    }

    public int GetSubMeshIndex(Mesh mesh, int triangleIndex)
    {
        if (mesh.isReadable == false)
        {
            Debug.LogError("You need to mark model's mesh as Read/Write Enabled in Import Settings.", mesh);
            return 0;
        }

        int triangleCounter = 0;
        for (int subMeshIndex = 0; subMeshIndex < mesh.subMeshCount; subMeshIndex++)
        {
            var indexCount = mesh.GetSubMesh(subMeshIndex).indexCount;
            triangleCounter += indexCount / 3;
            if (triangleIndex < triangleCounter)
            {
                return subMeshIndex;
            }
        }

        Debug.LogError(
            $"Failed to find triangle with index {triangleIndex} in mesh '{mesh.name}'. Total triangle count: {triangleCounter}",
            mesh);
        return 0;
    }
}
