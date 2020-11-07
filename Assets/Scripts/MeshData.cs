using UnityEngine;

public class MeshData
{
    int width;
    int height;

    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        width = meshWidth;
        height = meshHeight;

        vertices = new Vector3[(meshWidth+1) * (meshHeight+1)];
        uvs = new Vector2[(meshWidth + 1) * (meshHeight + 1)];
        triangles = new int[meshWidth * meshHeight * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;

        triangleIndex += 3;
    }

    public void FillMeshByTriangles()
    {
        for (int y = 0, vi = 0, ti = 0; y < height; y++, vi++)
        {
            for (int x = 0; x < width; vi++, ti += 6, x++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + width + 1;
                triangles[ti + 5] = vi + width + 2;
            }
        }      
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
