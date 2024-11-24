using UnityEngine;
using Photon.Pun;

public class bl_ControllerExample : MonoBehaviourPunCallbacks
{
    [SerializeField] private bl_Joystick Joystick;
    [SerializeField] private float Speed = 5f;
    private PolygonCollider2D boundaryCollider;
    private Transform playerTransform;

    void Awake()
    {
        if (Joystick == null)
        {
            Joystick = FindObjectOfType<bl_Joystick>();
        }
        GameObject polCol = GameObject.Find("polCollider");
        if (polCol != null)
        {
            boundaryCollider = polCol.GetComponent<PolygonCollider2D>();
        }
        playerTransform = transform;
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        ProcessMovement();
    }

    private void ProcessMovement()
    {
        if (Joystick == null) return;

        float horizontalInput = Joystick.Horizontal;
        float verticalInput = Joystick.Vertical;
        Vector2 movement = new Vector2(horizontalInput, verticalInput);

        if (movement.magnitude > 1f)
        {
            movement.Normalize();  // 입력 벡터를 정규화하여 속도를 일정하게 유지
        }

        // 다음 위치 계산
        Vector2 nextPosition = (Vector2)playerTransform.position + (movement * Speed * Time.deltaTime);

        // 경계 확인 및 이동
        if (IsPointInPolygon(nextPosition))
        {
            playerTransform.position = nextPosition;  // Transform을 사용하여 위치 이동
        }
        else
        {
            Vector2 clampedPosition = ClampToPolygon(nextPosition);  // 경계를 벗어난 경우 폴리곤 안으로 클램핑
            playerTransform.position = clampedPosition;  // 위치 업데이트
        }
    }

    // 포인트가 폴리곤 내부에 있는지 확인
    private bool IsPointInPolygon(Vector2 point)
    {
        return boundaryCollider.OverlapPoint(point);
    }

    // 폴리곤 내에서 가장 가까운 유효한 위치를 찾기 위한 함수
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

        // 이진 탐색 방식으로 유효한 위치 찾기
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
