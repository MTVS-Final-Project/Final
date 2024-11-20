using UnityEngine;
using Photon.Pun;

public class bl_ControllerExample : MonoBehaviourPunCallbacks
{
    [SerializeField] private bl_Joystick Joystick;
    [SerializeField] private float Speed = 5f;

    private Rigidbody2D rb;
    private PhotonView photonView;
    private PolygonCollider2D boundaryCollider;
    private Transform playerTransform;

    void Awake()
    {
        if (Joystick == null)
        {
            Joystick = FindObjectOfType<bl_Joystick>();
        }

        // polCollider 오브젝트의 PolygonCollider2D 찾기
        boundaryCollider = GameObject.Find("polCollider").GetComponent<PolygonCollider2D>();
        playerTransform = transform;
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

        if (Joystick == null)
        {
            Joystick = FindObjectOfType<bl_Joystick>();
            if (Joystick == null) return;
        }

        float horizontalInput = Joystick.Horizontal;
        float verticalInput = Joystick.Vertical;

        Vector2 movement = new Vector2(horizontalInput, verticalInput);

        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        // 다음 위치 계산
        Vector2 nextPosition = (Vector2)playerTransform.position + (movement * Speed * Time.deltaTime);

        // 다음 위치가 폴리곤 콜라이더 안에 있는지 확인
        if (IsPointInPolygon(nextPosition))
        {
            // 영역 안에 있으면 이동 허용
            playerTransform.position = nextPosition;
        }
        else
        {
            // 영역 밖이면 가장 가까운 유효한 위치로 이동
            Vector2 clampedPosition = ClampToPolygon(nextPosition);
            playerTransform.position = clampedPosition;
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

        // 현재 위치와 목표 위치 사이를 이진 탐색하여 경계에 가장 가까운 유효한 위치 찾기
        Vector2 currentPos = playerTransform.position;
        Vector2 direction = (position - currentPos).normalized;
        float distance = Vector2.Distance(currentPos, position);
        float minDistance = 0f;
        float maxDistance = distance;
        float currentDistance = distance;
        Vector2 validPosition = currentPos;

        for (int i = 0; i < 10; i++) // 10회 반복으로 충분한 정확도 확보
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