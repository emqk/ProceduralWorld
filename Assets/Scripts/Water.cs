using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Collections;

public class Water : MonoBehaviour {

    [SerializeField] float flowSpeed = 0.25f;
    [SerializeField] float multiplier = 0.1f;
    [SerializeField ]float scale = 5;

    int chunkSize;
    Vector3[] verts;

    public static Water globalWaterInstance;

    public void Setup(int size) {

        chunkSize = size;
        verts = GetComponent<MeshFilter>().mesh.vertices;
        Debug.Log("Water verts count: " + verts.Length);

        globalWaterInstance = this;
    }

    void Update() {

        //Simulate();
        SimulateJobs();
    }

    void Simulate()
    {
        int currVert = 0;
        int count = verts.Length;
        for (int y = 0; y < count; y++)
        {
          //  for (int x = 0; x < count; x++)
          //  {
                float currX = verts[currVert].x / scale + Time.time * flowSpeed;
                float currY = verts[currVert].y / scale + Time.time * flowSpeed;
                verts[currVert].y = Mathf.PerlinNoise(currX, currY) * multiplier;
                currVert++;
           // }
        }

        GetComponent<MeshFilter>().mesh.vertices = verts;
    }

    void SimulateJobs()
    {
        NativeArray<Vector3> vertsNative = new NativeArray<Vector3>(verts, Allocator.TempJob);
        var job = new SimulateJob
        {
            nativeVerts = vertsNative,
            multiplier = multiplier,
            scale = scale,
            flowSpeed = flowSpeed,
            time = Time.time
        };
        var jobHandle = job.Schedule(vertsNative.Length, 250);
        jobHandle.Complete();

        vertsNative.CopyTo(verts);
        GetComponent<MeshFilter>().mesh.vertices = verts;
        vertsNative.Dispose();
    }

    [Unity.Burst.BurstCompile]
    public struct SimulateJob : IJobParallelFor
    {
        public NativeArray<Vector3> nativeVerts;
        public float multiplier;
        public float scale;
        public float flowSpeed;
        public float time;

        public void Execute(int index)
        {
              var currVert = nativeVerts[index];

              float currX = currVert.x / scale + time * flowSpeed;
              float currY = currVert.z / scale + time * flowSpeed;

              currVert.y = Mathf.PerlinNoise(currX, currY) * multiplier;

              nativeVerts[index] = currVert;
           /* New math test
            *var currVert = nativeVerts[index];
            Unity.Mathematics.float2 f = new Unity.Mathematics.float2 { x= currVert.x / scale + time * flowSpeed, y= currVert.z / scale + time * flowSpeed };
            nativeVerts[index] = new Unity.Mathematics.float3 { x = currVert.x, y = Unity.Mathematics.noise.cnoise(f)*multiplier, z = currVert.z};*/
        }
    }
}
