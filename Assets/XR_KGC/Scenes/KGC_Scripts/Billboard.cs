using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        // ī�޶��� ���� ���͸� ����մϴ�.
        Vector3 cameraDirection = Camera.main.transform.position - transform.position;

        // �ݴ�� �ٶ󺸵��� ������ ������ŵ�ϴ�.
        cameraDirection = -cameraDirection;

        // ��ü�� ȸ���� ī�޶� �ٶ󺸴� �������� �����մϴ�.
        Quaternion targetRotation = Quaternion.LookRotation(cameraDirection);

        // ���ʹϾ��� ����Ͽ� ��ü�� ȸ���� ��� ���� ����Ͽ� �����մϴ�.
        transform.rotation = targetRotation;
    }
}
