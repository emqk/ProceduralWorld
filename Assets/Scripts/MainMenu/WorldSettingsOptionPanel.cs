using UnityEngine;
using UnityEngine.UI;

public enum TerrainSettingType
{
    NormalTree, TallTree, ChristmasTree,
    Rock,
    Bush,
    Grass,
    Flower,
    terrainNoiseScale, terrainOctaves, terrainPersistance, terrainLacunarity
}

public class WorldSettingsOptionPanel : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] TerrainSettingType vegetationType;

    private void Awake()
    {
        ChangeTargetTypeValue(float.Parse(inputField.text));
        inputField.onEndEdit.AddListener(OnFieldChange);
    }

    public void OnFieldChange(string arg0)
    {
        Debug.Log("End");
        ChangeTargetTypeValue(float.Parse(inputField.text));
    }

    void ChangeTargetTypeValue(float value)
    {
        NewWorldData.countDictionary[vegetationType] = value;
    }
}
