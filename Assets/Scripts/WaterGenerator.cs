using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour {


    [Range(0, 10)] [SerializeField] int levelOfDetail;
    const int mapChunkSize = 241;
    TerrainDisplay terrainDisplay;

    public static WaterGenerator instance;

    private void Awake()
    {
        terrainDisplay = GetComponent<TerrainDisplay>();
        instance = this;
    }

    /*private void Start()
    {
        GenerateWater();
    }*/

    public void GenerateWater()
    {
        terrainDisplay.DrawMeshOnly(MeshGenerator.GenerateFlatMesh(mapChunkSize, levelOfDetail));
        terrainDisplay.meshFilter.GetComponent<Water>().Setup(mapChunkSize);
    }
}
