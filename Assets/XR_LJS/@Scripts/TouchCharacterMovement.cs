using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TouchCharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private SpriteRenderer spriteRenderer;

    // UI 관련 변수
    private EventSystem eventSystem;
    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastResults;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // EventSystem 초기화
        eventSystem = EventSystem.current;

        if (eventSystem == null)
        {
            Debug.LogError("No EventSystem found in the scene. Please add one.");
        }

        pointerEventData = new PointerEventData(eventSystem);
        raycastResults = new List<RaycastResult>();
    }

    void Update()
    {
        // 버튼 개수 확인
        int buttonCount = CountVisibleButtons();

        // 버튼이 3개 미만일 때만 터치 이동 허용
        if (buttonCount < 3)
        {
            HandleTouchMovement();
        }
        else
        {
            // 버튼이 3개 이상이면 이동 중지
            isMoving = false;
        }

        // 캐릭터 이동
        if (isMoving)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 이동 중 방향이 바뀌었는지 확인하고 뒤집기
            if (newPosition.x != transform.position.x)
            {
                FlipCharacter(newPosition.x > transform.position.x);
            }

            transform.position = newPosition;

            // 목표 지점에 도달했는지 확인
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

    void HandleTouchMovement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                // UI 요소를 터치했는지 확인
                if (!IsPointerOverUIElement(touch.position))
                {
                    // 터치한 위치를 월드 좌표로 변환
                    targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));
                    targetPosition.z = transform.position.z; // z 위치 유지
                    isMoving = true;

                    // 이동 방향에 따라 캐릭터 뒤집기
                    FlipCharacter(targetPosition.x > transform.position.x);
                }
            }
        }
    }

    void FlipCharacter(bool facingRight)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = facingRight; // Adjusted to set flipX directly
        }

        // Flip all child SpriteRenderers recursively
        FlipAllChildSpriteRenderers(transform, facingRight);
    }

    void FlipAllChildSpriteRenderers(Transform parent, bool facingRight)
    {
        foreach (Transform child in parent)
        {
            SpriteRenderer childSpriteRenderer = child.GetComponent<SpriteRenderer>();
            if (childSpriteRenderer != null)
            {
                childSpriteRenderer.flipX = facingRight; // Adjusted to set flipX directly
            }

            // Recurse into the child to find any nested SpriteRenderers
            FlipAllChildSpriteRenderers(child, facingRight);
        }
    }

    int CountVisibleButtons()
    {
        int count = 0;
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            UnityEngine.UI.Button[] buttons = canvas.GetComponentsInChildren<UnityEngine.UI.Button>(false);
            count += buttons.Length;
        }
        return count;
    }

    bool IsPointerOverUIElement(Vector2 touchPosition)
    {
        pointerEventData.position = touchPosition;
        raycastResults.Clear();
        eventSystem.RaycastAll(pointerEventData, raycastResults);
        return raycastResults.Count > 0;
    }
}
