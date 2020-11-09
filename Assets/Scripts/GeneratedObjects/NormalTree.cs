using UnityEngine;

public class NormalTree : Tree
{
    public override void Generate(int width, int height, Vector3 localScale, float radius, int generateChildsLevels, int branchesAmount, int nestedTreesAmount)
    {
       // Debug.Log("-------------> IN " + branchesAmount);
        generatedBranch.Generate(width, height, radius, null);
        generatedBranch.transform.localScale = new Vector3(generatedBranch.transform.localScale.x, generatedBranch.transform.localScale.y * 2, generatedBranch.transform.localScale.z);
        generatedLeaves.Generate();

        AdjustTreeLeaves();

        if (generateChildsLevels > 0)
        {
            generateChildsLevels--;
            for (int i = 0; i < nestedTreesAmount; i++)
            {
                AddBranchesTrees(VegetationGenerator.instance.generatedNormalTreePrefab, generateChildsLevels, branchesAmount, nestedTreesAmount);
            }
        }
        else
        {
            for (int i = 0; i < branchesAmount; i++)
            {
                AddBranches();
            }
        }

        AddLODs(0.125f, 0.025f);

        generatedBranch.VerySlowlyConvertToFlatShading();
        generatedLeaves.VerySlowlyConvertToFlatShading();

        //Debug.Log("Change tree scale(from start)");
        transform.localScale = localScale;

        //Debug.Log("Tree branch verts: " + generatedBranch.GetComponent<MeshFilter>().mesh.vertices.Length + "Tree branch tris: " + generatedBranch.GetComponent<MeshFilter>().mesh.triangles.Length);
        //Debug.Log("Tree leaves verts: " + generatedLeaves.GetComponent<MeshFilter>().mesh.vertices.Length + "Tree leaves tris: " + generatedLeaves.GetComponent<MeshFilter>().mesh.triangles.Length);


        StartCoroutine(MergeChildTrees(0.1f, 0.025f));
    }

    public static int GetDefaultTreeBrachesAmount()
    {
        return Random.Range(0, 6);
    }
    public static int GetDefaultTreeNestedTreesAmount()
    {
        return Random.Range(0, 4);
    }
}