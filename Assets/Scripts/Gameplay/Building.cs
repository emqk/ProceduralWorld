using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GeneratedCube mainBlock;

    public Transform entrance;

    void Start()
    {
        mainBlock.Generate();
        mainBlock.VerySlowlyConvertToFlatShading();
        SetupEntrance();
    }


    void SetupEntrance()
    {
        entrance.position = new Vector3(mainBlock.transform.position.x, mainBlock.transform.position.y, mainBlock.transform.position.z-mainBlock.transform.localScale.z/2f-1.5f);
    }
}
