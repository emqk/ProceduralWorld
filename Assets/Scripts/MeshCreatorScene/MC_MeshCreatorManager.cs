using UnityEngine;
using UnityEngine.UI;

public class MC_MeshCreatorManager : MonoBehaviour
{
    [HideInInspector] public Transform spawnPoint;
    GameObject latelyCreatedObject;

    [Header("UI settings")]
    [SerializeField] GameObject panelsParent;

    [Header("Export UI settings")]
    [SerializeField] GameObject settingsPanel;
    [SerializeField] InputField settingsInputField;

    public static MC_MeshCreatorManager instance;

    enum ObjType
    {
        NormalTree, TallTree, ChristmasTree, Bush, Rock, Grass, Flower
    }
    ObjType currObjType;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0)
        {
            CreateMesh();
        }
    }

    public void ExportMesh()
    {
        GameObject[] go = new GameObject[1];
        go[0] = latelyCreatedObject;
        MeshExporter.ExportGameObjects(go, settingsInputField.text);
    }

    void CreateMesh()
    {
        if(latelyCreatedObject)
            Destroy(latelyCreatedObject);

        GameObject instance = CreateTreeByEnum(currObjType);
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation= spawnPoint.rotation;

        latelyCreatedObject = instance;
    }
    GameObject CreateTreeByEnum(ObjType treeType)
    {
        switch (treeType)
        {
            case ObjType.NormalTree:
                return VegetationGenerator.instance.SpawnTree(false);
            case ObjType.TallTree:
                return VegetationGenerator.instance.SpawnTallTree(false);
            case ObjType.ChristmasTree:
                return VegetationGenerator.instance.SpawnChristmasTree(false);
            case ObjType.Bush:
                return VegetationGenerator.instance.SpawnBush(false);
            case ObjType.Rock:
                return VegetationGenerator.instance.SpawnRock(false);
            case ObjType.Grass:
                return VegetationGenerator.instance.SpawnGrass(false);
            case ObjType.Flower:
                return VegetationGenerator.instance.SpawnFlower(false);
            default:
                return null;
        }
    }


    public void EnableOnlyOneObjectInParent(Transform objToEnable)
    {
        bool targetBool = !objToEnable.gameObject.activeSelf;

        int count = objToEnable.parent.childCount;
        for (int i = 0; i < count; i++)
        {
            objToEnable.parent.GetChild(i).gameObject.SetActive(false);
        }

        objToEnable.gameObject.SetActive(targetBool);
    }

    public void SetCurrObjType(int objType)
    {
        currObjType = (ObjType)objType;
    }
}
