using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tree : MonoBehaviour
{
    [SerializeField]
    protected GeneratedBranch generatedBranch;
    protected GeneratedBranch generatedBranchLOD;
    [SerializeField]
    public GeneratedLeaves generatedLeaves;
    protected GeneratedLeaves generatedLeavesLOD;
    [SerializeField]
    protected Transform generatedBranchesParent;

    Vector2Int defaultSubtreeWidthFromTo = new Vector2Int(4, 9);
    Vector2Int defaultSubtreeTreeHeightFromTo = new Vector2Int(4, 7);
    Vector2 defaultSubtreeScaleFromTo = new Vector2(0.4f, 0.6f);


    public void TakeWood(ref AnimalCarrying animalCarrying)
    {
        //woodAmount = 5;
        int takeWoodAmount = 5;
        animalCarrying.SetupCarrying(takeWoodAmount);
        Destroy(gameObject);
    }

    public virtual void Generate(int width, int height, Vector3 localScale, float radius, float segmentHeight, int generateChildsLevels, int branchesAmount, int nestedTreesAmount)
    {
        Debug.LogError("Generating tree from empty virtual function!");
    }
    protected void AddLODs(float LOD0_Distance, float LOD1_Distance)
    {
        if (GetComponent<LODGroup>())
        {
            LOD[] lods = new LOD[2];
            Renderer[] renderers = new Renderer[2 + generatedBranchesParent.childCount];
            renderers[0] = generatedBranch.GetComponent<Renderer>();
            renderers[1] = generatedLeaves.GetComponent<Renderer>();
            for (int i = 2; i < renderers.Length; i++)
            {
                renderers[i] = generatedBranchesParent.GetChild(i-2).GetComponent<Renderer>();
            }

            Renderer[] renderers2 = new Renderer[2];
            renderers2[0] = CreateLODFromMesh(generatedBranch.gameObject, 1).GetComponent<Renderer>();
            renderers2[1] = CreateLODFromMesh(generatedLeaves.gameObject, 1).GetComponent<Renderer>();
            renderers2[0].material.color = generatedBranch.GetComponent<Renderer>().material.color;
            renderers2[1].material.color = generatedLeaves.GetComponent<Renderer>().material.color;

            lods[0] = new LOD(/*0.22f*/LOD0_Distance, renderers);
            lods[1] = new LOD(/*0.025f*/LOD1_Distance, renderers2);

            GetComponent<LODGroup>().SetLODs(lods);
            GetComponent<LODGroup>().RecalculateBounds();
        }
    }
    protected GameObject CreateLODFromMesh(GameObject target, int LOD_Level)
    {
        GameObject goToInstantiate = null;
        GameObject go = null;

        if (target.GetComponent<GeneratedBranch>())
        {
            go = target.GetComponent<GeneratedBranch>().CreateLODObject(target, transform, LOD_Level, target.GetComponent<MeshFilter>().mesh);
            go.transform.position = target.transform.position;
            go.transform.localScale = target.transform.localScale;
            go.GetComponent<GeneratedMesh>().VerySlowlyConvertToFlatShading();
            generatedBranchLOD = go.GetComponent<GeneratedBranch>();
            return go;
        }

        if (target.GetComponent<GeneratedCone>())
        {
            go = target.GetComponent<GeneratedCone>().CreateLODObject(target, transform, LOD_Level, target.GetComponent<MeshFilter>().mesh);
            go.transform.position = target.transform.position;
            go.transform.localScale = target.transform.localScale;
            go.GetComponent<GeneratedMesh>().VerySlowlyConvertToFlatShading();
            return go;
        }

      /*  if (target.GetComponent<GeneratedBranch>())
        {
            goToInstantiate = VegetationGenerator.instance.generatedBranchPrefab.gameObject;
        }
        else */if(target.GetComponent<GeneratedLeaves>())
        {
            goToInstantiate = VegetationGenerator.instance.generatedLeavesPrefab.gameObject;
        }
        else
        {
            Debug.LogError("There is no implementation for creating LOD from this object!", target);
        }

        go = Instantiate(goToInstantiate.gameObject, transform);
        go.name = "LOD_Test";  

        if(target.GetComponent<GeneratedLeaves>())
        {
            go.transform.position = target.transform.position;
            go.transform.localScale = target.transform.localScale;
            go.GetComponent<MeshFilter>().mesh = target.GetComponent<MeshFilter>().mesh;
            go.GetComponent<GeneratedLeaves>().Generate(LOD_Level);
            generatedLeavesLOD = go.GetComponent<GeneratedLeaves>();
        }

        go.GetComponent<GeneratedMesh>().VerySlowlyConvertToFlatShading();
        return go;
    }

    protected IEnumerator MergeChildTrees(float LOD0_Distance, float LOD1_Distance)
    {
        yield return new WaitForEndOfFrame();

        List<Tree> treesToMerge = new List<Tree>();

        if (transform.parent && !transform.parent.GetComponent<Tree>())
        {
            treesToMerge.Add(this);
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<Tree>())
                {
                    treesToMerge.Add(transform.GetChild(i).GetComponent<Tree>());
                }
            }


            //Merge
            int count = treesToMerge.Count;
            if (count > 0)
            {
                List<MeshFilter> meshFiltersBranches = new List<MeshFilter>();
                List<MeshFilter> meshFiltersLeaves = new List<MeshFilter>();

                List<MeshFilter> meshFiltersBranchesLOD = new List<MeshFilter>();
                List<MeshFilter> meshFiltersLeavesLOD = new List<MeshFilter>();
                for (int i = 0; i < count; i++)
                {
                    meshFiltersBranches.Add(treesToMerge[i].generatedBranch.GetComponent<MeshFilter>());
                    meshFiltersBranches.AddRange(treesToMerge[i].generatedBranchesParent.GetComponentsInChildren<MeshFilter>());
                    meshFiltersLeaves.Add(treesToMerge[i].generatedLeaves.GetComponent<MeshFilter>());

                    meshFiltersBranchesLOD.Add(treesToMerge[i].generatedBranchLOD.GetComponent<MeshFilter>());
                    meshFiltersLeavesLOD.Add(treesToMerge[i].generatedLeavesLOD.GetComponent<MeshFilter>());
                }
                GameObject mergedBranches = GeneratedMesh.CombineMeshes(transform, meshFiltersBranches.ToArray());
                mergedBranches.AddComponent<GeneratedBranch>();
                mergedBranches.name = "Newly merged branches";
                generatedBranch = mergedBranches.GetComponent<GeneratedBranch>();

                GameObject mergedLeaves = GeneratedMesh.CombineMeshes(transform, meshFiltersLeaves.ToArray());
                mergedLeaves.AddComponent<GeneratedLeaves>();
                mergedLeaves.name = "Newly merged leaves";
                generatedLeaves = mergedLeaves.GetComponent<GeneratedLeaves>();

                //LOD
                GameObject mergedBranchesLOD = GeneratedMesh.CombineMeshes(transform, meshFiltersBranchesLOD.ToArray());
                mergedBranchesLOD.AddComponent<GeneratedBranch>();
                mergedBranchesLOD.name = "Newly merged branches LOD";
                generatedBranchLOD = mergedBranchesLOD.GetComponent<GeneratedBranch>();

                GameObject mergedLeavesLOD = GeneratedMesh.CombineMeshes(transform, meshFiltersLeavesLOD.ToArray());
                mergedLeavesLOD.AddComponent<GeneratedLeaves>();
                mergedLeavesLOD.name = "Newly merged leaves LOD";
                generatedLeavesLOD = mergedLeavesLOD.GetComponent<GeneratedLeaves>();
            }

            AddLODsNew(LOD0_Distance, LOD1_Distance);
        }
    }
    void AddLODsNew(float LOD0_Distance, float LOD1_Distance)
    {
        if (GetComponent<LODGroup>())
        {

            Renderer[] renderers = new Renderer[2];
            renderers[0] = generatedBranch.GetComponent<Renderer>();
            renderers[1] = generatedLeaves.GetComponent<Renderer>();

            Renderer[] renderers2 = new Renderer[2];
            renderers2[0] = generatedBranchLOD.GetComponent<Renderer>();
            renderers2[1] = generatedLeavesLOD.GetComponent<Renderer>();

            LOD[] lods = new LOD[2];
            lods[0] = new LOD(LOD0_Distance, renderers);
            lods[1] = new LOD(LOD1_Distance, renderers2);

            GetComponent<LODGroup>().SetLODs(lods);
            GetComponent<LODGroup>().RecalculateBounds();
        }
    }


    protected void AddBranches()
    {
        GameObject instance = Instantiate(VegetationGenerator.instance.generatedBranchPrefab.gameObject, transform);
        instance.GetComponent<GeneratedBranch>().Generate(Random.Range(5, 9), Random.Range(1, 6), 1f, 1, null);
        instance.transform.parent = transform;
        instance.transform.localPosition = new Vector3(0, Random.Range(2.5f, generatedBranch.transform.localScale.y * generatedBranch.meshHeight * generatedBranch.mySegmentHeight * 0.95f), 0);
        instance.transform.localScale = new Vector3(Random.Range(0.25f, 0.35f), Random.Range(0.3f, 0.6f)*2f, Random.Range(0.25f, 0.35f));
        instance.transform.localRotation = Quaternion.Euler(new Vector3(Random.Range(35, 65) ,Random.Range(0, 360), 0));
        instance.GetComponent<GeneratedBranch>().VerySlowlyConvertToFlatShading();

        instance.transform.SetParent(generatedBranchesParent);
    }
    protected Tree AddBranchesTrees(GameObject treePrefab, int _generateChildLevel, int branchesAmount, int nestedTreesAmount)
    {
        GameObject instance = Instantiate(treePrefab, transform);
        instance.GetComponent<Tree>().Generate(Random.Range(defaultSubtreeWidthFromTo.x, defaultSubtreeWidthFromTo.y)
            , Random.Range(defaultSubtreeTreeHeightFromTo.x, defaultSubtreeTreeHeightFromTo.y)
            , transform.localScale, 1f, 1, _generateChildLevel, branchesAmount, nestedTreesAmount);

        instance.transform.parent = transform;
        float fixedHeight = generatedBranch.transform.localScale.y * generatedBranch.meshHeight;
        instance.transform.localPosition = new Vector3(0, Random.Range(fixedHeight * 0.2f, fixedHeight * 0.6f) * generatedBranch.mySegmentHeight, 0);
        float randScale = Random.Range(defaultSubtreeScaleFromTo.x, defaultSubtreeScaleFromTo.y);
        instance.transform.localScale = new Vector3(randScale, randScale * transform.localScale.y, randScale);
        instance.transform.localRotation = Quaternion.Euler(new Vector3(Random.Range(30, 45), Random.Range(0, 360), 0));

        return instance.GetComponent<Tree>();
    }

    protected void AdjustTreeLeaves()
    {
        generatedLeaves.transform.localPosition = new Vector3(
              0f
            , generatedBranch.mySegmentHeight * generatedBranch.meshHeight * generatedBranch.transform.localScale.y + generatedLeaves.transform.localScale.y / 2f
            , 0f);
        float height = (generatedBranch.meshHeight * generatedBranch.transform.parent.localScale.y) * 0.5f;
        generatedLeaves.transform.localScale = new Vector3(3f + Random.Range(0f, height), 3f + Random.Range(0f, height), 3f + Random.Range(0f, height));
    }


    private void OnDestroy()
    {
        VegetationGenerator.instance.RemoveTreeData(this);
    }
}