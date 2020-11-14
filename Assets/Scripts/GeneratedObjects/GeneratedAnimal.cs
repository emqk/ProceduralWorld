using UnityEngine;

public class GeneratedAnimal : MonoBehaviour
{
    public GeneratedLeaves body;
    public GeneratedBranch[] legs;

    protected AnimalSettings animalSettings = new AnimalSettings();

    protected void GenerateAnimal(AnimalSettings _animalSettings)
    {
        animalSettings = _animalSettings;

        switch (animalSettings.animalType)
        {
            case AnimalSettings.AnimalType.Land:
                GenerateLandAnimal();
                break;
            case AnimalSettings.AnimalType.Air:
                GenerateSkyAnimal();
                break;
            default:
                break;
        }
    }
    void GenerateLandAnimal()
    {
        body.Generate(1);
        AdjustBody();
        body.VerySlowlyConvertToFlatShading();

        legs = new GeneratedBranch[animalSettings.legRows * 2];
        int count = legs.Length;
        for (int i = 0; i < count; i++)
        {
            legs[i] = Instantiate(AnimalsManager.instance.animalLegPrefab, transform).GetComponent<GeneratedBranch>();
            legs[i].Generate(6, 2, 1, 1, null);
           // legs[i].VerySlowlyConvertToFlatShading();
        }

        AdjustLegs();
    }
    void GenerateSkyAnimal()
    {
        body.Generate(1);
        body.VerySlowlyConvertToFlatShading();
    }

    void AdjustBody()
    {
        body.transform.localScale = new Vector3(animalSettings.scale.x * Random.Range(0.9f, 1.1f), animalSettings.scale.y * Random.Range(0.9f, 1.1f), animalSettings.scale.z * Random.Range(0.9f, 1.1f));
    }

    void AdjustLegs()
    {
        float offsetBetweenLegsZ = body.transform.localScale.z;
        Vector3 currLegOffset = new Vector3(0.35f, -0.4f, -offsetBetweenLegsZ/2f);

        int count = legs.Length;
        for (int i = 0; i < count; i+=2)
        {
            legs[i].transform.localPosition = currLegOffset;
            legs[i].transform.localScale = new Vector3(0.2f, 0.9f, 0.2f);
            legs[i].transform.localEulerAngles = new Vector3(180, 0, 0);

            legs[i+1].transform.localPosition = new Vector3(-currLegOffset.x, currLegOffset.y, currLegOffset.z);
            legs[i+1].transform.localScale = new Vector3(0.2f, 0.9f, 0.2f);
            legs[i+1].transform.localEulerAngles = new Vector3(180, 0, 0);

            currLegOffset += new Vector3(0, 0, offsetBetweenLegsZ/(legs.Length/2-1));
        }
    }
}
