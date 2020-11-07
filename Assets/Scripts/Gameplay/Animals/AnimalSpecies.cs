using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalSpecies : MonoBehaviour
{
    [SerializeField]
    List<Animal> animals = new List<Animal>();
    AnimalSettings.AnimalType animalType;

    [SerializeField]
    List<Building> buildings = new List<Building>();
    
    public Building mainBuilding;

    public GameObject[] palisade;


    Supply supplies = new Supply();

    const float buildingMaxDistanceFromOrigin = 35;

    void Start()
    {
        Debug.Log("New species arrived!");

        animalType = (AnimalSettings.AnimalType)Mathf.RoundToInt(Random.Range(0f, 1f));
        AnimalSettings animalSettings = new AnimalSettings(animalType, Random.Range(1, 4), new Vector3(1, 1, Random.Range(1.5f, 3.5f)));
        for (int i = 0; i < 10; i++)
        {
            CreateNewAnimal(animalSettings);
        }
    }

    bool bp = true;
    void Update()
    {
        if (supplies.woodAmount >= 10)
        {
            if (bp)
            {
                CreatePalisade();
                bp = false;
            }
            else
            {
                CreateBuilding();
            }

            supplies.woodAmount -= 10;
        }
    }

    void CreateBuilding()
    {
        Building building = Instantiate(VegetationGenerator.instance.generatedBuildingPrefab);
        VegetationGenerator.instance.PlaceOnTerrainOnRandomPosInCircle(building.transform, transform.position, buildingMaxDistanceFromOrigin);
        building.mainBlock.transform.localScale = new Vector3(10, 10, 6);
        building.gameObject.AddComponent<NavMeshObstacle>().carving = true;
        building.GetComponent<NavMeshObstacle>().carveOnlyStationary = true;
        building.GetComponent<NavMeshObstacle>().size = building.mainBlock.transform.lossyScale - new Vector3(0.5f, 0, 0.5f);

        buildings.Add(building);
        if (buildings.Count == 1)
        mainBuilding = buildings[0];
    }

    void CreatePalisade()
    {
        Vector3[] palisadePoints = VegetationGenerator.instance.GetPointsAroundCircle(transform.position, (int)buildingMaxDistanceFromOrigin/4, buildingMaxDistanceFromOrigin + 5f);
        float noiseAmount = 2;
        VegetationGenerator.instance.MakeNoiseInVec3(palisadePoints, new Vector3(-noiseAmount, -noiseAmount, -noiseAmount), new Vector3(noiseAmount, noiseAmount, noiseAmount));
        palisade = new GameObject[palisadePoints.Length];
        for (int i = 0; i < palisadePoints.Length; i++)
        {          
            GameObject go = Instantiate(VegetationGenerator.instance.generatedRockPrefab.gameObject);
            go.GetComponent<Rock>().generatedLeaves.GetComponent<GeneratedLeaves>().Generate();
            go.GetComponent<Rock>().generatedLeaves.GetComponent<GeneratedLeaves>().VerySlowlyConvertToFlatShading();
            go.transform.position = new Vector3(palisadePoints[i].x, transform.position.y, palisadePoints[i].z);
            VegetationGenerator.instance.PlaceObjectOnObjectUnderneath(go.transform);
            go.transform.localScale = go.transform.localScale * 2.5f;
            go.AddComponent<NavMeshObstacle>().carving = true;
            go.GetComponent<NavMeshObstacle>().carveOnlyStationary = true;
            go.GetComponent<NavMeshObstacle>().shape = NavMeshObstacleShape.Capsule;
            palisade[i] = go;     
        }
    }

    public void ChangeFoodAmount(int amount)
    {
        supplies.foodAmount += amount;
    }
    public void ChangeWoodAmount(int amount)
    {
        supplies.woodAmount += amount;
    }

    void CreateNewAnimal(AnimalSettings animalSettings)
    {
        Animal animalPrefabToSpawn = null;
        switch (animalSettings.animalType)
        {
            case AnimalSettings.AnimalType.Land:
                animalPrefabToSpawn = AnimalsManager.instance.landAnimalPrefab;
                break;
            case AnimalSettings.AnimalType.Air:
                animalPrefabToSpawn = AnimalsManager.instance.airAnimalPrefab;
                break;
            default:
                break;
        }

        GameObject newAnimal = Instantiate(animalPrefabToSpawn.gameObject);
        newAnimal.transform.position = transform.position;
        newAnimal.transform.SetParent(TerrainGenerator.instance.transform.parent);
        Animal animal = newAnimal.GetComponent<Animal>();
        animal.FirstGeneration(animalSettings);
        animal.SetNest(this);
        animals.Add(animal);
    }
}