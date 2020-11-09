using UnityEngine;

public class GeneratedCone : GeneratedMesh
{
    public void Generate(int width, float radius, Mesh parentMesh)
    {
        Setup();
        GenerateCone(width, radius, parentMesh);
    }

    public override GameObject CreateLODObject(GameObject target, Transform parent, int LOD_Level, Mesh parentMesh)
    {
        GameObject go = Instantiate(VegetationGenerator.instance.generatedConeLeavesPrefab.gameObject, parent);
        go.name = "Overrided LOD";

        //Debug.Log("I have to handle LOD_Level! parent ");

        go.transform.localPosition = target.transform.position;
        go.transform.localScale = target.transform.localScale;
        go.GetComponent<GeneratedCone>().Generate(meshWidth, 1f, parentMesh);

        return go;
    }
}