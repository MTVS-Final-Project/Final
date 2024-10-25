using UnityEngine;

public class SpawnNyangMill : MonoBehaviour
{
    public float radius = 30f;
    public float yOffset = 0f; // Y�� ���� ������

    public GameObject nm;

    void Start()
    {
        Vector3 randomPosition = GetRandomPositionInCircleRelativeToSelf(radius, yOffset);
        Debug.Log("Random Position: " + randomPosition);

        GameObject go = Instantiate(nm);
        go.transform.position = randomPosition;
    }

    Vector3 GetRandomPositionInCircleRelativeToSelf(float radius, float yOffset)
    {
        // �� ���� ������ ������ ������ �Ÿ� ����
        float angle = Random.Range(0f, Mathf.PI * 2);
        float distance = Random.Range(0f, radius);

        // XZ ��鿡 ��ǥ ���
        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;

        // �ڽ��� ��ġ �������� ��ǥ ��ȯ
        return transform.position + new Vector3(x, yOffset, z);
    }
}
