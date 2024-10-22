using UnityEngine;

public class CatMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private SpriteRenderer[] spriteRenderers; // ������� ��� SpriteRenderer��

    void Start()
    {
        // ������� ��� SpriteRenderer�� ã�� (���� ������Ʈ ����)
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                // ��ġ ��ġ�� ���� ��ǥ�� ��ȯ
                targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));
                targetPosition.z = transform.position.z;
                isMoving = true;
            }
        }

        if (isMoving)
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
    }

    // ������� ��� SpriteRenderer�� flipX �����ϴ� �Լ�
    void SetCatFlip(bool flip)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.flipX = flip;
        }
    }
}
