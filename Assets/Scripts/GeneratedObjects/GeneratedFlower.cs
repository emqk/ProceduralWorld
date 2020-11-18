using UnityEngine;

public class GeneratedFlower : MonoBehaviour
{
    public GeneratedBranch generatedBranch;
    public GeneratedLeaves generatedLeaves;

    public GameObject mergedMesh;

    void Start()
    {
        GenerateBranch();

        GenerateLeaves();
        //MergeGrass();
        //AddLODs();
    }

    void GenerateBranch()
    {
        generatedBranch.Generate(4, 1, 1, 1, null, 0.5f, 0.65f);
        generatedBranch.VerySlowlyConvertToFlatShading();
        generatedBranch.transform.localScale = Vector3.Scale(transform.localScale, new Vector3(0.05f, 0.4f, 0.05f));
    }

    void GenerateLeaves()
    {
        int randInt = Random.Range(0, 100);
        if (randInt <= 49)
        {
            generatedLeaves.GetComponent<Renderer>().material = VegetationGenerator.instance.flowerRedMat;
        }
        else
        {
            generatedLeaves.GetComponent<Renderer>().material = VegetationGenerator.instance.flowerYellowMat;
        }

        generatedLeaves.Generate(0);
        generatedLeaves.VerySlowlyConvertToFlatShading();
        generatedLeaves.transform.localPosition = Vector3.Scale(transform.localScale, new Vector3(0, 0.5f, 0));
        generatedLeaves.transform.localScale = Vector3.Scale(transform.localScale, new Vector3(0.1f, 0.175f, 0.1f));
    }

    void AddLODs()
    {
        if (GetComponent<LODGroup>())
        {
            LOD[] lods = new LOD[1];
            Renderer[] renderers = new Renderer[1];
            renderers[0] = mergedMesh.GetComponent<Renderer>();

            lods[0] = new LOD(0.0045f, renderers);

            GetComponent<LODGroup>().SetLODs(lods);
            GetComponent<LODGroup>().RecalculateBounds();
        }
    }

    void MergeGrass()
    {
        //Merge objects to one mesh
        MeshFilter[] meshFilters = new MeshFilter[2];
        meshFilters[0] = generatedBranch.GetComponent<MeshFilter>();
        meshFilters[1] = generatedLeaves.GetComponent<MeshFilter>();

        mergedMesh = GeneratedMesh.CombineMeshes(transform, meshFilters);
    }
}
