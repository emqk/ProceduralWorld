using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

    public enum DrawMode
    {
        NoiseMap, ColourMap, Mesh
    }
    public DrawMode drawMode;

    public static readonly int mapChunkSize = 241;
    [Range(0, 10)]
    public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    public float persistance;
    public float lacunarity;

    [SerializeField]
    bool useRandomSeed;
    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier = 1;
    public AnimationCurve meshHeightCurve;

    public TerrainType[] regions;

    public static TerrainGenerator instance;
    [HideInInspector] public TerrainDisplay terrainDisplay;

    private void Awake()
    {
        instance = this;
        terrainDisplay = GetComponent<TerrainDisplay>();

        noiseScale = NewWorldData.countDictionary[TerrainSettingType.terrainNoiseScale];
        octaves = (int)NewWorldData.countDictionary[TerrainSettingType.terrainOctaves];
        persistance = NewWorldData.countDictionary[TerrainSettingType.terrainPersistance];
        lacunarity = NewWorldData.countDictionary[TerrainSettingType.terrainLacunarity];
    }

    /*private void Start()
    {
        if (useRandomSeed)
            seed = Random.Range(-1000000, 1000000);

        GenerateTerrain();
        terrainDisplay.meshFilter.gameObject.AddComponent<MeshCollider>();
        Debug.Log("Terrain verts count: " + terrainDisplay.meshFilter.mesh.vertexCount);
    }*/

    public void SetupAndGenerateTerrain()
    {
        if (useRandomSeed)
            seed = Random.Range(-1000000, 1000000);

        GenerateTerrain();
        terrainDisplay.meshFilter.gameObject.AddComponent<MeshCollider>();
        Debug.Log("Terrain verts count: " + terrainDisplay.meshFilter.mesh.vertexCount);
    }

    void GenerateTerrain()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapChunkSize + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        if (drawMode == DrawMode.NoiseMap)
        {
            terrainDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            terrainDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            terrainDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        }
    }
}


[System.Serializable]
public struct TerrainType
{
    public float height;
    public Color color;
}