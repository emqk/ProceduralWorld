using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MC_CameraController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float rotateCameraSpeed;
    [SerializeField] float verticalMovementCameraSpeed;
    [SerializeField] float maxCameraHeight;
    [SerializeField] float minCameraHeight;

    [Header("Zoom")]
    [SerializeField] float zoomScrollCameraSpeed;
    [SerializeField] float zoomGoToTargetCameraSpeed;
    [SerializeField] float maxCameraZoom;
    [SerializeField] float minCameraZoom;

    Vector3 targetLookAtPos = new Vector3();

    void Start()
    {
        targetLookAtPos = MC_MeshCreatorManager.instance.spawnPoint.position;
    }

    void Update()
    {
        SmoothLookAtTarget();
    }

    float rotateHorAmount = 0;
    float rotateVerAmount = 2;
    float zoomAmount = 6;
    float prevZoomAmount = 6;
    void SmoothLookAtTarget()
    {
        //Vertical
        rotateVerAmount += Input.GetAxis("Vertical") * verticalMovementCameraSpeed * Time.deltaTime;
        rotateVerAmount = Mathf.Clamp(rotateVerAmount, minCameraHeight, maxCameraHeight);
        targetLookAtPos.y = rotateVerAmount;
        transform.position = new Vector3(transform.position.x, targetLookAtPos.y, transform.position.z);
        
        //Horizontal
        rotateHorAmount = Input.GetAxis("Horizontal") * rotateCameraSpeed * Time.deltaTime;
        transform.eulerAngles = transform.eulerAngles - new Vector3(0,rotateHorAmount,0);

        //Zoom
        zoomAmount -= Input.GetAxis("Mouse ScrollWheel") * zoomScrollCameraSpeed * Time.deltaTime;
        zoomAmount = Mathf.Clamp(zoomAmount, maxCameraZoom, minCameraZoom);
        transform.position = new Vector3(0, transform.position.y, 0) - transform.forward * Mathf.Lerp(prevZoomAmount, zoomAmount, zoomGoToTargetCameraSpeed * Time.deltaTime);
        prevZoomAmount = Vector3.Distance(transform.position, new Vector3(0,transform.position.y, 0));
    }
}