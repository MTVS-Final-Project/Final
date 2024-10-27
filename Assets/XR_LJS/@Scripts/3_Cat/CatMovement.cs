using UnityEngine;
using TMPro; // TextMeshPro ����� ���� �߰�

public class CatMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isTouched = false; // ����̰� Ŭ���ǰų� ��ġ�Ǿ����� ����
    private SpriteRenderer[] spriteRenderers; // ������� ��� SpriteRenderer��
    
    void Start()
    {
        // ������� ��� SpriteRenderer�� ã�� (���� ������Ʈ ����)
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {

        // �Է� ó�� (���콺 Ŭ�� �Ǵ� ��ġ)
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            Vector3 inputPosition;
            if (Input.GetMouseButtonDown(0)) // ���콺 Ŭ�� �Է�
            {
                inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else // ��ġ �Է�
            {
                inputPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }

            inputPosition.z = 0f; // z���� 0���� ����

            RaycastHit2D hit = Physics2D.Raycast(inputPosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == this.gameObject) // ����̸� Ŭ�� �Ǵ� ��ġ�ߴ��� Ȯ��
            {
                isTouched = false;
                isMoving = false; // ����̸� Ŭ���ϰų� ��ġ�ϸ� �̵� ����
            }
            else if (!isTouched) // ����̰� Ŭ�� �Ǵ� ��ġ���� ���� ��쿡�� �̵�
            {
                // targetPosition�� ����̿� ������ z�� ������ �����Ͽ� �������� ��Ȯ�ϰ� �߻��ϵ��� ����
                targetPosition = new Vector3(inputPosition.x, inputPosition.y, transform.position.z);
                isMoving = true;
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
        if (Input.GetMouseButtonUp(0) || Input.touchCount == 1)
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
}
