using UnityEngine;

public class MobileSwipe : MonoBehaviour
{

    public Vector2 moveDirection;

    Vector2 fingerUpPos = new Vector3(0, 0, 0);
    Vector2 fingerDownPos = new Vector3(0, 0, 0);

    int currentTouchID = -1;

    public static MobileSwipe instance;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (currentTouchID > Input.touchCount)
            currentTouchID = -1;

        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (currentTouchID >= 0 && currentTouchID != i)
            {
                continue;
            }

            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                currentTouchID = i;
                fingerDownPos = touch.position;
                fingerUpPos = fingerDownPos;
                CalculateDirection();
            }

            if (touch.position.x < Screen.width / 2)
            {
                touch.phase = TouchPhase.Canceled;
                StopSwipe();
            }

            if (touch.phase == TouchPhase.Moved)
            {
                fingerDownPos = touch.position;
                CalculateDirection();
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                fingerDownPos = touch.position;
                fingerUpPos = fingerDownPos;
                CalculateDirection();
                StopSwipe();
            }
        }
    }

    void CalculateDirection()
    {
        moveDirection = fingerDownPos - fingerUpPos;
        fingerUpPos = fingerDownPos;
    }

    void StopSwipe()
    {
        currentTouchID = -1;
    }
}
