using UnityEngine;

public class CatMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private SpriteRenderer[] spriteRenderers; // 고양이의 모든 SpriteRenderer들

    void Start()
    {
        // 고양이의 모든 SpriteRenderer를 찾음 (하위 오브젝트 포함)
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                // 터치 위치를 월드 좌표로 변환
                targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));
                targetPosition.z = transform.position.z;
                isMoving = true;
            }
        }

        if (isMoving)
        {
            // 이동 방향에 따라 고양이의 모든 SpriteRenderer에 flipX를 적용
            if (targetPosition.x > transform.position.x)
            {
                SetCatFlip(true); // 오른쪽으로 이동할 때 flipX를 true로 설정
            }
            else if (targetPosition.x < transform.position.x)
            {
                SetCatFlip(false); // 왼쪽으로 이동할 때 flipX를 false로 설정
            }

            // 고양이를 목표 위치로 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 목표 위치에 도착하면 이동 멈춤
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

    // 고양이의 모든 SpriteRenderer에 flipX 적용하는 함수
    void SetCatFlip(bool flip)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.flipX = flip;
        }
    }
}
