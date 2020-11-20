using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class CameraSelector : MonoBehaviour
{
    public Camera arCamera;
    public GameObject selectorCrosshair;
    GameObject currCrosshair;

    GameObject currPrototype;
    bool wasSpawned = false;
    [SerializeField] GameObject prefabPrototype;
    [SerializeField] GameObject prefab;

    [SerializeField] GameObject UIPanel;
    [SerializeField] Slider UISlider;



    void Start()
    {
        currPrototype = Instantiate(prefabPrototype, new Vector3(0, -3, 0), Quaternion.Euler(0, 0, 0));
        WorldGenerator.ChangeObjectScaleToAR(currPrototype.transform);
        ChangePrototypeScaleToSliderValue();
    }

    void Update()
    {
        
        RefreshSelector();

       // if (wasSpawned == false)
       //     SpawnAfterTouch();
    }

    void RefreshSelector()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(arCamera.transform.position, arCamera.transform.forward, out hit, 1000))
        {
            if (wasSpawned)
            {
                currCrosshair.transform.position = hit.point;
            }
            else
            {
                currPrototype.transform.position = hit.point;
            }
        }
    }

  /*  void SpawnAfterTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = touch.position;
            RaycastHit hit = new RaycastHit();

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touchPosition);
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject instance = Instantiate(prefab, currPrototype.transform.position, currPrototype.transform.rotation);
                    instance.transform.localScale = currPrototype.transform.localScale

                    currCrosshair = Instantiate(selectorCrosshair, new Vector3(0, -3, 0), Quaternion.Euler(0, 0, 0));
                    wasSpawned = true;
                    Destroy(currPrototype);
                }
            }
        }
    }*/

    void DestroyAfterTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = touch.position;
            RaycastHit hit = new RaycastHit();

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touchPosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }


    void ChangePrototypeScaleToSliderValue()
    {
        if (currPrototype)
        {
            float sliderAmount = UISlider.value;
            WorldGenerator.ChangeObjectScaleAR(currPrototype.transform, new Vector3(sliderAmount, sliderAmount, sliderAmount));
        }
    }

    void ClearArPlanes()
    {
        //Clear planes
        ARPlaneManager planeManager = GameObject.FindObjectOfType<ARPlaneManager>();
        if (planeManager)
        {
            planeManager.enabled = false;
        }
        ARPlane[] ArPlanes = GameObject.FindObjectsOfType<ARPlane>();
        foreach (var currPlane in ArPlanes)
        {
            Destroy(currPlane.gameObject);
        }

        //Clear point cloud
        ARPointCloudManager pointCloudManager = GameObject.FindObjectOfType<ARPointCloudManager>();
        if (pointCloudManager)
        {
            pointCloudManager.enabled = false;
            foreach (var point in pointCloudManager.trackables)
            {
                Destroy(point.gameObject);
            }
        }
}

    #region UI

    public void SpawnWorld()
    {
        GameObject instance = Instantiate(prefab, currPrototype.transform.position, currPrototype.transform.rotation);
        Vector3 targetScale = currPrototype.transform.localScale/* / 500f*/; /* / 500 because then terrain mesh scale is qual to 1x1x1 */
        instance.transform.localScale *= WorldGenerator.worldGenerator.GetScaleMultiplier();
        //WorldGenerator.worldGenerator.GenerateWorldWithScale(targetScale);

        currCrosshair = Instantiate(selectorCrosshair, new Vector3(0, -3, 0), Quaternion.Euler(0, 0, 0));
        wasSpawned = true;
        Destroy(currPrototype);

        UIPanel.SetActive(false);

        ClearArPlanes();
    }

    public void OnSliderValueChange()
    {
        ChangePrototypeScaleToSliderValue();
    }

    #endregion



}
