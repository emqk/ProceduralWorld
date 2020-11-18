using UnityEngine;

public class AnimalCarrying
{
    public bool isCarrying = false;
    public int carryingAmount;

    public void SetupCarrying(int amount)
    {
        isCarrying = true;
        carryingAmount = amount;
    }

    public void FinishCarrying()
    {
        isCarrying = false;
        carryingAmount = 0;
    }
}

public class Animal : GeneratedAnimal
{
    protected AnimalSpecies nest;
    protected AnimalCarrying animalCarrying = new AnimalCarrying();

    protected InteractionTarget interactionTarget;
    protected const float interactionDistance = 2f;
    protected Vector3 targetPos = new Vector3(0, 0, 0);

    public void FirstGeneration(AnimalSettings _animalSettings)
    {
        animalSettings = _animalSettings;
        GenerateAnimal(animalSettings);

        AddLODs();
    }
    void AddLODs()
    {
        if (GetComponent<LODGroup>())
        {
           // Debug.Log("I am in LOD group and my type is: " + animalSettings.animalType);

            LOD[] lods = new LOD[2];
            Renderer[] renderers = new Renderer[1 + legs.Length];
            for (int i = 0; i < legs.Length; i++)
            {
                renderers[i] = legs[i].GetComponent<Renderer>();
            }
            renderers[legs.Length] = body.GetComponent<Renderer>();

            Renderer[] renderers2 = new Renderer[1 + legs.Length];
            for (int i = 0; i < legs.Length; i++)
            {
                renderers2[i] = legs[i].CreateLODObject(gameObject, legs[i].transform, 2, legs[i].GetComponent<MeshFilter>().mesh).GetComponent<Renderer>();
                renderers2[i].transform.localPosition = new Vector3(0,0,0)/*legs[i].transform.localPosition*/;
                renderers2[i].transform.localRotation = Quaternion.Euler(new Vector3(0,0,0))/*legs[i].transform.localRotation*/;
                renderers2[i].transform.localScale = new Vector3(1,1,1)/*legs[i].transform.localScale*/;
                renderers2[i].GetComponent<Renderer>().materials = legs[i].GetComponent<Renderer>().materials;
                legs[i].VerySlowlyConvertToFlatShading();
                renderers2[i].GetComponent<GeneratedMesh>().VerySlowlyConvertToFlatShading();
            }
            renderers2[legs.Length] = body.CreateLODObject(gameObject, body.transform, 0, null/*body.GetComponent<MeshFilter>().mesh*/).GetComponent<Renderer>();
            renderers2[legs.Length].transform.localScale = new Vector3(1, 1, 1);
            renderers2[legs.Length].GetComponent<Renderer>().materials = body.GetComponent<Renderer>().materials;
            renderers2[legs.Length].GetComponent<GeneratedMesh>().VerySlowlyConvertToFlatShading();

            lods[0] = new LOD(0.08f, renderers);
            lods[1] = new LOD(0.005f, renderers2);

            GetComponent<LODGroup>().SetLODs(lods);
            GetComponent<LODGroup>().RecalculateBounds();
        }
    }

    public void SetNest(AnimalSpecies animalSpecies)
    {
        nest = animalSpecies;
    }

    protected void ControlMovementAnimations()
    {
        float distToTargetSq = (transform.position - targetPos).sqrMagnitude;
        if (distToTargetSq > interactionDistance * interactionDistance)
            PlayMovementAnimation();
    }

    protected bool MoveToTree()
    {
        if (!interactionTarget)
        {
           // Debug.Log("Hello, im looking for tree");
            SetupNearestTreeAsTarget();
            return true;
        }
        if (!interactionTarget)
        {
            // Debug.Log("I can't find tree!");
            return false;
        }

        float distToTarget = (transform.position - targetPos).sqrMagnitude;
        if (distToTarget <= 8f)
        {
            interactionTarget.GetComponent<Tree>().TakeWood(ref animalCarrying);
            interactionTarget.RemoveAnimal(this);
        }

        return false;
    }
    void SetupNearestTreeAsTarget()
    {
        Tree nearestTree = VegetationGenerator.instance.GetReadyToInteractNearestTree(transform.position);
        if (!nearestTree)
            return;

        interactionTarget = nearestTree.GetComponent<InteractionTarget>();
        interactionTarget.AddAnimal(this);
        targetPos = interactionTarget.transform.position;
    }


    protected void MoveToBush()
    {
        if (!interactionTarget)
        {
            //Debug.Log("Hello, im looking for bush");
            SetupNearestBushAsTarget();
        }
        if (!interactionTarget)
        {
            //Debug.Log("I can't find bush with food!");
            return;
        }

        float distToTarget = (transform.position - targetPos).sqrMagnitude;
        if (distToTarget <= 8f)
        {
            interactionTarget.GetComponent<Bush>().TakeFood(ref animalCarrying);
            interactionTarget.RemoveAnimal(this);
        }
    }
    void SetupNearestBushAsTarget()
    {
        Bush nearestBush = VegetationGenerator.instance.GetReadyToInteractNearestBushWithFood(transform.position);
        if (!nearestBush)
            return;

        interactionTarget = nearestBush.GetComponent<InteractionTarget>();
        interactionTarget.AddAnimal(this);
        targetPos = interactionTarget.transform.position;
    }

    protected bool MoveToNest()
    {
        if (nest.mainBuilding)
            targetPos = nest.mainBuilding.entrance.position;
        else
            targetPos = nest.transform.position;

        float distToTarget = (transform.position - targetPos).sqrMagnitude;
        if (distToTarget <= 8f)
        {
            nest.ChangeWoodAmount(animalCarrying.carryingAmount);

            interactionTarget = null;
            animalCarrying.FinishCarrying();
        }

        return false;
    }

    void PlayMovementAnimation()
    {
        int count = legs.Length;
        for (int i = 0; i < count; i++)
        {
            legs[i].transform.localEulerAngles = new Vector3(-Mathf.PingPong(Time.time * 30, 20), legs[i].transform.localEulerAngles.y, legs[i].transform.localEulerAngles.z);
        }

       //const float height = 0.25f;
        //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + Mathf.PingPong(Time.time * 0.75f, height), transform.localPosition.z);
    }

    private void OnDestroy()
    {
        if(interactionTarget)
            interactionTarget.RemoveAnimal(this);
    }
}