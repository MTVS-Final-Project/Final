using UnityEngine;

public class LookPlayerDir : MonoBehaviour
{
    void Update()
    {
        // ���� ī�޶��� ��ġ ��������
        Vector3 cameraPosition = Camera.main.transform.position;

        // ��ü���� ī�޶�� ���ϴ� ���� ���� ���
        Vector3 direction = transform.position - cameraPosition;
        direction.y = 0; // Y�� ������ ���� Y���� 0���� ����

        // ������ ��ȿ�� ���� ȸ��
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}
