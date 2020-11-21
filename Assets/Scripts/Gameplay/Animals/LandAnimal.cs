using UnityEngine;
using UnityEngine.AI;

public class LandAnimal : Animal
{
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed *= WorldGenerator.GetScaleMultiplier();

        interactionDistance *= WorldGenerator.GetScaleMultiplier();
        distToCollect *= WorldGenerator.GetScaleMultiplier();
    }

    void Update()
    {
        if (animalCarrying.isCarrying)
        {
            Vector3 currTargetPos = targetPos;
            MoveToNest();
            if(currTargetPos != targetPos)
                agent.SetDestination(targetPos);
        }
        else
        {
            if(MoveToTree())
                agent.SetDestination(targetPos);
        }

        ControlMovementAnimations();
    }
}
