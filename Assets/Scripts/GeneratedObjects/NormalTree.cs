using UnityEngine;

public class NormalTree : Tree
{
    public override void Generate(int width, int height, Vector3 localScale, float radius, float segmentHeight, int generateChildsLevels, int branchesAmount, int nestedTreesAmount)
    {
       // Debug.Log("-------------> IN " + branchesAmount);
        generatedBranch.Generate(width, height, radius, segmentHeight, null);
        generatedBranch.transform.localScale = new Vector3(generatedBranch.transform.localScale.x, generatedBranch.transform.localScale.y * 2, generatedBranch.transform.localScale.z);
        generatedLeaves.Generate();
        generatedBranch.GetComponent<Renderer>().material.color = new Color(Random.Range(0.4f, 0.65f), Random.Range(0.284f, 0.35f), Random.Range(0.0f, 0.26f));
        generatedLeaves.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.7f, 1.0f), Random.Range(0.0f, 0.35f));

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

    public static Vector2Int GetDefaultTreeBrachesAmount()
    {
        return new Vector2Int(0, 3);
    }
    public static Vector2Int GetDefaultTreeNestedTreesAmount()
    {
        return new Vector2Int(0, 3);
    }
}