using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    [SerializeField]
    NavMeshSurface navMeshSurface;

    public static NavMeshManager instance;

    private void Awake()
    {
        instance = this;
    }

   /* void Start()
    {
        Invoke("BuildNavMesh", 1f);
    }
    */
    public void BuildNavMesh()
    {
        if (WorldGenerator.GetIsItAR())
        {
            navMeshSurface.size *= WorldGenerator.worldGenerator.GetScaleMultiplier();
            navMeshSurface.voxelSize *= WorldGenerator.worldGenerator.GetScaleMultiplier();
            navMeshSurface.voxelSize = 0.01f;
            navMeshSurface.overrideTileSize = true;
            navMeshSurface.tileSize = 1;
        }
        navMeshSurface.BuildNavMesh();
    }
}
