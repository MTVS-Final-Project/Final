using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CatInteraction : MonoBehaviour
{
    public Transform player; // ĳ������ Transform
    public Transform cat; // ������� Transform
    public GameObject catHead; // ����� �Ӹ� �κ�
    public GameObject catButt; // ����� ������ �κ�

    public TMP_Text interactionText; // TextMeshProUGUI ������Ʈ ���� (UI �ؽ�Ʈ)

    private float originalCatScale = 1f; // ���� ����� ũ��
    private float enlargedCatScale = 1.5f; // Ȯ��� ����� ũ��
    private float originalCameraSize = 5f; // ���� ī�޶� ������
    private float enlargedCameraSize = 3f; // Ȯ��� ī�޶� ������

    private float interactionDistance = 1f; // ĳ���Ϳ� ����� ���� ��ȣ�ۿ� ��� �Ÿ�
    private bool isDragging = false; // �巡�� ������ ���� Ȯ��
    private GameObject draggedObject; // �巡�� ���� ������Ʈ ����

    private float doubleTapTime = 0.3f; // ���� �� ���� ���� �ð�
    private float lastTapTime = 0f; // ������ ��/Ŭ�� �ð�
    private Vector3 targetPosition; // ����̰� �̵��� ��ǥ ��ġ
    private bool isMoving = false; // ����̰� �̵� ������ ����

    private void Awake()
    {
        player = Resources.Load("Avatar1").GetComponent<Transform>();
        cat = GetComponent<Transform>();
    }

    void Update()
    {
        // ĳ���Ϳ� ����� �� �Ÿ� ���
        float distance = Vector2.Distance(cat.position, player.position);

        // ��ȣ�ۿ� �Ÿ� ���� ���� �� ����� Ȯ�� �� ī�޶� ���
        if (distance <= interactionDistance)
        {
            cat.localScale = Vector3.Lerp(cat.localScale, new Vector3(enlargedCatScale, enlargedCatScale, 1f), Time.deltaTime * 2f);
            

            HandleTouchInput(); // �Է� ó��
        }
        else
        {
            cat.localScale = Vector3.Lerp(cat.localScale, new Vector3(originalCatScale, originalCatScale, 1f), Time.deltaTime * 2f);
            

            ShowTextBox("");  // �ؽ�Ʈ �ʱ�ȭ
        }

        // ����̰� �̵� ���� ��, ��ǥ ��ġ�� �̵�
        if (isMoving)
        {
            cat.position = Vector3.MoveTowards(cat.position, targetPosition, Time.deltaTime * 2f);

            // ��ǥ ��ġ�� �����ϸ� �̵� ����
            if (Vector3.Distance(cat.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

    private void HandleTouchInput()
    {
        // ��ġ �Է� ó��
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == catHead)
                {
                    isDragging = true; // �巡�� ����
                    draggedObject = hit.collider.gameObject;
                    ShowTextBox("�����: �Ӹ��� ������!");
                }

                // ���� ��ġ ���� �� �̵� ó��
                if (hit.collider != null && hit.collider.gameObject == catButt)
                {
                    if (Time.time - lastTapTime < doubleTapTime)
                    {
                        // �����̸� �� �� ��ġ�ϸ� �ش� ��ġ�� �̵�
                        targetPosition = hit.collider.transform.position;
                        isMoving = true;
                        ShowTextBox("�����: �����̸� ������! �̵� ��...");
                    }
                    lastTapTime = Time.time;
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging && draggedObject == catHead)
            {
                // �巡�� ���� �� �ؽ�Ʈ ������Ʈ
                ShowTextBox("�����: �Ӹ��� �巡�� ���̾�!");
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false; // �巡�� ����
                draggedObject = null;
            }
        }

        // ���콺 �Է� ó��
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == catHead)
            {
                isDragging = true; // ���콺 �巡�� ����
                draggedObject = hit.collider.gameObject;
                ShowTextBox("�����: �Ӹ��� ������!");
            }

            // ���� Ŭ�� ���� �� �̵� ó��
            if (hit.collider != null && hit.collider.gameObject == catButt)
            {
                if (Time.time - lastTapTime < doubleTapTime)
                {
                    // �����̸� �� �� Ŭ���ϸ� �ش� ��ġ�� �̵�
                    targetPosition = hit.collider.transform.position;
                    isMoving = true;
                    ShowTextBox("�����: �����̸� ������! �̵� ��...");
                }
                lastTapTime = Time.time;
            }
        }
        else if (Input.GetMouseButton(0) && isDragging && draggedObject == catHead)
        {
            // ���콺 �巡�� ���� �� �ؽ�Ʈ ������Ʈ
            ShowTextBox("�����: �Ӹ��� ������!");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false; // ���콺 �巡�� ����
            draggedObject = null;
        }
    }

    private void ShowTextBox(string message)
    {
        if (interactionText != null)
        {
            interactionText.text = message;
        }
        else
        {
            Debug.LogWarning("interactionText�� �������� �ʾҽ��ϴ�!");
        }
    }
}
