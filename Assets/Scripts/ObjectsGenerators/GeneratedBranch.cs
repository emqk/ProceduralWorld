﻿using UnityEngine;

public class GeneratedBranch : GeneratedMesh
{
    public float mySegmentHeight = 1;


    public void Generate(int width, int height, float radius, float segmentHeight, Mesh parentMesh, float thinMultiplierMin = 0.85f, float thinMultiplierMax = 0.97f)
    {
        mySegmentHeight = segmentHeight;
        Setup();
        //GenerateCylinder(24, 6, 1f);
        GenerateCylinder(width, height, radius, segmentHeight, parentMesh, thinMultiplierMin, thinMultiplierMax);

        //GetComponent<MeshFilter>().mesh.RecalculateNormals();
        //VerySlowlyConvertToFlatShading();
    }

    public override GameObject CreateLODObject(GameObject target, Transform parent, int LOD_Level, Mesh parentMesh)
    {
        GameObject go = Instantiate(VegetationGenerator.instance.generatedBranchPrefab.gameObject, parent);
        go.name = "Overrided LOD";

        //Debug.Log("I have to handle LOD_Level! parent ");

        go.transform.localPosition = target.transform.position;
        go.transform.localScale = target.transform.localScale;
        go.GetComponent<GeneratedBranch>().Generate(meshWidth /* / LOD_Level*/, meshHeight, 1f, 1f, parentMesh);

        return go;
    }
}