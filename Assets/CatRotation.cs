using UnityEngine;

public class CatRotation : MonoBehaviour
{
    private Vector3 lastpos; // ���� ��ġ�� �����ϴ� Vector3

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �ʱ�ȭ: ���� ��ġ�� ���� ��ġ�� ����
        lastpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // �̵� ���� ���
        Vector3 nowpos = transform.position; // ���� ��ġ
        Vector3 moveVector = nowpos - lastpos;

        // X���� ���� ������� �������� Ȯ��
        if (moveVector.x > 0)
        {
            //Debug.Log("X�� �̵�: ��� (���������� �̵�)");
            transform.rotation = Quaternion.Euler(0, 180, 0); // �������� ���� ����
        }
        else if (moveVector.x < 0)
        {
           // Debug.Log("X�� �̵�: ���� (�������� �̵�)");
            transform.rotation = Quaternion.Euler(0, 0, 0); // ������ ���� ����
        }
        else
        {
           // Debug.Log("X�� �̵� ����");
        }

        // ���� ��ġ�� ���� ��ġ�� ����
        lastpos = nowpos;
    }
}
