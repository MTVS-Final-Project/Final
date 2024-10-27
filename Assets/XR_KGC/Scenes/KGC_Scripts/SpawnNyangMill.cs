using UnityEngine;

public class SpawnNyangMill : MonoBehaviour
{
    public float radius = 30f;
    public float yOffset = 0f; // Y축 높이 오프셋

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
        // 원 범위 내에서 랜덤한 각도와 거리 생성
        float angle = Random.Range(0f, Mathf.PI * 2);
        float distance = Random.Range(0f, radius);

        // XZ 평면에 좌표 계산
        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;

        // 자신의 위치 기준으로 좌표 반환
        return transform.position + new Vector3(x, yOffset, z);
    }
}
