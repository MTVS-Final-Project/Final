using UnityEngine;
using TMPro; // TextMeshPro 사용을 위해 추가

public class CatMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isTouched = false; // 고양이가 클릭되거나 터치되었는지 여부
    private SpriteRenderer[] spriteRenderers; // 고양이의 모든 SpriteRenderer들
    public TMP_Text displayText; // 텍스트 메시지를 표시할 TMP_Text

    void Start()
    {
        // 고양이의 모든 SpriteRenderer를 찾음 (하위 오브젝트 포함)
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        // TMP_Text를 비활성화 (초기에는 텍스트가 보이지 않도록 설정)
        if (displayText != null)
        {
            displayText.text = "";
            displayText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // 텍스트가 활성화되어 있다면 고양이의 움직임을 멈춤
        if (displayText != null && displayText.gameObject.activeInHierarchy)
        {
            isMoving = false;
            return; // 텍스트가 보이는 동안 이동하지 않음
        }

        // 마우스 클릭 입력 처리
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == this.gameObject) // 고양이를 클릭했는지 확인
            {
                isTouched = true;
                isMoving = false; // 고양이를 클릭하면 이동 멈춤
                ShowTextBox("고양이가 클릭되었습니다!"); // 텍스트 출력
            }
            else if (!isTouched) // 고양이가 클릭되지 않은 경우에만 이동
            {
                targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                targetPosition.z = transform.position.z; // 고양이 z축 위치 유지
                isMoving = true;
            }
        }

        // 터치 입력 처리
        if (Input.touchCount > 0) // 터치 입력이 있을 때
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            touchPos.z = 0f;

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == this.gameObject) // 고양이를 터치했는지 확인
                {
                    isTouched = true;
                    isMoving = false; // 고양이를 터치하면 이동 멈춤
                    ShowTextBox("고양이가 터치되었습니다!"); // 텍스트 출력
                }
                else if (!isTouched) // 고양이가 터치되지 않은 경우에만 이동
                {
                    targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));
                    targetPosition.z = transform.position.z; // 고양이 z축 위치 유지
                    isMoving = true;
                }
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
        if (Input.GetMouseButtonUp(0) || Input.touchCount == 0)
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

    // 텍스트 박스를 활성화하고 메시지를 출력하는 함수
    void ShowTextBox(string message)
    {
        if (displayText != null)
        {
            displayText.text = message;
            displayText.gameObject.SetActive(true);

            // 일정 시간 후 텍스트 박스 비활성화 (예: 2초 후)
            Invoke("HideTextBox", 2f);
        }
    }

    // 텍스트 박스를 비활성화하는 함수
    void HideTextBox()
    {
        if (displayText != null)
        {
            displayText.gameObject.SetActive(false);
        }
    }
}
