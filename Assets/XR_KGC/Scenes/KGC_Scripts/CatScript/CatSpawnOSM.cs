using UnityEngine;

public class CatSpawnOSM : MonoBehaviour
{
    public float radius = 80f;
    public float yOffset = 0f; // Y�� ���� ������

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //���ϸ�� ó�� �׳� �����Ǵ� ����̵�. ���ÿ��� ��ȣ�ۿ��ϸ� ������ ��.
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 GetRandomPositionInCircleRelativeToSelf(float radius, float yOffset)
    {
        // �� ���� ������ ������ ������ �Ÿ� ����
        float angle = Random.Range(0f, Mathf.PI * 2);
        float distance = Random.Range(5f, radius);

        // XZ ��鿡 ��ǥ ���
        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;

        // �ڽ��� ��ġ �������� ��ǥ ��ȯ
        return transform.position + new Vector3(x, yOffset, z);
    }
}
