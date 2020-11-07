using UnityEngine;

[System.Serializable]
public class AnimalSettings
{
    public enum AnimalType
    {
        Land, Air
    }
    public AnimalType animalType;

    public int legRows;
    public Vector3 scale;

    public AnimalSettings() { }

    public AnimalSettings(AnimalType _animalType, int _legRows, Vector3 _scale)
    {
        animalType = _animalType;
        legRows = _legRows;
        scale = _scale;
    }
}
