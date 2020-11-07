using UnityEngine;

public class TouchPlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 30;
    [SerializeField] float rotationSpeed = 5;

    [SerializeField] Transform transformToControl;

    void Update()
    {
        //Move Front/Back
        if (MobileJoystick.instance.moveDirection.y != 0)
        {
            transformToControl.Translate((transformToControl.forward * Time.deltaTime * movementSpeed * MobileJoystick.instance.moveDirection.y) + (transformToControl.right * Time.deltaTime * movementSpeed * MobileJoystick.instance.moveDirection.x), Space.World);
        }

        transformToControl.Rotate(new Vector3(rotationSpeed * Time.deltaTime * -MobileSwipe.instance.moveDirection.y, 0, 0) , Space.Self);
        transformToControl.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime * MobileSwipe.instance.moveDirection.x, 0) , Space.World);
    }
}