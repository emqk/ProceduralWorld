using System.Collections.Generic;
using UnityEngine;

public class InteractionTarget : MonoBehaviour
{
    List<Animal> interactingAnimals = new List<Animal>();

    public void AddAnimal(Animal animal)
    {
        interactingAnimals.Add(animal);
    }

    public void RemoveAnimal(Animal animal)
    {
        interactingAnimals.Remove(animal);
    }

    public bool IsSomeoneInteracting()
    {
        return interactingAnimals.Count > 0;
    }

    public int GetInteractingAnimalsAmount()
    {
        return interactingAnimals.Count;
    }
}
