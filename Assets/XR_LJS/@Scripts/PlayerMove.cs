using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f; // �ƹ�Ÿ �̵� �ӵ�

    private void Update()
    {
        // ���콺 Ŭ�� �Է��� ó���մϴ�.
        if (Mouse.current.leftButton.wasPressedThisFrame) // ���콺 ���� ��ư Ŭ��
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue(); // Ŭ���� ��ġ�� �н��ϴ�.
            mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane)); // ���� ��ǥ�� ��ȯ
            mousePosition.z = 0; // 2D�̹Ƿ� Z���� 0���� ����

            // Ŭ���� ��ġ�� �ƹ�Ÿ �̵�
            StartCoroutine(MoveToPosition(mousePosition)); // �ڷ�ƾ ȣ��
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f) // ��ǥ ��ġ���� ������� ������
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime); // �ƹ�Ÿ ��ġ �̵�
            yield return null; // ���� �����ӱ��� ���
        }

        // ��ǥ ��ġ�� �����ϸ� ��ġ�� ��Ȯ�� ���� (�Ҽ��� ���� ����)
        transform.position = targetPosition;
    }
}
