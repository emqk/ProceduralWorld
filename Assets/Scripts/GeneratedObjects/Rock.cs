using UnityEngine;

public class Rock : MonoBehaviour
{
    public GeneratedLeaves generatedLeaves;
    const int defaultRecursionLevel = 1;

    void Start()
    {
        generatedLeaves.Generate(defaultRecursionLevel);
        Adjust();

        AddLODs();

        generatedLeaves.VerySlowlyConvertToFlatShading();
    }
    void AddLODs()
    {
        if (GetComponent<LODGroup>())
        {
            LOD[] lods = new LOD[2];
            Renderer[] renderers = new Renderer[1];
            renderers[0] = generatedLeaves.GetComponent<Renderer>();
            Renderer[] renderers2 = new Renderer[1];
            renderers2[0] = CreateLODFromMesh(generatedLeaves.gameObject, 0).GetComponent<Renderer>();
            lods[0] = new LOD(0.1f, renderers);
            lods[1] = new LOD(0.005f, renderers2);

            GetComponent<LODGroup>().SetLODs(lods);
            GetComponent<LODGroup>().RecalculateBounds();
        }
    }
    GameObject CreateLODFromMesh(GameObject target, int recursion_Level)
    {
        GameObject go = null;

        go = CreateLODObject(target, transform, recursion_Level, null);
        go.GetComponent<GeneratedLeaves>().VerySlowlyConvertToFlatShading();

        return go;
    }
    GameObject CreateLODObject(GameObject target, Transform parent, int recursion_Level, Mesh parentMesh)
    {
        GameObject go = Instantiate(VegetationGenerator.instance.generatedLeavesPrefab.gameObject, parent);
        go.name = "Overrided LOD_TEST";

        go.transform.position = target.transform.position;
        go.transform.localScale = target.transform.localScale;
        go.GetComponent<GeneratedLeaves>().Generate(recursion_Level);
        go.GetComponent<MeshRenderer>().materials = generatedLeaves.GetComponent<MeshRenderer>().materials;

        return go;
    }


    private void Adjust()
    {
        generatedLeaves.transform.localScale = new Vector3(0.8f + Random.Range(0f, 0.5f), 0.8f + Random.Range(0f, 0.5f), 0.8f + Random.Range(0f, 0.5f));
        generatedLeaves.transform.localPosition = new Vector3(0f, generatedLeaves.transform.localScale.y- generatedLeaves.transform.localScale.y*Random.Range(0.1f, 0.4f), 0f);
    }
}
