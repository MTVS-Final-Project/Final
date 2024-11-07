using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    void Update()
    {
        // 물체의 위치를 기준으로 Z축 -0.1 위치를 계산
        Vector3 rayStartPosition = transform.position + transform.forward * -0.1f;

        // Z축 방향으로 레이 방향 설정
        Vector3 rayDirection = transform.forward;

        // 레이캐스트 실행
        RaycastHit hit;
        if (Physics.Raycast(rayStartPosition, rayDirection, out hit, 10f,1<<10))
        {
            Debug.Log("Hit Object: " + hit.collider.gameObject.name);
        }

        // 레이 시각화를 위해 디버그 라인 그리기 (에디터에서만 보임)
        Debug.DrawRay(rayStartPosition, rayDirection * 1000f, Color.red);
    }
}
