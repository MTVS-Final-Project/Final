using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TouchCharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private SpriteRenderer spriteRenderer;

    // UI ���� ����
    private EventSystem eventSystem;
    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastResults;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // EventSystem �ʱ�ȭ
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
        // ��ư ���� Ȯ��
        int buttonCount = CountVisibleButtons();

        // ��ư�� 3�� �̸��� ���� ��ġ �̵� ���
        if (buttonCount < 3)
        {
            HandleTouchMovement();
        }
        else
        {
            // ��ư�� 3�� �̻��̸� �̵� ����
            isMoving = false;
        }

        // ĳ���� �̵�
        if (isMoving)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // �̵� �� ������ �ٲ������ Ȯ���ϰ� ������
            if (newPosition.x != transform.position.x)
            {
                FlipCharacter(newPosition.x > transform.position.x);
            }

            transform.position = newPosition;

            // ��ǥ ������ �����ߴ��� Ȯ��
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
                // UI ��Ҹ� ��ġ�ߴ��� Ȯ��
                if (!IsPointerOverUIElement(touch.position))
                {
                    // ��ġ�� ��ġ�� ���� ��ǥ�� ��ȯ
                    targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));
                    targetPosition.z = transform.position.z; // z ��ġ ����
                    isMoving = true;

                    // �̵� ���⿡ ���� ĳ���� ������
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
