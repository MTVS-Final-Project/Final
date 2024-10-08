using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f; // 아바타 이동 속도

    private void Update()
    {
        // 마우스 클릭 입력을 처리합니다.
        if (Mouse.current.leftButton.wasPressedThisFrame) // 마우스 왼쪽 버튼 클릭
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue(); // 클릭한 위치를 읽습니다.
            mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane)); // 월드 좌표로 변환
            mousePosition.z = 0; // 2D이므로 Z축은 0으로 설정

            // 클릭한 위치로 아바타 이동
            StartCoroutine(MoveToPosition(mousePosition)); // 코루틴 호출
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f) // 목표 위치까지 가까워질 때까지
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime); // 아바타 위치 이동
            yield return null; // 다음 프레임까지 대기
        }

        // 목표 위치에 도달하면 위치를 정확히 설정 (소수점 오차 방지)
        transform.position = targetPosition;
    }
}
