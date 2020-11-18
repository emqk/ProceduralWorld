using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChristmasTree : Tree
{


    //public GameObject leaves1;

    public override void Generate(int width, int height, Vector3 localScale, float radius, float segmentHeight, int generateChildsLevels, int branchesAmount, int nestedTreesAmount)
    {
        //Debug.Log("-------------> IN " + branchesAmount);
        generatedBranch.Generate(width, 1, radius, segmentHeight, null);
        generatedBranch.transform.localScale = new Vector3(generatedBranch.transform.localScale.x, generatedBranch.transform.localScale.y * 2, generatedBranch.transform.localScale.z);
        //generatedLeaves.Generate();

        CreateLeaves(width, height);

        generatedBranch.VerySlowlyConvertToFlatShading();
        //generatedLeaves.VerySlowlyConvertToFlatShading();

        transform.localScale = localScale;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 1.5f, transform.localScale.z);
    }

    void CreateLeaves(int width, int leavesAmount)
    {
        float startPosY = 1.5f;
        float startScale = leavesAmount;
        int leavesWidth = width;

        GeneratedCone[] generatedLeavesLOD0 = new GeneratedCone[leavesAmount];
        GeneratedCone firstLeaves = Instantiate(VegetationGenerator.instance.generatedConeLeavesPrefab, transform);
        firstLeaves.Generate(leavesWidth, 1, null);
        //firstLeaves.VerySlowlyConvertToFlatShading();
        firstLeaves.transform.localPosition = new Vector3(0, startPosY, 0);
        firstLeaves.transform.localScale *= startScale;
        startScale = 1 * 0.8f;
        generatedLeavesLOD0[0] = firstLeaves;
        firstLeaves.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.7f, 1.0f), Random.Range(0.0f, 0.35f));

        //leaves1 = firstLeaves.gameObject;

        //Creating leaves for LOD0
        for (int i = 1; i < leavesAmount; i++)
        {
            GeneratedCone newLeaves = Instantiate(VegetationGenerator.instance.generatedConeLeavesPrefab, generatedLeavesLOD0[i-1].transform /*transform*/);
            newLeaves.Generate(leavesWidth, 1, null);
            //newLeaves.VerySlowlyConvertToFlatShading();

            newLeaves.transform.localPosition = newLeaves.transform.up * 0.575f /*new Vector3(0,0,0)*/;
            newLeaves.transform.localScale = new Vector3(1f,1.25f,1f) * startScale / firstLeaves.transform.parent.localScale.x;
            newLeaves.transform.localRotation = Quaternion.Euler(new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f)));
            startScale *= 0.9f;
            generatedLeavesLOD0[i] = newLeaves;
            newLeaves.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.7f, 1.0f), Random.Range(0.0f, 0.35f));
        }

        //Creating leaves for LOD1
        GameObject[] generatedLeavesLOD1 = new GameObject[leavesAmount];
        int count = generatedLeavesLOD0.Length;
        generatedLeavesLOD1[0] = CreateLODFromMesh(generatedLeavesLOD0[0].gameObject, 1);
        for (int i = 1; i < count; i++)
        {
            generatedLeavesLOD1[i] = CreateLODFromMesh(generatedLeavesLOD0[i].gameObject, 1);
            generatedLeavesLOD1[i].transform.parent = generatedLeavesLOD1[i - 1].transform;
            generatedLeavesLOD1[i].transform.localScale = generatedLeavesLOD0[i].transform.localScale;
            generatedLeavesLOD1[i].transform.localRotation = generatedLeavesLOD0[i].transform.localRotation;
            generatedLeavesLOD1[i].GetComponent<Renderer>().material.color = generatedLeavesLOD0[i].GetComponent<Renderer>().material.color;
        }

        //Making each leave flat-shaded
        for (int i = 0; i < count; i++)
        {
            generatedLeavesLOD0[i].GetComponent<GeneratedCone>().VerySlowlyConvertToFlatShading();
            generatedLeavesLOD1[i].GetComponent<GeneratedCone>().VerySlowlyConvertToFlatShading();
        }


        GameObject mergedLeavesLOD0;
        GameObject mergedLeavesLOD1;

        //Merge leaves objects to one mesh LOD0
        count = generatedLeavesLOD0.Length;
        MeshFilter[] meshFilters = new MeshFilter[count];
        for (int i = 0; i < count; i++)
        {
            meshFilters[i] = generatedLeavesLOD0[i].GetComponent<MeshFilter>();
        }
        mergedLeavesLOD0 = GeneratedMesh.CombineMeshes(transform, meshFilters);

        //Merge leaves objects to one mesh LOD1
        count = generatedLeavesLOD0.Length;
        meshFilters = new MeshFilter[count];
        for (int i = 0; i < count; i++)
        {
            meshFilters[i] = generatedLeavesLOD1[i].GetComponent<MeshFilter>();
        }
        mergedLeavesLOD1 = GeneratedMesh.CombineMeshes(transform, meshFilters);


        AddLODs(0.105f, 0.025f, ref mergedLeavesLOD0, ref mergedLeavesLOD1);
    }


    void AddLODs(float LOD0_Distance, float LOD1_Distance, ref GameObject LOD0, ref GameObject LOD1)
    {
        if (GetComponent<LODGroup>())
        {
            LOD[] lods = new LOD[2];

            Renderer[] renderers = new Renderer[2];
            renderers[0] = generatedBranch.GetComponent<Renderer>();
            renderers[1] = LOD0.GetComponent<Renderer>();

            Renderer[] renderers2 = new Renderer[2];
            renderers2[0] = CreateLODFromMesh(generatedBranch.gameObject, 1).GetComponent<Renderer>();
            renderers2[1] = LOD1.GetComponent<Renderer>();

            lods[0] = new LOD(LOD0_Distance, renderers);
            lods[1] = new LOD(LOD1_Distance, renderers2);

            GetComponent<LODGroup>().SetLODs(lods);
            GetComponent<LODGroup>().RecalculateBounds();
        }
    }
}
