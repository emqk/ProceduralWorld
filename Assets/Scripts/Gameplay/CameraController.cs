using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float movementSpeed;

    [SerializeField]
    float lookSpeed;

    bool useMobileInput;
    [SerializeField] GameObject mobileControllerParent;

    private void Awake()
    {
        #if UNITY_ANDROID
            useMobileInput = true;
            mobileControllerParent.SetActive(true);
        #endif
    }

    void Update()
    {
        if (useMobileInput == false)
        {
            Movement();
            CameraLook();
        }
    }

    float forwardInput = 0;
    float horizontalInput = 0;
    void Movement()
    {
        if(!useMobileInput)
        {
            forwardInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");
        }
        Vector3 moveVec = (transform.forward * forwardInput + transform.right * horizontalInput) * movementSpeed * Time.deltaTime;
        transform.position += moveVec;
    }

    float mouseX = 0;
    float mouseY = 0;
    void CameraLook()
    {
        if (!useMobileInput)
        {
            mouseX += Input.GetAxis("Mouse X");
            mouseY += Input.GetAxis("Mouse Y");      
        }

        transform.rotation = Quaternion.Euler(new Vector3(mouseY, mouseX, 0));
    }
}
