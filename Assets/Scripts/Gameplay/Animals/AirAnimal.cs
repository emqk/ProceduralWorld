using UnityEngine;

public class AirAnimal : Animal
{
    float targY;
    float flyHeight = 30;
    float flyHorizontalSpeed = 5;
    float flyVerticalSpeed = 5;

    private void Start()
    {
        flyHeight *= WorldGenerator.worldGenerator.GetScaleMultiplier();
        flyHorizontalSpeed *= WorldGenerator.worldGenerator.GetScaleMultiplier();
        flyVerticalSpeed *= WorldGenerator.worldGenerator.GetScaleMultiplier();

        interactionDistance *= WorldGenerator.worldGenerator.GetScaleMultiplier();
        distToCollect *= WorldGenerator.worldGenerator.GetScaleMultiplier();
    }

    void Update()
    {
        if (animalCarrying.isCarrying)
        {
            MoveToNest();
        }
        else
        {
            MoveToBush();
        }

        SkyMovement();   
        ControlMovementAnimations();
    }

    void SkyMovement()
    {
        float distXZSq = (new Vector2(transform.position.x, transform.position.z) - new Vector2(targetPos.x, targetPos.z)).sqrMagnitude;
        if (distXZSq > interactionDistance * interactionDistance)
        {
            targY = flyHeight;
        }
        else
        {
            targY = targetPos.y;
        }
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, transform.position.y, targetPos.z), Time.deltaTime * flyHorizontalSpeed);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targY, transform.position.z), Time.deltaTime * flyVerticalSpeed);
    }
}