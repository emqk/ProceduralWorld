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
        AddLODs();
    }

    void GenerateBranch()
    {
        generatedBranch.Generate(4, 2, 2.2f, 1.25f, null, 0.75f, 0.85f);
        generatedBranch.VerySlowlyConvertToFlatShading();
        generatedBranch.transform.localScale = Vector3.Scale(transform.localScale, new Vector3(0.05f, 0.4f, 0.05f));
    }

    void GenerateLeaves()
    {
        generatedLeaves.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

        generatedLeaves.Generate(0);
        generatedLeaves.VerySlowlyConvertToFlatShading();
        
        generatedLeaves.transform.localPosition = new Vector3(0, generatedBranch.mySegmentHeight * generatedBranch.meshHeight * 0.4f, 0);
        generatedLeaves.transform.localScale = Vector3.Scale(transform.localScale, new Vector3(0.2f, 0.275f, 0.2f));
    }

    void AddLODs()
    {
        if (GetComponent<LODGroup>())
        {
            LOD[] lods = new LOD[1];
            Renderer[] renderers = new Renderer[2];
            renderers[0] = generatedBranch.GetComponent<Renderer>();
            renderers[1] = generatedLeaves.GetComponent<Renderer>();

            lods[0] = new LOD(0.015f, renderers);

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
