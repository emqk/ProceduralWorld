using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class GeneratedMesh : MonoBehaviour
{
    public enum MeshType
    {
        Plane, Cube, Cylinder, Sphere, Cone
    }
    public MeshType currentMeshType;

    public int meshWidth = 0, meshHeight = 0;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    public void Setup()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter.mesh = new Mesh();
    }

     /* private void Start()
      {
          meshFilter.mesh = new Mesh();

          //meshFilter.mesh.RecalculateNormals();

          GenerateMesh(currentMeshType, meshWidth, meshHeight);
          VerySlowlyConvertToFlatShading();
        gameObject.AddComponent<MeshCollider>().convex = true;

          //TerrainGenerator.instance.GenerateObstacles(gameObject);
      }*/

    public void VerySlowlyConvertToFlatShading()
    {
        //Debug.Log("!Warning! I am really slow, please do something with me !Warning!");
        Vector3[] oldVerts = meshFilter.mesh.vertices;
        int[] triangles = meshFilter.mesh.triangles;
        Vector3[] vertices = new Vector3[triangles.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            vertices[i] = oldVerts[triangles[i]];
            triangles[i] = i;
        }
        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.triangles = triangles;
        meshFilter.mesh.RecalculateNormals();
    }

    public static GameObject CombineMeshes(Transform parent, MeshFilter[] targets)
    {
        //posBefore is here because during merge objects everything must be on pos(0,0,0)
        Vector3 posBefore = parent.position;
        parent.position = new Vector3(0, 0, 0);

        MeshFilter[] meshFilters = targets;

        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int count = meshFilters.Length;
        for (int i = 0; i < count; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            Destroy(meshFilters[i].gameObject);
            //meshFilters[i].gameObject.SetActive(false);
        }

        //Debug.Log("CombinedMeshes count: " + combine.Length);

        GameObject mergedFood = new GameObject("Merged");
        mergedFood.transform.SetParent(parent);
        mergedFood.transform.localPosition = new Vector3(0, 0, 0);
        mergedFood.AddComponent<MeshFilter>();
        mergedFood.AddComponent<MeshRenderer>();
        mergedFood.GetComponent<Renderer>().materials = targets[0].GetComponent<Renderer>().materials;

        mergedFood.GetComponent<MeshFilter>().mesh = new Mesh();
        mergedFood.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true, true);

        parent.position = posBefore;

        return mergedFood;
    }

    public static GameObject CombineMeshesManyMats(Transform parent, MeshFilter[] targets)
    {
        //posBefore is here because during merge objects everything must be on pos(0,0,0)
        Vector3 posBefore = parent.position;
        parent.position = new Vector3(0, 0, 0);

        MeshFilter[] meshFilters = targets;

        List<Material> materials = new List<Material>(1);

        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int count = meshFilters.Length;
        for (int i = 0; i < count; i++)
        {
            if (!materials.Contains(targets[i].GetComponent<Renderer>().material))
            {
                materials.Add(targets[i].GetComponent<Renderer>().material);
            }

            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            Destroy(meshFilters[i].gameObject);
            //meshFilters[i].gameObject.SetActive(false);
        }

        //Debug.Log("CombinedMeshes count: " + combine.Length);

        GameObject mergedFood = new GameObject("Merged");
        mergedFood.transform.SetParent(parent);
        mergedFood.transform.localPosition = new Vector3(0, 0, 0);
        mergedFood.AddComponent<MeshFilter>();
        mergedFood.AddComponent<MeshRenderer>();
        //mergedFood.GetComponent<Renderer>().materials = targets[0].GetComponent<Renderer>().materials;
        mergedFood.GetComponent<Renderer>().materials = materials.ToArray();

        mergedFood.GetComponent<MeshFilter>().mesh = new Mesh();
        mergedFood.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, false, true);

        parent.position = posBefore;

        return mergedFood;
    }


    /*
        void Update()
        {
            GenerateMesh(currentMeshType, meshWidth, meshHeight);
            meshFilter.mesh.RecalculateNormals();
            Vector3[] oldVerts = meshFilter.mesh.vertices;
            int[] triangles = meshFilter.mesh.triangles;
            Vector3[] vertices = new Vector3[triangles.Length];
            for (int i = 0; i < triangles.Length; i++)
            {
                vertices[i] = oldVerts[triangles[i]];
                triangles[i] = i;
            }
            meshFilter.mesh.vertices = vertices;
            meshFilter.mesh.triangles = triangles;
            Debug.Log("Verts count: " + meshFilter.mesh.vertices.Length);
            Debug.Log("Tris count: " + meshFilter.mesh.triangles.Length);
            Debug.Log("Normals count: " + meshFilter.mesh.normals.Length);
            //meshFilter.mesh.RecalculateBounds();
            //meshFilter.mesh.RecalculateNormals();
            // GenerateMesh(currentMeshType, meshWidth, meshHeight);
            //GenerateTestMesh();
        }
        */

    /*private void OnDrawGizmos()
    {
          int count = meshFilter.mesh.vertexCount;
          for (int i = 0; i < count; i++)
          {
              Gizmos.color = Color.yellow;
              Gizmos.DrawSphere(meshFilter.mesh.vertices[i] + transform.position, 0.1f);
          }
    }*/

    //Mono
    /* void FillMeshByTriangles(int width, int height)
     {
         int[] tri = new int[width * height * 6];

         for (int y = 0, vi = 0, ti = 0; y < height; y++, vi++)
         {
             for (int x = 0; x < width; vi++, ti += 6, x++)
             {
                 tri[ti] = vi;
                 tri[ti + 3] = tri[ti + 2] = vi + 1;
                 tri[ti + 4] = tri[ti + 1] = vi + width + 1;
                 tri[ti + 5] = vi + width + 2;
             }
         }

         meshFilter.mesh.triangles = tri;
     }*/
    //Mono
    /*  public void GenerateTestMesh()
      {
          int w = 23;
          int h = 7;
          //MeshData meshData = new MeshData(w, h);

          // Jobs //
          GenerateMesh(currentMeshType, w, h);
          FillCylinderMeshTrianglesJobs(w, h);
          ///////////////////

          // All underneath is for mono, not necessary for jobs
         int vertexIndex = 0;
          Vector3[] verts = new Vector3[(w+1) * (h+1)];
          //meshFilter.mesh.vertices = new Vector3[(w+1) * (h+1)];

          //verts = GenerateMeshVertsTestJobs(w, h);
            for (int y = 0; y <= h; y++)
            {
                for (int x = 0; x <= w; x++, vertexIndex++)
                {
                  //verts[vertexIndex] = new Vector3(x, y, 0f);
                  // meshData.uvs[vertexIndex] = new Vector2(x / (float)w, y / (float)h);
              }
              //  }
              //meshData.FillMeshByTriangles();
              //meshData.AddTriangle(0, 6, 1);
              //meshData.AddTriangle(1, 6, 7);

              // return meshData;
          }
          meshFilter.mesh.vertices = verts;
          FillMeshByTriangles(w, h);
      }*/

    public virtual GameObject CreateLODObject(GameObject target, Transform parent, int LOD_Level, Mesh parentMesh)
    {
        Debug.LogError("There is no implementation for getting LOD object from this object!", gameObject);
        return null;
    }

    /*void GenerateMesh(MeshType meshType, int width, int height)
    {
        switch (meshType)
        {
            case MeshType.Plane:
                GeneratePlane(width, height);
                break;
            case MeshType.Cylinder:
                GenerateCylinder(width, height, 1f);
                break;
            case MeshType.Sphere:
                GenerateIcoSphere();
                break;
            default:
                return;
                break;
        }
    }*/

    void GetMeshWidthAndHeight(int width, int height)
    {
        meshWidth = width;
        meshHeight = height;
    }

    #region Sphere

    protected void GenerateIcoSphere(int recursionLevel = 2, float radius = 1f)
    {
        MeshFilter filter = meshFilter;
        Mesh mesh = filter.mesh;
        mesh.Clear();

        List<Vector3> vertList = new List<Vector3>();
        Dictionary<long, int> middlePointIndexCache = new Dictionary<long, int>();

        // create 12 vertices of a icosahedron
        float t = (1f + Mathf.Sqrt(5f)) / 2f;

        vertList.Add(new Vector3(-1f, t, 0f).normalized * radius);
        vertList.Add(new Vector3(1f, t, 0f).normalized * radius);
        vertList.Add(new Vector3(-1f, -t, 0f).normalized * radius);
        vertList.Add(new Vector3(1f, -t, 0f).normalized * radius);

        vertList.Add(new Vector3(0f, -1f, t).normalized * radius);
        vertList.Add(new Vector3(0f, 1f, t).normalized * radius);
        vertList.Add(new Vector3(0f, -1f, -t).normalized * radius);
        vertList.Add(new Vector3(0f, 1f, -t).normalized * radius);

        vertList.Add(new Vector3(t, 0f, -1f).normalized * radius);
        vertList.Add(new Vector3(t, 0f, 1f).normalized * radius);
        vertList.Add(new Vector3(-t, 0f, -1f).normalized * radius);
        vertList.Add(new Vector3(-t, 0f, 1f).normalized * radius);


        // create 20 triangles of the icosahedron
        List<TriangleIndices> faces = new List<TriangleIndices>
        {

            // 5 faces around point 0
            new TriangleIndices(0, 11, 5),
            new TriangleIndices(0, 5, 1),
            new TriangleIndices(0, 1, 7),
            new TriangleIndices(0, 7, 10),
            new TriangleIndices(0, 10, 11),

            // 5 adjacent faces 
            new TriangleIndices(1, 5, 9),
            new TriangleIndices(5, 11, 4),
            new TriangleIndices(11, 10, 2),
            new TriangleIndices(10, 7, 6),
            new TriangleIndices(7, 1, 8),

            // 5 faces around point 3
            new TriangleIndices(3, 9, 4),
            new TriangleIndices(3, 4, 2),
            new TriangleIndices(3, 2, 6),
            new TriangleIndices(3, 6, 8),
            new TriangleIndices(3, 8, 9),

            // 5 adjacent faces 
            new TriangleIndices(4, 9, 5),
            new TriangleIndices(2, 4, 11),
            new TriangleIndices(6, 2, 10),
            new TriangleIndices(8, 6, 7),
            new TriangleIndices(9, 8, 1)
        };


        // refine triangles
        for (int i = 0; i < recursionLevel; i++)
        {
            List<TriangleIndices> faces2 = new List<TriangleIndices>();
            foreach (var tri in faces)
            {
                // replace triangle by 4 triangles
                int a = getMiddlePoint(tri.v1, tri.v2, ref vertList, ref middlePointIndexCache, radius);
                int b = getMiddlePoint(tri.v2, tri.v3, ref vertList, ref middlePointIndexCache, radius);
                int c = getMiddlePoint(tri.v3, tri.v1, ref vertList, ref middlePointIndexCache, radius);

                faces2.Add(new TriangleIndices(tri.v1, a, c));
                faces2.Add(new TriangleIndices(tri.v2, b, a));
                faces2.Add(new TriangleIndices(tri.v3, c, b));
                faces2.Add(new TriangleIndices(a, b, c));
            }
            faces = faces2;
        }

        //Make some noise
        int seed = Random.Range(-100000, 100000);
        for (int i = 0; i < vertList.Count; i++)
        {
            //noise
            //Unity.Mathematics.float2 fz = new Unity.Mathematics.float2 { x = index + seed / 150f + seed, y = index + seed / 150f };
            //float posZOffset = Unity.Mathematics.noise.cnoise(fz) * 0.33f;

            float noiseStrength = 0.1f;
            vertList[i] = vertList[i] + new Vector3(Random.Range(-noiseStrength, noiseStrength), Random.Range(-noiseStrength, noiseStrength), Random.Range(-noiseStrength, noiseStrength));
        }
        mesh.vertices = vertList.ToArray();

        List<int> triList = new List<int>();
        for (int i = 0; i < faces.Count; i++)
        {
            triList.Add(faces[i].v1);
            triList.Add(faces[i].v2);
            triList.Add(faces[i].v3);
        }
        mesh.triangles = triList.ToArray();

        // mesh.uv = new Vector2[vertices.Length];

        /* Vector3[] normales = new Vector3[vertList.Count];
         for (int i = 0; i < normales.Length; i++)
             normales[i] = vertList[i].normalized;*/


        //mesh.normals = normales;

        //mesh.RecalculateBounds();
        //mesh.Optimize();
    }

    private struct TriangleIndices
    {
        public int v1;
        public int v2;
        public int v3;

        public TriangleIndices(int v1, int v2, int v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }
    }

    // return index of point in the middle of p1 and p2
    int getMiddlePoint(int p1, int p2, ref List<Vector3> vertices, ref Dictionary<long, int> cache, float radius)
    {
        // first check if we have it already
        bool firstIsSmaller = p1 < p2;
        long smallerIndex = firstIsSmaller ? p1 : p2;
        long greaterIndex = firstIsSmaller ? p2 : p1;
        long key = (smallerIndex << 32) + greaterIndex;

        int ret;
        if (cache.TryGetValue(key, out ret))
        {
            return ret;
        }

        // not in cache, calculate it
        Vector3 point1 = vertices[p1];
        Vector3 point2 = vertices[p2];
        Vector3 middle = new Vector3
        (
            (point1.x + point2.x) / 2f,
            (point1.y + point2.y) / 2f,
            (point1.z + point2.z) / 2f
        );

        // add vertex makes sure point is on unit sphere
        int i = vertices.Count;
        vertices.Add(middle.normalized * radius);

        // store it, return index
        cache.Add(key, i);

        return i;
    }

    #endregion

    #region Cylinder
    protected void GenerateCylinder(int width, int height, float radius, float segmentHeight, Mesh parentMesh, float thinMultiplierMin = 0.85f, float thinMultiplierMax = 0.97f)
    {
        //width = 18;
        //height = 5;
        int startLoopJ = 0;
        if (parentMesh == null)
        {
            GenerateCylinderMeshVertsByJobs(width, height, radius, segmentHeight, thinMultiplierMin, thinMultiplierMax);
        }
        else
        {
            if (width % 2 == 1)
                startLoopJ = 1;

            width /= 2;
            //Debug.Log("GENERATING LOD FROM PARENT MESH");
            int fixedWidth = width + 1;
            int fixedHeight = height + 1;
            Vector3[] nArr = new Vector3[fixedWidth * fixedHeight];
            for (int i = 0, j = startLoopJ; i < nArr.Length - height; i++, j += 2)
            {
                if (startLoopJ == 1)
                {
                    if (i % fixedWidth == 0 && i != 0)
                    {
                        j++;
                    }
                }
                nArr[i] = parentMesh.vertices[j];
            }

            meshFilter.mesh.vertices = nArr;
        }

        FillCylinderMeshTrianglesByJobs(width, height);
        meshFilter.mesh.RecalculateNormals();

        GetMeshWidthAndHeight(width, height);
    }

    void FillCylinderMeshTrianglesByJobs(int w, int h)
    {
        NativeArray<int> trianglesNative = new NativeArray<int>((w * h + w-1 /*+*//*for top and bottom*//*w-2*//**/) * 6, Allocator.TempJob);

        var job = new FillCylinderMeshTrianglesJob
        {
            width = w,
            height = h,
            tri = trianglesNative
        };

        var jobHandle = job.Schedule();
        jobHandle.Complete();

        //vertsNative.CopyTo(verts);
        meshFilter.mesh.triangles = trianglesNative.ToArray();
        trianglesNative.Dispose();
    }
    [Unity.Burst.BurstCompile]
    struct FillCylinderMeshTrianglesJob : IJob
    {
        [ReadOnly]
        public int width;
        [ReadOnly]
        public int height;
        public NativeArray<int> tri;

        public void Execute()
        {
            int ti = 0, vi = 0, y = 0, x = 0;
            for (; y < height; y++, vi+= 1)
            {
                for (; x < width; vi+= 1, ti += 6, x++)
                {
                    if (x < width - 1)
                    {
                        tri[ti] = vi;
                        tri[ti + 1] = vi + width + 1;
                        tri[ti + 2] = vi + 1;

                        tri[ti + 3] = vi + width;
                        tri[ti + 4] = vi + width + 1;
                        tri[ti + 5] = vi;
                    }
                    else
                    {
                        tri[ti] = vi;
                        tri[ti + 1] = vi + 1;
                        tri[ti + 2] = vi - width + 1;

                        tri[ti + 3] = vi + width;
                        tri[ti + 4] = vi + 1;
                        tri[ti + 5] = vi - (width - 1) + width - 1;/*width-1*/;
                    }
                }
                x = 0;
                vi--;
            }

            int vecOffset = 0;
            int countFixR = 0;
            int countFixL = 0;
            if (width % 4 != 0)
            {
                if (width % 2 == 0)
                {
                    countFixR = 1;
                    countFixL = 1;
                }
                else
                {
                    countFixR = 1;
                    countFixL = 1;
                    vi++;
                }
            }

            int startI = width % 2 == 0 ? 1 : 0;
            int ti_startUp = ti;
            int su = vi + startI;
            int vi_startUp = vi + startI;
                vi+=startI;

          
            //Right side work good!   
            for (int i = 0; i < (width/2/2-1+countFixR); i++, vecOffset++, ti+=6)
            {
                tri[ti] = vi + (width / 2 - 1) - vecOffset;
                tri[ti + 1] = vi + vecOffset;
                tri[ti + 2] = vi - 1 + vecOffset;
                tri[ti + 3] = vi + (width / 2 - 1) - vecOffset;
                tri[ti + 4] = vi + (width / 2 - 1) - 1 - vecOffset;
                tri[ti + 5] = vi + vecOffset;
            }
            if (countFixR == 0)
            {
                tri[ti] = vi + (width / 2 - 1) - vecOffset;
                tri[ti + 1] = vi + vecOffset;
                tri[ti + 2] = vi - 1 + vecOffset;
                ti += 3;
            }
     
            //LeftSize
            //Center
            tri[ti] = vi + (width / 2) - 1;
            tri[ti + 1] = vi - 1;
            tri[ti + 2] = vi + width/2;
            tri[ti + 3] = vi-1;
            tri[ti + 4] = vi + width - 2;
            tri[ti + 5] = vi + (width / 2);
            ti +=6;

            vecOffset = 1;
            for (int i = 0; i < (width / 2 / 2 - 1 - 1 + countFixL/*another -1 because it can not start from 0, also that si why vecOffset is 1 at the beginning*/); i++, vecOffset++, ti += 6)
            {
                tri[ti] = vi + (width / 2 - 1) + vecOffset;
                tri[ti + 1] = vi - vecOffset - 1 + width;
                tri[ti + 2] = vi + width - vecOffset - 2;

                tri[ti + 3] = vi + (width / 2 - 1) + vecOffset;
                tri[ti + 4] = vi + width - vecOffset - 2;
                tri[ti + 5] = vi + (width / 2 - 1) + vecOffset + 1;
            }

            //Fill last triangle if there is still space for it
            if (ti + 2 < tri.Length)
            {
                tri[ti] = vi + (width / 2 - 1) + vecOffset;
                tri[ti + 1] = vi - vecOffset - 1 + width;
                tri[ti + 2] = vi + width - vecOffset - 2;
            }
        }
    }

    void GenerateCylinderMeshVertsByJobs(int width, int height, float radius, float segmentHeight, float thinMultiplierMin, float thinMultiplierMax)
    {
        int fixedWidth = width + 1;
        int fixedHeight = height + 1;

        NativeArray<Vector3> vertsNative = new NativeArray<Vector3>(fixedWidth * fixedHeight, Allocator.TempJob);
        NativeArray<float> randHeights = new NativeArray<float>(vertsNative.Length, Allocator.TempJob);
         NativeArray<Vector3> randCurves = new NativeArray<Vector3>(vertsNative.Length, Allocator.TempJob);
        float mult = (int)radius;
        for (int i = 0; i < randHeights.Length; i++)
        {
            randHeights[i] = mult;
            mult *= Random.Range(thinMultiplierMin, thinMultiplierMax);
            randCurves[i] = new Vector3(Random.Range(-0.4f, 0.4f), 0f, Random.Range(-0.4f, 0.4f));
        }

        var job = new GenerateVertsForCylinderJob
        {
            width = fixedWidth,
            randomHeights = randHeights,
            curves = randCurves,
            radius = radius,
            segmentHeight = segmentHeight,
            seed = Random.Range(-100000, 100000),
            vertices = vertsNative,
        };
        
        var jobHandle = job.Schedule(vertsNative.Length, 25);
        jobHandle.Complete();

        //vertsNative.CopyTo(verts);
        meshFilter.mesh.vertices = vertsNative.ToArray();
           vertsNative.Dispose();
        randHeights.Dispose();
        randCurves.Dispose();

    }
    [Unity.Burst.BurstCompile]
    struct GenerateVertsForCylinderJob : IJobParallelFor
    {
        [ReadOnly]
        public int width;
        [ReadOnly]
        public NativeArray<float> randomHeights;
        [ReadOnly]
        public NativeArray<Vector3> curves;
        [ReadOnly]
        public float radius;
        [ReadOnly]
        public float segmentHeight;
        [ReadOnly]
        public float seed;
        [WriteOnly]
        public NativeArray<Vector3> vertices;

        public void Execute(int index)
        {
            int posY = index / (width - 1);
            radius = randomHeights[posY];
            float posX = radius * Mathf.Sin((2 * Mathf.PI * index) / (width - 1));
            float posZ = radius * - Mathf.Cos((2 * Mathf.PI * index) / (width - 1));

            //noise
            Unity.Mathematics.float2 fz = new Unity.Mathematics.float2 { x = posX*index + seed / 1f + seed, y = posZ * index + seed / 1f };
            float posZOffset = Unity.Mathematics.noise.cnoise(fz) * 0.33f;

            vertices[index] = new Vector3(posX + posZOffset, posY * segmentHeight, posZ + posZOffset) + curves[posY];
        }
    }

    #endregion


    #region Cone
    protected void GenerateCone(int width, float radius, Mesh parentMesh)
    {
        int height = 1;
        int startLoopJ = 0;
        if (parentMesh == null)
        {
            GenerateConeMeshVertsByJobs(width, radius);
        }
        else
        {
            if (width % 2 == 1)
                startLoopJ = 1;

            width /= 2;
            //Debug.Log("GENERATING LOD FROM PARENT MESH");
            int fixedWidth = (parentMesh.vertices.Length-1)/2;
            int fixedCount = fixedWidth + 1;
            Vector3[] nArr = new Vector3[fixedCount/*fixedWidth * height*/];
             for (int i = 0, j = startLoopJ; i < fixedWidth; i++, j += 2)
             {
                 nArr[i] = parentMesh.vertices[j];
             }

            nArr[fixedCount-1] = parentMesh.vertices[parentMesh.vertices.Length-1];
            meshFilter.mesh.vertices = nArr;

            FillConeMeshTrianglesByJobs(fixedWidth, 1);
            GetMeshWidthAndHeight(fixedWidth, 1);
            return;
        }

        FillConeMeshTrianglesByJobs(width, height);
        GetMeshWidthAndHeight(width, height);
    }

    void FillConeMeshTrianglesByJobs(int w, int h)
    {
        NativeArray<int> trianglesNative = new NativeArray<int>((w*h + 1 /*w * h + w - 1*/ /*+*//*for top and bottom*//*w-2*//**/) * 6, Allocator.TempJob);

        var job = new FillConeMeshTrianglesJob
        {
            width = w,
            height = h,
            tri = trianglesNative
        };

        var jobHandle = job.Schedule();
        jobHandle.Complete();

        //vertsNative.CopyTo(verts);
        meshFilter.mesh.triangles = trianglesNative.ToArray();
        trianglesNative.Dispose();
    }
    [Unity.Burst.BurstCompile]
    struct FillConeMeshTrianglesJob : IJob
    {
        [ReadOnly]
        public int width;
        [ReadOnly]
        public int height;
        public NativeArray<int> tri;

        public void Execute()
        {
            int ti = 0, vi = 0;

            int vecOffset = 0;
            int countFixR = 0;
            int countFixL = 0;
            if (width % 4 != 0)
            {
                if (width % 2 == 0)
                {
                    countFixR = 1;
                    countFixL = 1;
                }
                else
                {
                    countFixR = 1;
                    countFixL = 1;
                    vi++;
                }
            }

            int startI = width % 2 == 0 ? 1 : 0;
            int ti_startUp = ti;
            int su = vi + startI;
            int vi_startUp = vi + startI;
            vi += startI;

            //Right side   
            for (int i = 0; i < (width / 2 / 2 - 1 + countFixR); i++, vecOffset++, ti += 6)
            {
                tri[ti] = vi + vecOffset;
                tri[ti + 1] = vi + (width / 2 - 1) - vecOffset;
                tri[ti + 2] = vi - 1 + vecOffset;
                tri[ti + 3] = vi + (width / 2 - 1) - 1 - vecOffset ;
                tri[ti + 4] = vi + (width / 2 - 1) - vecOffset;
                tri[ti + 5] = vi + vecOffset;
            }
            if (countFixR == 0)
            {
                tri[ti] = vi + vecOffset ;
                tri[ti + 1] = vi + (width / 2 - 1) - vecOffset;
                tri[ti + 2] = vi - 1 + vecOffset;
                ti += 3;
            }

            //LeftSize
            //Center
            tri[ti] = vi - 1 ;
            tri[ti + 1] = vi + (width / 2) - 1;
            tri[ti + 2] = vi + width / 2;
            tri[ti + 3] = vi + width - 2;
            tri[ti + 4] = vi - 1 ;
            tri[ti + 5] = vi + (width / 2);
            ti += 6;

            vecOffset = 1;
            for (int i = 0; i < (width / 2 / 2 - 1 - 1 + countFixL/*another -1 because it can not start from 0, also that si why vecOffset is 1 at the beginning*/); i++, vecOffset++, ti += 6)
            {
                tri[ti] = vi - vecOffset - 1 + width;
                tri[ti + 1] = vi + (width / 2 - 1) + vecOffset ;
                tri[ti + 2] = vi + width - vecOffset - 2;

                tri[ti + 3] = vi + width - vecOffset - 2;
                tri[ti + 4] = vi + (width / 2 - 1) + vecOffset;
                tri[ti + 5] = vi + (width / 2 - 1) + vecOffset + 1;
            }

            tri[ti] = vi - vecOffset - 1 + width;
            tri[ti + 1] = vi + (width / 2 - 1) + vecOffset;
            tri[ti + 2] = vi + width - vecOffset - 2;
            ti += 3;

            vi = 0;
            for (int i = 0; i < width-1; i++, vi++, ti+=3)
            {
                tri[ti] = vi;
                tri[ti + 1] = width;
                tri[ti + 2] = vi+1;
            }

            tri[ti] = vi;
            tri[ti + 1] = width;
            tri[ti + 2] = 0;
        }
    }

    void GenerateConeMeshVertsByJobs(int width, float radius)
    {
        //int fixedWidth = width + 1;
        int fixedHeight = 1;

        NativeArray<Vector3> vertsNative = new NativeArray<Vector3>(width * fixedHeight + 1, Allocator.TempJob);
        float mult = (int)radius;
        var job = new GenerateVertsForConeJob
        {
            width = width,
            radius = radius,
            seed = Random.Range(-100000, 100000),
            vertices = vertsNative,
        };

        var jobHandle = job.Schedule(vertsNative.Length-1, 10);
        jobHandle.Complete();

        vertsNative[vertsNative.Length - 1] = new Vector3(0, 1, 0);

        meshFilter.mesh.vertices = vertsNative.ToArray();
        vertsNative.Dispose();
    }
    [Unity.Burst.BurstCompile]
    struct GenerateVertsForConeJob : IJobParallelFor
    {
        [ReadOnly]
        public int width;
        [ReadOnly]
        public float radius;
        [ReadOnly]
        public float seed;
        [WriteOnly]
        public NativeArray<Vector3> vertices;

        public void Execute(int index)
        {
            int posY = index / (width);
            float posX = radius * Mathf.Sin((2 * Mathf.PI * index) / (width));
            float posZ = radius * -Mathf.Cos((2 * Mathf.PI * index) / (width));

            //noise
            Unity.Mathematics.float2 fz = new Unity.Mathematics.float2 { x = posX * index + seed / 100f + seed, y = posZ * index + seed / 100f };
            float posZOffset = Unity.Mathematics.noise.cnoise(fz) * 0.15f;

            vertices[index] = new Vector3(posX + posZOffset, posY + posZOffset, posZ + posZOffset);
        }
    }

    #endregion


    #region Cube

    protected void GenerateCube()
    {
        GenerateCubeMeshVerts();
        FillCubeMeshTriangles();

        GetMeshWidthAndHeight(1, 1);
    }

    void FillCubeMeshTriangles()
    {
        int[] tris = new int[36]
        {
            //Front
            0, 2, 1, 2, 3, 1,
            //Back
            5, 6, 4, 5, 7, 6,
            //Left
            4, 2, 0, 4, 6, 2,
            //Right
            1, 7, 5, 1, 3, 7,
            //Bottom
            1, 4, 0, 5, 4, 1,
            2, 6, 3, 6, 7, 3
        };
        meshFilter.mesh.triangles = tris;
    }

    void GenerateCubeMeshVerts()
    {
        Vector3[] verts = new Vector3[8]
        {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),

            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f)
        };
        meshFilter.mesh.vertices = verts;
    }
    #endregion


    #region Plane

    protected void GeneratePlane(int width, int height)
    {
        GeneratePlaneMeshVertsByJobs(width, height);
        FillPlaneMeshTrianglesByJobs(width, height);

        GetMeshWidthAndHeight(width, height);
    }

    void FillPlaneMeshTrianglesByJobs(int w, int h)
    {
        //Vector3[] verts = new Vector3[(w + 1) * (h + 1)];
        NativeArray<int> trianglesNative = new NativeArray<int>((w * h /*+*//*cylinder*//*w*/ /*- 2*//**/) * 6, Allocator.TempJob);

        var job = new FillPlaneMeshTrianglesJob
        {
            width = w,
            height = h,
            tri = trianglesNative
        };

        var jobHandle = job.Schedule();
        jobHandle.Complete();

        //vertsNative.CopyTo(verts);
        meshFilter.mesh.triangles = trianglesNative.ToArray();
        trianglesNative.Dispose();
    }
    [Unity.Burst.BurstCompile]
    struct FillPlaneMeshTrianglesJob : IJob
    {
        [ReadOnly]
        public int width;
        [ReadOnly]
        public int height;
        public NativeArray<int> tri;

        public void Execute()
        {
            int ti = 0, vi = 0, y = 0, x = 0;
            for (; y < height; y++, vi++)
            {
                for (; x < width; vi++, ti += 6, x++)
                {
                    tri[ti] = vi;
                    tri[ti + 3] = tri[ti + 2] = vi + 1;
                    tri[ti + 4] = tri[ti + 1] = vi + width + 1;
                    tri[ti + 5] = vi + width + 2;
                }
                x = 0;
            }
        }
    }

    void GeneratePlaneMeshVertsByJobs(int w, int h)
    {
        //Vector3[] verts = new Vector3[(w + 1) * (h + 1)];
        int fixedWidth = w + 1;
        int fixedHeight = h + 1;

        NativeArray<Vector3> vertsNative = new NativeArray<Vector3>(fixedWidth * fixedHeight, Allocator.TempJob);

        var job = new GenerateVertsForPlaneJob
        {
            width = fixedWidth,
            vertices = vertsNative
        };

        var jobHandle = job.Schedule(vertsNative.Length, 15);
        jobHandle.Complete();

        //vertsNative.CopyTo(verts);
        meshFilter.mesh.vertices = vertsNative.ToArray();
        vertsNative.Dispose();

    }
    [Unity.Burst.BurstCompile]
    struct GenerateVertsForPlaneJob : IJobParallelFor
    {
        [ReadOnly]
        public int width;
        [WriteOnly]
        public NativeArray<Vector3> vertices;

        public void Execute(int index)
        {
            float posX = index % width;
            float posY = index / width;

            Unity.Mathematics.float2 f = new Unity.Mathematics.float2 { x = posX / 1.5f, y = posY / 1.5f};
            float posZ = Unity.Mathematics.noise.cnoise(f) * 2;

            vertices[index] = new Vector3(posX, posY, posZ);

        }
    }
    #endregion

}