using UnityEngine;

public class LookPlayerDir : MonoBehaviour
{
    void Update()
    {
        // 메인 카메라의 위치 가져오기
        Vector3 cameraPosition = Camera.main.transform.position;

        // 물체에서 카메라로 향하는 방향 벡터 계산
        Vector3 direction = transform.position - cameraPosition;
        direction.y = 0; // Y축 고정을 위해 Y값을 0으로 설정

        // 방향이 유효할 때만 회전
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}
