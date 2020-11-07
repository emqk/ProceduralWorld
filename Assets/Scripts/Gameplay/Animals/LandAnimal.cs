using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LandAnimal : Animal
{
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (animalCarrying.isCarrying)
        {
            MoveToNest();
        }
        else
        {
            MoveToTree();
            //MoveToBush();
        }

        LandMovement();
        ControlMovementAnimations();
    }


    void LandMovement()
    {
        agent.SetDestination(targetPos);
    }
}
