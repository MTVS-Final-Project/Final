using UnityEngine;
using TMPro; // TextMeshPro 사용을 위해 추가

public class CatMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isTouched = false; // 고양이가 클릭되거나 터치되었는지 여부
    private SpriteRenderer[] spriteRenderers; // 고양이의 모든 SpriteRenderer들
    
    void Start()
    {
        // 고양이의 모든 SpriteRenderer를 찾음 (하위 오브젝트 포함)
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {

        // 입력 처리 (마우스 클릭 또는 터치)
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            Vector3 inputPosition;
            if (Input.GetMouseButtonDown(0)) // 마우스 클릭 입력
            {
                inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else // 터치 입력
            {
                inputPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }

            inputPosition.z = 0f; // z축을 0으로 설정

            RaycastHit2D hit = Physics2D.Raycast(inputPosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == this.gameObject) // 고양이를 클릭 또는 터치했는지 확인
            {
                isTouched = false;
                isMoving = false; // 고양이를 클릭하거나 터치하면 이동 멈춤
            }
            else if (!isTouched) // 고양이가 클릭 또는 터치되지 않은 경우에만 이동
            {
                // targetPosition을 고양이와 동일한 z축 값으로 설정하여 움직임이 정확하게 발생하도록 수정
                targetPosition = new Vector3(inputPosition.x, inputPosition.y, transform.position.z);
                isMoving = true;
            }
        }

        // 고양이가 클릭 또는 터치되지 않았을 때만 이동
        if (isMoving && !isTouched)
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

        // 고양이가 클릭되거나 터치되었다가 더 이상 클릭되지 않은 상태로 변경되면 다시 이동 가능
        if (Input.GetMouseButtonUp(0) || Input.touchCount == 1)
        {
            isTouched = false; // 클릭이나 터치가 끝났을 때 이동 재개 가능
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
