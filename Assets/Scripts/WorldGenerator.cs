using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class WorldGenerator : MonoBehaviour
{
    [SerializeField] bool isItAR;
    [SerializeField] GameObject generationPanel;
    [SerializeField] Text generationProgressText;

    static Transform arParent = null;
    public static WorldGenerator worldGenerator;

    enum WorldGenerationProgress
    {
        Nothing, Terrain, Water, Vegetation, AI, Done
    }

    private void Awake()
    {
        worldGenerator = this;
    }


    static Transform GetArParent()
    {
        /*if (GetIsItAR() == false)
            return null;
        */

        GameObject arRoot = GameObject.FindGameObjectWithTag("AR_Root");
        if (arRoot == null)
            return null;

        if (arParent)
            return arParent;
        else
            return arParent = arRoot.transform;
    }
    public static float GetScaleMultiplier()
    {
        return GetIsItAR() ? 0.01f : 1;
    }

    public static bool GetIsItAR()
    {
        if (!worldGenerator)
            return false;

        return worldGenerator.isItAR;
    }

    private void Start()
    {
        GetArParent();

        if (isItAR)
        {
            //transform.parent.SetParent(GameObject.FindGameObjectWithTag("AR_Root").transform);
        }
        else
            generationProgressText.text = "Generating terrain...";

        Invoke("GenerateWholeWorld", 0.05f);

        //if (isItAR == false)
        //{
        //    generationProgressText.text = "Generating terrain...";
        //    Invoke("GenerateWholeWorld", 0.05f);
        //}
        //else
        //{
        //    transform.parent.SetParent(GameObject.FindGameObjectWithTag("AR_Root").transform);
        //}
    }

    public void GenerateWorldWithScale(Vector3 targetScale)
    {
        ChangeObjectScaleAR(transform.parent, targetScale);

       // StartCoroutine(ChangeTerrainParentScale(targetScale));
    }
    private IEnumerator ChangeTerrainParentScale(Vector3 targetScale)
    {
        yield return new WaitForEndOfFrame();
        TerrainGenerator.instance.SetupAndGenerateTerrain();
        yield return new WaitForEndOfFrame();
        WaterGenerator.instance.GenerateWater();

        yield return new WaitForEndOfFrame();
        StartCoroutine(GenerateVegetationAfterDelay());
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        //transform.parent.localScale = targetScale;
        yield return new WaitForEndOfFrame();
        //NavMeshManager.instance.BuildNavMesh();
    }

    void GenerateWholeWorld()
    {
        TerrainGenerator.instance.SetupAndGenerateTerrain();
        WaterGenerator.instance.GenerateWater();

        StartCoroutine(GenerateVegetationAfterDelay());
    }

    private IEnumerator GenerateVegetationAfterDelay()
    {
        if(isItAR == false)
          generationProgressText.text = "Generating Vegetation...";

        yield return new WaitForEndOfFrame();
        VegetationGenerator.instance.GenerateSomeRandomVegetation();
        StartCoroutine(GenerateNavMeshAfterDelay());
    }

    private IEnumerator GenerateNavMeshAfterDelay()
    {
        if(isItAR == false)
            generationProgressText.text = "Generating AI...";

        yield return new WaitForEndOfFrame();
        NavMeshManager.instance.BuildNavMesh();
        yield return new WaitForEndOfFrame();
        AnimalsManager.instance.Generate();

        yield return new WaitForEndOfFrame();

        if(isItAR == false)
            generationProgressText.text = "Optimizing...";

        yield return new WaitForEndOfFrame();
        VegetationGenerator.instance.OptimizeMeshes();
        yield return new WaitForEndOfFrame();

        if (isItAR == false)
            generationPanel.SetActive(false);
    }

    public static void ChangeObjectScaleToAR(Transform targetTrans)
    {
        //Debug.Log("Ar parent: " + GetArParent());
        Vector3 originalScale = targetTrans.transform.localScale;
        targetTrans.SetParent(GetArParent());
        ChangeObjectScaleAR(targetTrans, originalScale);
    }

    public static void ChangeObjectScaleAR(Transform targetTrans, Vector3 targetScale)
    {
        targetTrans.transform.localScale = targetScale;
    }
}
