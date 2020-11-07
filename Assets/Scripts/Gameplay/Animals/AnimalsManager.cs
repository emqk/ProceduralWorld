using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AnimalsManager : MonoBehaviour
{
    public Animal landAnimalPrefab;
    public Animal airAnimalPrefab;

    [SerializeField]
    List<AnimalSpecies> animalSpecies;

    public GeneratedMesh animalLegPrefab;

    float eachDimensionSpeciesGenerationOffset = 5f;

    public static AnimalsManager instance;

    private void Awake()
    {
        instance = this;
    }
/*
    void Start()
    {
        Generate();
    }
    */
    public void Generate() 
    {
        StartCoroutine(WaitUntilNextFrame());
    }

    IEnumerator WaitUntilNextFrame()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < 3; i++)
        {
            CreateNewSpecies();
        }
    }

    void CreateNewSpecies()
    {
        GameObject newSpecies = new GameObject("Animal species");
        VegetationGenerator.instance.PlaceOnTerrainOnRandomPos(newSpecies.transform);
        newSpecies.AddComponent<AnimalSpecies>();
    }
}
