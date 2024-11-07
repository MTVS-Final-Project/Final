using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    void Update()
    {
        // ��ü�� ��ġ�� �������� Z�� -0.1 ��ġ�� ���
        Vector3 rayStartPosition = transform.position + transform.forward * -0.1f;

        // Z�� �������� ���� ���� ����
        Vector3 rayDirection = transform.forward;

        // ����ĳ��Ʈ ����
        RaycastHit hit;
        if (Physics.Raycast(rayStartPosition, rayDirection, out hit, 10f,1<<10))
        {
            Debug.Log("Hit Object: " + hit.collider.gameObject.name);
        }

        // ���� �ð�ȭ�� ���� ����� ���� �׸��� (�����Ϳ����� ����)
        Debug.DrawRay(rayStartPosition, rayDirection * 1000f, Color.red);
    }
}
