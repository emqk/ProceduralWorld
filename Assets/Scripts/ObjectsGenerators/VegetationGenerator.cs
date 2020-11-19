using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class VegetationGenerator : MonoBehaviour
{
    public GameObject generatedNormalTreePrefab;
    public GameObject generatedTallTreePrefab;
    public GameObject generatedChristmasTreePrefab;
    public GameObject generatedBushPrefab;
    public GameObject generatedRockPrefab;
    public GameObject generatedGrassPrefab;
    public GameObject generatedFlowerPrefab;

    public GeneratedBranch generatedBranchPrefab;
    public GeneratedLeaves generatedLeavesPrefab;
    public GeneratedCone generatedConeLeavesPrefab;

    public GeneratedLeaves generatedBushFoodPrefab;
    List<Bush> bushes = new List<Bush>();
    List<Tree> trees = new List<Tree>();
    List<GameObject> grasses = new List<GameObject>();

    public Building generatedBuildingPrefab;

    Vector2Int defaultTreeWidthFromTo = new Vector2Int(13, 18/*17, 24*/);
    Vector2Int defaultTreeHeightFromTo = new Vector2Int(3, 4);
    Vector2Int defaultChristmasTreeWidthFromTo = new Vector2Int(6, 10);
    Vector2Int defaultChristmasTreeLeavesHeightFromTo = new Vector2Int(3, 5);
    readonly float defaultTreeRadius = 1;

    Vector3 terrainMeshScale;

    public static VegetationGenerator instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

  /*  void Start()
    {
        GenerateSomeRandomTrees();

        //Generating trees is delayed because of setting up terrain colliders
        terrainMeshScale = GetComponent<TerrainDisplay>().meshFilter.gameObject.transform.localScale;
        Invoke("GenerateSomeRandomTrees", 1.1f);
    }*/

    public void GenerateSomeRandomVegetation()
    {
        //terrainMeshScale = GetComponent<TerrainDisplay>().meshFilter.gameObject.transform.localScale;
        terrainMeshScale = TerrainGenerator.instance.terrainDisplay.meshFilter.gameObject.transform.localScale;
        Debug.Log("Terrain mesh scale: " + terrainMeshScale);

        SpawnTrees();
        SpawnBushes();
        SpawnRocks();
        SpawnFlowers();
        SpawnGrasses();
        StartCoroutine(MergeDelay());

    }
    private IEnumerator MergeDelay()
    {
        yield return new WaitForSeconds(2f);
        MergeGrassGroup();
    }
    void SpawnTrees()
    {
        for (int i = 0; i < NewWorldData.countDictionary[TerrainSettingType.NormalTree]; i++)
        {
            NormalTreeGenerationData treeGenData = new NormalTreeGenerationData {
                  widthRange = defaultTreeWidthFromTo
                , heightRange = defaultTreeHeightFromTo
                , radius = Random.Range(1.0f, 1.5f)
                , segmentHeight = 1.6f
                , childLevels = 1
                , branchesAmountRange = NormalTree.GetDefaultTreeBrachesAmount()
                , nestedTreeAmountRange = NormalTree.GetDefaultTreeNestedTreesAmount() };
            SpawnTree(treeGenData, true);
        }
        for (int i = 0; i < NewWorldData.countDictionary[TerrainSettingType.TallTree]; i++)
        {
            SpawnTallTree(true);
        }
        for (int i = 0; i < NewWorldData.countDictionary[TerrainSettingType.ChristmasTree]; i++)
        {
            SpawnChristmasTree(true);
        }
    }
    void SpawnBushes()
    {
        for (int i = 0; i < NewWorldData.countDictionary[TerrainSettingType.Bush]; i++)
        {
            SpawnBush(true);
        }
    }
    void SpawnRocks()
    {
        for (int i = 0; i < NewWorldData.countDictionary[TerrainSettingType.Rock]; i++)
        {
            SpawnRock(true);
        }
    }
    void SpawnFlowers()
    {
        for (int i = 0; i < NewWorldData.countDictionary[TerrainSettingType.Flower]; i++)
        {
            SpawnFlower(true);
        }
    }
    void SpawnGrasses()
    {
        for (int i = 0; i < NewWorldData.countDictionary[TerrainSettingType.Grass]; i++)
        {
            SpawnGrass(true);
        }
    }
    void MergeGrassGroup()
    {
        int mergeGroupSize = 5;
        int count = grasses.Count / mergeGroupSize;
        for (int i = 0; i < count; i++)
        {
            GameObject[] grassesToMerge = FindClosestGrassGroup(mergeGroupSize);
            MergeGrass(grassesToMerge);

            //Clean old empty grass GameObjects (those original ones, before merging)
            foreach (GameObject grassToClean in grassesToMerge)
            {
                Destroy(grassToClean);
            }
        }
    }
    GameObject[] FindClosestGrassGroup(int groupSize)
    {
        GameObject[] group = new GameObject[groupSize];
        GameObject startGrass = grasses[0];
        group[0] = startGrass;
        grasses.RemoveAt(0);

        for (int i = 1; i < groupSize; i++)
        {
            float closestDist = float.MaxValue;
            int closestIndex = -1;

            for (int j = 0; j < grasses.Count; j++)
            {
                float currDist = (startGrass.transform.position - grasses[j].transform.position).sqrMagnitude;
                if (currDist < closestDist)
                {
                    closestDist = currDist;
                    closestIndex = j;
                }
            }

            group[i] = grasses[closestIndex];
            grasses.RemoveAt(closestIndex);
        }

        return group;
    }

    public GameObject SpawnTree(NormalTreeGenerationData treeGenData, bool findGround)
    {
        GameObject instance = Instantiate(generatedNormalTreePrefab);
        instance.GetComponent<NormalTree>().Generate(
            Random.Range(treeGenData.widthRange.x, treeGenData.widthRange.y)
            , Random.Range(treeGenData.heightRange.x, treeGenData.heightRange.y)
            , new Vector3(0.75f, 0.75f, 0.75f)
            , treeGenData.radius
            , treeGenData.segmentHeight
            , treeGenData.childLevels
            , Random.Range(treeGenData.branchesAmountRange.x, treeGenData.branchesAmountRange.y)
            , Random.Range(treeGenData.nestedTreeAmountRange.x, treeGenData.nestedTreeAmountRange.y));

        trees.Add(instance.GetComponent<Tree>());

        if (findGround)
            PlaceOnTerrainOnRandomPos(instance.transform);

        return instance;
    }

    public GameObject SpawnTallTree(bool findGround)
    {
        GameObject instance = Instantiate(generatedTallTreePrefab);
        //int nestedTreesAmount = Random.Range(1, 5);
        //int branchesAmount = Random.Range(3, 3);
        instance.GetComponent<TallTree>().Generate(
            Random.Range(defaultTreeWidthFromTo.x, defaultTreeWidthFromTo.y)
            , Random.Range(defaultTreeHeightFromTo.x * 2, defaultTreeHeightFromTo.y * 2)
            , new Vector3(1, 1.25f, 1)
            , defaultTreeRadius * Random.Range(1.25f, 2.0f)
            , 1
            , 1
            , TallTree.GetTallTreeBrachesAmount()
            , TallTree.GetTallTreeNestedTreesAmount());

        trees.Add(instance.GetComponent<Tree>());

        if(findGround)
            PlaceOnTerrainOnRandomPos(instance.transform, 19f);

        return instance;
    }

    public GameObject SpawnChristmasTree(bool findGround)
    {
        GameObject instance = Instantiate(generatedChristmasTreePrefab);
        instance.GetComponent<ChristmasTree>().Generate(
            Random.Range(defaultChristmasTreeWidthFromTo.x, defaultChristmasTreeWidthFromTo.y)
            , Random.Range(defaultChristmasTreeLeavesHeightFromTo.x, defaultChristmasTreeLeavesHeightFromTo.y)
            , new Vector3(1, 1, 1)
            , defaultTreeRadius
            , 1
            , 1
            , 0
            , 0);

        trees.Add(instance.GetComponent<Tree>());

        if(findGround)
            PlaceOnTerrainOnRandomPos(instance.transform, 15f);

        return instance;
    }


    public GameObject SpawnBush(bool findGround)
    {
        GameObject instance = Instantiate(generatedBushPrefab);
        bushes.Add(instance.GetComponent<Bush>());

        if(findGround)
            PlaceOnTerrainOnRandomPos(instance.transform);

        return instance;
    }
    public GameObject SpawnRock(bool findGround)
    {
        GameObject instance = Instantiate(generatedRockPrefab.gameObject);

        if(findGround)
            PlaceOnTerrainOnRandomPos(instance.transform, 0, 10.5f);

        return instance;
    }
    public GameObject SpawnFlower(bool findGround)
    {
        GameObject instance = Instantiate(generatedFlowerPrefab.gameObject);

        if (findGround)
            PlaceOnTerrainOnRandomPos(instance.transform);

        return instance;
    }
    public GameObject SpawnGrass(bool findGround)
    {
        GameObject instance = Instantiate(generatedGrassPrefab.gameObject);

        if(findGround)
            PlaceOnTerrainOnRandomPos(instance.transform);

        grasses.Add(instance);

        return instance;
    }
    void MergeGrass(GameObject[] _grassesToMerge)
    {
        GameObject mergedMesh = new GameObject("MergedGrassGroup");
        Vector3 avgPos = GetAvgPosOfObjs(_grassesToMerge);
        mergedMesh.transform.position = avgPos;
        //Merge objects to one mesh
        int count = _grassesToMerge.Length;
        MeshFilter[] meshFilters = new MeshFilter[count];
        for (int i = 0; i < count; i++)
        {
            meshFilters[i] = _grassesToMerge[i].transform.GetChild(0).GetComponent<MeshFilter>();
            _grassesToMerge[i].transform.position -= avgPos;
        }

        GeneratedMesh.CombineMeshes(mergedMesh.transform, meshFilters);

        /////////////////////////
        
        //AddLods
        mergedMesh.AddComponent<LODGroup>();
        LOD[] lods = new LOD[1];
        Renderer[] renderers = new Renderer[1];
        renderers[0] = mergedMesh.transform.GetChild(0).GetComponent<Renderer>();       
        lods[0] = new LOD(0.07f, renderers);

        mergedMesh.GetComponent<LODGroup>().SetLODs(lods);
        mergedMesh.GetComponent<LODGroup>().RecalculateBounds();      
    }
    Vector3 GetAvgPosOfObjs(GameObject[] objs)
    {
        int count = objs.Length;
        Vector3 resultTemp = new Vector3();
        foreach (GameObject obj in objs)
        {
            resultTemp += obj.transform.position;
        }
        Vector3 result = new Vector3(resultTemp.x / count, resultTemp.y / count, resultTemp.z / count);
        return result;
    }


    public Bush GetReadyToInteractNearestBushWithFood(Vector3 pos)
    {
        int count = bushes.Count;
        int nearestBushIndex = -1;
        float nearestBushDistance = float.MaxValue;
        for (int i = 0; i < count; i++)
        {
            float dist = (pos - bushes[i].transform.position).sqrMagnitude;
            if (dist < nearestBushDistance)
            {
                if (bushes[i].HaveFood() && !bushes[i].GetComponent<InteractionTarget>().IsSomeoneInteracting())
                {
                    nearestBushIndex = i;
                    nearestBushDistance = dist;
                }
            }
        }
        if (nearestBushIndex >= 0)
            return bushes[nearestBushIndex];
        else
            return null;
    }
    public Tree GetReadyToInteractNearestTree(Vector3 pos)
    {
        int count = trees.Count;
        int nearestTreeIndex = -1;
        float nearestTreeDistance = float.MaxValue;
        for (int i = 0; i < count; i++)
        {
            float dist = (pos - trees[i].transform.position).sqrMagnitude;
            if (dist < nearestTreeDistance)
            {
                if (!trees[i].GetComponent<InteractionTarget>().IsSomeoneInteracting())
                {
                    nearestTreeIndex = i;
                    nearestTreeDistance = dist;
                }
            }
        }
        if (nearestTreeIndex >= 0)
            return trees[nearestTreeIndex];
        else
            return null;
    }


    public void PlaceOnTerrainOnRandomPos(Transform objToPlace, float minHeight = float.MinValue, float maxHeight = float.MaxValue)
    {
        float arScaleMultiplier = WorldGenerator.GetIsItAR() ? 482 : 1;

        float terrainChunkSize = TerrainGenerator.mapChunkSize / 2f * TerrainGenerator.instance.transform.parent.localScale.x;
        if (WorldGenerator.GetIsItAR()) // is AR
        {
            terrainChunkSize = (TerrainGenerator.mapChunkSize * (arScaleMultiplier * TerrainGenerator.instance.transform.parent.localScale.x)) / 2f /* * TerrainGenerator.instance.transform.parent.localScale.x*/;
        }
        //Debug.Log("Terrain chunk size: " + terrainChunkSize);

        Vector3 terrainParentPosition = TerrainGenerator.instance.transform.position;
        float rayPosY = 500;

        Vector3 randPos = terrainParentPosition + new Vector3(Random.Range(-terrainChunkSize * terrainMeshScale.x + 0.5f, terrainChunkSize * terrainMeshScale.x - 0.5f)
                        , rayPosY
                        , Random.Range(-terrainChunkSize * terrainMeshScale.z + 0.5f, terrainChunkSize * terrainMeshScale.z - 0.5f));

        //Scale fix on axis y
        //randPos = new Vector3(randPos.x, randPos.y/2, randPos.z);

        //Debug.Log("terrChunkSize: " + terrainChunkSize);
        //Debug.Log("Rand pos: " + randPos);

        //return;

        RaycastHit hit = new RaycastHit();
        float waterPosY = 0;
        if(Water.globalWaterInstance)
            waterPosY = Water.globalWaterInstance.transform.position.y/* + terrainParentPosition.y*/;

        float fixedTerrainParentScaleY = TerrainGenerator.instance.transform.parent.localScale.y * arScaleMultiplier;
        minHeight *= fixedTerrainParentScaleY;
        maxHeight *= fixedTerrainParentScaleY;

        minHeight = minHeight + terrainParentPosition.y;
         maxHeight = maxHeight + terrainParentPosition.y;


        //Debug.Log("Water posY: " + waterPosY);
         if (minHeight < waterPosY)
             minHeight = waterPosY;
         if (maxHeight < waterPosY)
             maxHeight = waterPosY + 2;


        Physics.Raycast(randPos, Vector3.down, out hit);

        while (hit.point.y < waterPosY ||  hit.point.y < minHeight || hit.point.y > maxHeight)
        {
            randPos = terrainParentPosition
                                    + new Vector3(Random.Range(-terrainChunkSize * terrainMeshScale.x + 0.5f, terrainChunkSize * terrainMeshScale.x - 0.5f)
                                    , rayPosY
                                    , Random.Range(-terrainChunkSize * terrainMeshScale.z + 0.5f, terrainChunkSize * terrainMeshScale.z - 0.5f));
            Physics.Raycast(randPos, Vector3.down, out hit);
        }

        objToPlace.transform.position = hit.point - transform.up * 0.05f;
        objToPlace.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        if (TerrainGenerator.instance)
            objToPlace.SetParent(TerrainGenerator.instance.transform.parent);

       // Debug.Log("There is no collisions underneath me!");
    }
    public void PlaceOnTerrainOnRandomPosInCircle(Transform objToPlace, Vector3 origin, float circleSize, float minHeight = float.MinValue, float maxHeight = float.MaxValue)
    {
        float terrainChunkSize = TerrainGenerator.mapChunkSize / 2f;

        Vector2 randCirclePos = Random.insideUnitCircle * circleSize;
        Vector3 randPos = new Vector3(randCirclePos.x, 50, randCirclePos.y) + origin;
        RaycastHit hit = new RaycastHit();
        float waterPosY = Water.globalWaterInstance.transform.position.y;
        Debug.Log("Water posY: " + waterPosY);
        if (minHeight < waterPosY)
            minHeight = waterPosY;
        if (maxHeight < waterPosY)
            maxHeight = waterPosY + 2;

        Physics.Raycast(randPos, Vector3.down, out hit);
        while (hit.point.y < waterPosY || hit.point.y < minHeight || hit.point.y > maxHeight)
        {
            randCirclePos = Random.insideUnitCircle * circleSize;
            randPos = new Vector3(randCirclePos.x, 50, randCirclePos.y) + origin;
            Physics.Raycast(randPos, Vector3.down, out hit);
        }

        objToPlace.transform.position = hit.point - transform.up * 0.05f;
        objToPlace.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

        Debug.Log("There is no collisions underneath me!");
    }
    public bool PlaceObjectOnObjectUnderneath(Transform trans)
    {
        RaycastHit hit = new RaycastHit();
        trans.transform.position = new Vector3(trans.position.x, 100, trans.position.z);
        if (Physics.Raycast(trans.position, Vector3.down, out hit))
        {
            trans.position = hit.point;
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector3[] GetPointsAroundCircle(Vector3 source, int pointsCount, float range)
    {
        Vector3[] values = new Vector3[pointsCount];
        for (int i = 0; i < pointsCount; i++)
        {
            float x = Mathf.Sin(Mathf.PI * (i * (2f / pointsCount))) * range;
            float y = Mathf.Cos(Mathf.PI * (i * (2f / pointsCount))) * range;
            values[i] = new Vector3(x, 0, y) + source;
        }

        return values;
    }
    public void MakeNoiseInVec3(Vector3[] arr, Vector3 noiseFrom, Vector3 noiseTo)
    {
        int count = arr.Length;
        for (int i = 0; i < count; i++)
        {
            arr[i] += new Vector3(Random.Range(noiseFrom.x, noiseTo.x), Random.Range(noiseFrom.y, noiseTo.y), Random.Range(noiseFrom.z, noiseTo.z));
        }
    }


    public void RemoveTreeData(Tree tree)
    {
        trees.Remove(tree);
    }
}
