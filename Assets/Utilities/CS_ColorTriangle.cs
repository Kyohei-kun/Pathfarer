using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CS_ColorTriangle
{
    public static Color GetPixelColor(RaycastHit hit)
    {
        MeshRenderer meshRenderer = hit.collider.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Material[] materials = meshRenderer.sharedMaterials;

            MeshFilter meshFilter = hit.collider.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {

                Mesh mesh = meshFilter.sharedMesh;

                int triangleIndex = hit.triangleIndex;

                int submeshIndex = GetSubMeshIndex(mesh, triangleIndex);

                Material triangleMaterial = materials[submeshIndex];

                return triangleMaterial.color;
            }
            else
                return Color.black;
        }

        return Color.black;
    }

    private static int GetSubMeshIndex(Mesh mesh, int triangleIndex)
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
