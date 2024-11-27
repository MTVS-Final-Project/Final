using UnityEngine;

public class SpritePos : MonoBehaviour
{
    
    public enum PositionMode
    {
        Mode1, // ������ 0~10 y��ǥ�� ���� ����
        Mode2, // 0~10 y��ǥ�� ���� ������ ���� +0.001 �߰�
        Mode3  // �ٸ� ���� (�ʿ信 ���� �߰� ����)
    }
    public SpriteRenderer Renderer; //���̾���������

    public PositionMode currentMode = PositionMode.Mode1;
    Transform child;

    void Start()
    {
        // �ڽ� ��ü ���� (�ε��� 1)
        child = transform.GetChild(1);
        Renderer = child.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // ���� �θ� ��ü�� y ��ǥ ��������
        float parentY = transform.position.y;
       // Vector3 worldPosition = child.position;

        switch (currentMode)
        {
            case PositionMode.Mode1:
                // y ��ǥ�� 0���� 10 ������ �� z ��ǥ ����
                if (parentY >= 0 && parentY <= 10)
                {
                    int zOrder = (int)Mathf.Lerp(0, -100, parentY / 10);
                    Renderer.sortingOrder = zOrder;
                    //child.position = worldPosition;
                }
                break;

            case PositionMode.Mode2:
                // y ��ǥ�� 0���� 10 ������ �� ������ ���� +0.001 �߰�
                if (parentY >= 0 && parentY <= 10)
                {
                    int zOrder = (int)Mathf.Lerp(0, -100, parentY / 10)-1;
                    Renderer.sortingOrder = zOrder;
                    //child.position = worldPosition;
                }
                break;

            case PositionMode.Mode3:
                // �ʿ信 ���� �ٸ� ���� �߰� ����
                break;
        }
    }
}
