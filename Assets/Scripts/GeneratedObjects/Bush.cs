using UnityEngine;

public class Bush : MonoBehaviour
{
    public GeneratedLeaves generatedLeaves;
    public GameObject foodObject;
    const int defaultRecursionLevel = 2;

    void Start()
    {
        generatedLeaves.Generate(defaultRecursionLevel);
        generatedLeaves.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.7f, 1.0f), Random.Range(0.0f, 0.35f));

        GenerateFood();
        AdjustBushLeaves();

        AddLODs();

        generatedLeaves.VerySlowlyConvertToFlatShading();
    }

    public void TakeFood(ref AnimalCarrying animalCarrying)
    {
        int takeFoodAmount = 3;
        animalCarrying.SetupCarrying(takeFoodAmount);
        foodObject.SetActive(false);
        //foodAmount = 3;
    }
    public bool HaveFood()
    {
        return foodObject.activeSelf;
    }
    private void GenerateFood()
    {
        int count = 5;
        GeneratedLeaves[] food = new GeneratedLeaves[count];
        for (int i = 0; i < count; i++)
        {
            food[i] = Instantiate(VegetationGenerator.instance.generatedBushFoodPrefab, generatedLeaves.transform);
            food[i].transform.localPosition = generatedLeaves.GetComponent<MeshFilter>().mesh.vertices[Random.Range(0, generatedLeaves.GetComponent<MeshFilter>().mesh.vertexCount - 1)];
            food[i].Generate(0);
            food[i].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            food[i].VerySlowlyConvertToFlatShading();
        }

        //Merge food objects to one mesh
        count = food.Length;
        MeshFilter[] meshFilters = new MeshFilter[count];
        for (int i = 0; i < count; i++)
        {
            meshFilters[i] = food[i].GetComponent<MeshFilter>();
        }

        foodObject = GeneratedMesh.CombineMeshes(generatedLeaves.transform, meshFilters);
    }

    void AddLODs()
    {
        if (GetComponent<LODGroup>())
        {
            LOD[] lods = new LOD[3];
            Renderer[] renderers = new Renderer[2];
            renderers[0] = generatedLeaves.GetComponent<Renderer>();
            renderers[1] = foodObject.GetComponent<Renderer>();
            Renderer[] renderers2 = new Renderer[2];
            renderers2[0] = CreateLODFromMesh(generatedLeaves.gameObject, 1).GetComponent<Renderer>();
            renderers2[1] = foodObject.GetComponent<Renderer>();
            Renderer[] renderers3 = new Renderer[2];
            renderers3[0] = CreateLODFromMesh(generatedLeaves.gameObject, 0).GetComponent<Renderer>();
            renderers3[1] = foodObject.GetComponent<Renderer>();

            lods[0] = new LOD(0.075f, renderers);
            lods[1] = new LOD(0.05f, renderers2);
            lods[2] = new LOD(0.005f, renderers3);

            GetComponent<LODGroup>().SetLODs(lods);
            GetComponent<LODGroup>().RecalculateBounds();
        }
    }
    GameObject CreateLODFromMesh(GameObject target, int recursion_Level)
    {
        GameObject go = null;

        go = target.GetComponent<GeneratedLeaves>().CreateLODObject(target, transform, recursion_Level, null);
        go.GetComponent<GeneratedLeaves>().VerySlowlyConvertToFlatShading();

        return go;
    }

    private void AdjustBushLeaves()
    {
        generatedLeaves.transform.localScale = generatedLeaves.transform.localScale + new Vector3(Random.Range(0f, 0.3f), Random.Range(0.2f, 0.5f), Random.Range(0f, 0.3f));
        generatedLeaves.transform.localPosition = new Vector3(0f, generatedLeaves.transform.localScale.y - 0.05f/*offset*/, 0f);
    }
}
