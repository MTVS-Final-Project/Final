using UnityEngine;
using TMPro; // TextMeshPro ����� ���� �߰�

public class CatMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isTouched = false; // ����̰� Ŭ���ǰų� ��ġ�Ǿ����� ����
    private SpriteRenderer[] spriteRenderers; // ������� ��� SpriteRenderer��
    public TMP_Text displayText; // �ؽ�Ʈ �޽����� ǥ���� TMP_Text

    void Start()
    {
        // ������� ��� SpriteRenderer�� ã�� (���� ������Ʈ ����)
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        // TMP_Text�� ��Ȱ��ȭ (�ʱ⿡�� �ؽ�Ʈ�� ������ �ʵ��� ����)
        if (displayText != null)
        {
            displayText.text = "";
            displayText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // �ؽ�Ʈ�� Ȱ��ȭ�Ǿ� �ִٸ� ������� �������� ����
        if (displayText != null && displayText.gameObject.activeInHierarchy)
        {
            isMoving = false;
            return; // �ؽ�Ʈ�� ���̴� ���� �̵����� ����
        }

        // ���콺 Ŭ�� �Է� ó��
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư Ŭ��
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == this.gameObject) // ����̸� Ŭ���ߴ��� Ȯ��
            {
                isTouched = true;
                isMoving = false; // ����̸� Ŭ���ϸ� �̵� ����
                ShowTextBox("����̰� Ŭ���Ǿ����ϴ�!"); // �ؽ�Ʈ ���
            }
            else if (!isTouched) // ����̰� Ŭ������ ���� ��쿡�� �̵�
            {
                targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                targetPosition.z = transform.position.z; // ����� z�� ��ġ ����
                isMoving = true;
            }
        }

        // ��ġ �Է� ó��
        if (Input.touchCount > 0) // ��ġ �Է��� ���� ��
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            touchPos.z = 0f;

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == this.gameObject) // ����̸� ��ġ�ߴ��� Ȯ��
                {
                    isTouched = true;
                    isMoving = false; // ����̸� ��ġ�ϸ� �̵� ����
                    ShowTextBox("����̰� ��ġ�Ǿ����ϴ�!"); // �ؽ�Ʈ ���
                }
                else if (!isTouched) // ����̰� ��ġ���� ���� ��쿡�� �̵�
                {
                    targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));
                    targetPosition.z = transform.position.z; // ����� z�� ��ġ ����
                    isMoving = true;
                }
            }
        }

        // ����̰� Ŭ�� �Ǵ� ��ġ���� �ʾ��� ���� �̵�
        if (isMoving && !isTouched)
        {
            // �̵� ���⿡ ���� ������� ��� SpriteRenderer�� flipX�� ����
            if (targetPosition.x > transform.position.x)
            {
                SetCatFlip(true); // ���������� �̵��� �� flipX�� true�� ����
            }
            else if (targetPosition.x < transform.position.x)
            {
                SetCatFlip(false); // �������� �̵��� �� flipX�� false�� ����
            }

            // ����̸� ��ǥ ��ġ�� �̵�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // ��ǥ ��ġ�� �����ϸ� �̵� ����
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
        }

        // ����̰� Ŭ���ǰų� ��ġ�Ǿ��ٰ� �� �̻� Ŭ������ ���� ���·� ����Ǹ� �ٽ� �̵� ����
        if (Input.GetMouseButtonUp(0) || Input.touchCount == 0)
        {
            isTouched = false; // Ŭ���̳� ��ġ�� ������ �� �̵� �簳 ����
        }
    }

    // ������� ��� SpriteRenderer�� flipX �����ϴ� �Լ�
    void SetCatFlip(bool flip)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.flipX = flip;
        }
    }

    // �ؽ�Ʈ �ڽ��� Ȱ��ȭ�ϰ� �޽����� ����ϴ� �Լ�
    void ShowTextBox(string message)
    {
        if (displayText != null)
        {
            displayText.text = message;
            displayText.gameObject.SetActive(true);

            // ���� �ð� �� �ؽ�Ʈ �ڽ� ��Ȱ��ȭ (��: 2�� ��)
            Invoke("HideTextBox", 2f);
        }
    }

    // �ؽ�Ʈ �ڽ��� ��Ȱ��ȭ�ϴ� �Լ�
    void HideTextBox()
    {
        if (displayText != null)
        {
            displayText.gameObject.SetActive(false);
        }
    }
}
