using UnityEngine;

public class CatSpawnOSM : MonoBehaviour
{
    public float radius = 80f;
    public float yOffset = 0f; // Y축 높이 오프셋

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //포켓몬고 처럼 그냥 스폰되는 고양이들. 평상시에도 상호작용하면 보상을 줌.
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 GetRandomPositionInCircleRelativeToSelf(float radius, float yOffset)
    {
        // 원 범위 내에서 랜덤한 각도와 거리 생성
        float angle = Random.Range(0f, Mathf.PI * 2);
        float distance = Random.Range(5f, radius);

        // XZ 평면에 좌표 계산
        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;

        // 자신의 위치 기준으로 좌표 반환
        return transform.position + new Vector3(x, yOffset, z);
    }
}
