using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

public class bl_ControllerExample : MonoBehaviourPunCallbacks
{
    [SerializeField] private bl_Joystick Joystick;
    [SerializeField] private float Speed = 5f;
    private Rigidbody2D rb;
    private PhotonView photonView;
    private PolygonCollider2D boundaryCollider;
    private Transform playerTransform;
    private bool isDragging = false;
    private int touchId = -1;
    private Canvas canvas;
    private PointerEventData pointerEventData;

    void Awake()
    {
        if (Joystick == null)
        {
            Joystick = FindObjectOfType<bl_Joystick>();
        }
        boundaryCollider = GameObject.Find("polCollider").GetComponent<PolygonCollider2D>();
        playerTransform = transform;
        canvas = GameObject.FindObjectOfType<Canvas>();
        pointerEventData = new PointerEventData(EventSystem.current);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        if (rb != null)
        {
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        //HandleInput();
        ProcessMovement();
    }

//    private void HandleInput()
//    {
//#if UNITY_ANDROID || UNITY_IOS
//        // 모바일 터치 입력 처리
//        if (Input.touchCount > 0)
//        {
//            Touch myTouch = Input.GetTouch(0);

//            switch (myTouch.phase)
//            {
//                case TouchPhase.Began:
//                    if (!isDragging)
//                    {
//                        touchId = touch.fingerId;
//                        isDragging = true;
//                        OnTouchBegan(touch.position);
//                    }
//                    break;

//                case TouchPhase.Moved:
//                    if (isDragging && touch.fingerId == touchId)
//                    {
//                        OnTouchMoved(touch.position);
//                    }
//                    break;

//                case TouchPhase.Ended:
//                case TouchPhase.Canceled:
//                    if (touch.fingerId == touchId)
//                    {
//                        isDragging = false;
//                        touchId = -1;
//                        OnTouchEnded();
//                    }
//                    break;
//            }
//        }
//    }
//#else
//       // PC 마우스 입력 처리
//       if (Input.GetMouseButtonDown(0))
//       {
//           isDragging = true;
//           OnTouchBegan(Input.mousePosition);
//       }
//       else if (Input.GetMouseButton(0) && isDragging)
//       {
//           OnTouchMoved(Input.mousePosition);
//       }
//       else if (Input.GetMouseButtonUp(0))
//       {
//           isDragging = false;
//           OnTouchEnded();
//       }
//#endif
//}

private void ProcessMovement()
{
    if (Joystick == null) return;

    float horizontalInput = Joystick.Horizontal;
    float verticalInput = Joystick.Vertical;
    Vector2 movement = new Vector2(horizontalInput, verticalInput);

    if (movement.magnitude > 1f)
    {
        movement.Normalize();
    }

    // 다음 위치 계산
    Vector2 nextPosition = (Vector2)playerTransform.position + (movement * Speed * Time.deltaTime);

    // 경계 확인 및 이동
    if (IsPointInPolygon(nextPosition))
    {
        if (rb != null)
        {
            rb.MovePosition(nextPosition);
        }
        else
        {
            playerTransform.position = nextPosition;
        }
    }
    else
    {
        Vector2 clampedPosition = ClampToPolygon(nextPosition);
        if (rb != null)
        {
            rb.MovePosition(clampedPosition);
        }
        else
        {
            playerTransform.position = clampedPosition;
        }
    }
}

private void OnTouchBegan(Vector2 position)
{
    if (canvas != null && Joystick != null)
    {
        pointerEventData.position = position;
        Joystick.OnPointerDown(pointerEventData);
    }
}

private void OnTouchMoved(Vector2 position)
{
    if (canvas != null && Joystick != null)
    {
        pointerEventData.position = position;
        Joystick.OnDrag(pointerEventData);
    }
}

private void OnTouchEnded()
{
    if (Joystick != null)
    {
        Joystick.OnPointerUp(pointerEventData);
    }
}

private Vector3 GetWorldPosition(Vector2 screenPosition)
{
    if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
    {
        return screenPosition;
    }
    else
    {
        Vector2 tempVector = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.worldCamera,
            out tempVector);
        return canvas.transform.TransformPoint(tempVector);
    }
}

private bool IsPointInPolygon(Vector2 point)
{
    return boundaryCollider.OverlapPoint(point);
}

private Vector2 ClampToPolygon(Vector2 position)
{
    if (IsPointInPolygon(position))
        return position;

    Vector2 currentPos = playerTransform.position;
    Vector2 direction = (position - currentPos).normalized;
    float distance = Vector2.Distance(currentPos, position);
    float minDistance = 0f;
    float maxDistance = distance;
    float currentDistance = distance;
    Vector2 validPosition = currentPos;

    for (int i = 0; i < 10; i++)
    {
        currentDistance = (minDistance + maxDistance) * 0.5f;
        Vector2 testPosition = currentPos + direction * currentDistance;
        if (IsPointInPolygon(testPosition))
        {
            validPosition = testPosition;
            minDistance = currentDistance;
        }
        else
        {
            maxDistance = currentDistance;
        }
    }
    return validPosition;
}
}