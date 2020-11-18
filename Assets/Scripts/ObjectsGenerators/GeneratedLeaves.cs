using UnityEngine;

public class GeneratedLeaves : GeneratedMesh
{
    /*  void Awake()
      {
          Setup();
          GenerateIcoSphere();
          VerySlowlyConvertToFlatShading();
      }*/

    public void Generate(int recursionLevel = 2)
    {
        Setup();
        GenerateIcoSphere(recursionLevel);
        //VerySlowlyConvertToFlatShading(); 
    }

    public override GameObject CreateLODObject(GameObject target, Transform parent, int recursion_Level, Mesh parentMesh)
    {
        GameObject go = Instantiate(VegetationGenerator.instance.generatedLeavesPrefab.gameObject, parent);
        go.name = "Overrided LOD_TEST";

        go.transform.position = target.transform.position;
        go.transform.localScale = target.transform.localScale;
        go.GetComponent<GeneratedLeaves>().Generate(recursion_Level);
        if(target.GetComponent<Renderer>())
            go.GetComponent<GeneratedLeaves>().GetComponent<Renderer>().material.color = target.GetComponent<Renderer>().material.color;

        return go;
    }
}
