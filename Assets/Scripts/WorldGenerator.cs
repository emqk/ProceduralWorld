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
    WorldGenerationProgress currentWorldGenerationProgress = WorldGenerationProgress.Nothing;

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
    public static bool GetIsItAR()
    {
        return worldGenerator.isItAR;
    }

    private void Start()
    {
        GetArParent();

        if (isItAR == false)
        {
            generationProgressText.text = "Generating terrain...";
            Invoke("GenerateWholeWorld", 0.05f);
        }
        else
        {
            transform.parent.SetParent(GameObject.FindGameObjectWithTag("AR_Root").transform);
        }
    }

    public void GenerateWorldWithScale(Vector3 targetScale)
    {
        ChangeObjectScaleAR(transform.parent, targetScale);

        currentWorldGenerationProgress = WorldGenerationProgress.Terrain;
        StartCoroutine(ChangeTerrainParentScale(targetScale));
    }
    private IEnumerator ChangeTerrainParentScale(Vector3 targetScale)
    {
        yield return new WaitForEndOfFrame();
        TerrainGenerator.instance.SetupAndGenerateTerrain();
        currentWorldGenerationProgress = WorldGenerationProgress.Water;
        yield return new WaitForEndOfFrame();
        WaterGenerator.instance.GenerateWater();
        currentWorldGenerationProgress = WorldGenerationProgress.Vegetation;
        yield return new WaitForEndOfFrame();
        StartCoroutine(GenerateVegetationAfterDelay());
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        //transform.parent.localScale = targetScale;
        yield return new WaitForEndOfFrame();
        NavMeshManager.instance.BuildNavMesh();
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
        else
            currentWorldGenerationProgress = WorldGenerationProgress.Vegetation;

        yield return new WaitForEndOfFrame();
        VegetationGenerator.instance.GenerateSomeRandomVegetation();

            StartCoroutine(GenerateNavMeshAfterDelay());
    }

    private IEnumerator GenerateNavMeshAfterDelay()
    {
        if(isItAR == false)
            generationProgressText.text = "Generating AI...";
        else
            currentWorldGenerationProgress = WorldGenerationProgress.AI;

        yield return new WaitForEndOfFrame();
        NavMeshManager.instance.BuildNavMesh();
        AnimalsManager.instance.Generate();

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        if (isItAR == false)
            generationPanel.SetActive(false);
        else
            currentWorldGenerationProgress = WorldGenerationProgress.Done;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height-200;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 100, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 25;
        style.normal.textColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        string text = currentWorldGenerationProgress.ToString();
        GUI.Label(rect, text, style);
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
