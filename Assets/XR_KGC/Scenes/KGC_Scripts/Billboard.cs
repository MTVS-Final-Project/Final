using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        // 카메라의 방향 벡터를 계산합니다.
        Vector3 cameraDirection = Camera.main.transform.position - transform.position;

        // 반대로 바라보도록 방향을 반전시킵니다.
        cameraDirection = -cameraDirection;

        // 객체의 회전을 카메라를 바라보는 방향으로 설정합니다.
        Quaternion targetRotation = Quaternion.LookRotation(cameraDirection);

        // 쿼터니언을 사용하여 객체의 회전을 모든 축을 고려하여 적용합니다.
        transform.rotation = targetRotation;
    }
}
