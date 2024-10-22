using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가

public class CatInteraction : MonoBehaviour
{
    public Transform player; // 캐릭터의 Transform
    public Transform cat; // 고양이의 Transform
    public GameObject catHead; // 고양이 머리 부분
    public GameObject catButt; // 고양이 엉덩이 부분

    // TextMeshProUGUI 컴포넌트 연결 (UI 텍스트로 사용할 TMP 텍스트 변수)
    public TMP_Text interactionText;

    private float originalCatScale = 1f; // 원래 고양이 크기
    private float enlargedCatScale = 1.5f; // 확대된 고양이 크기
    private float originalCameraSize = 5f; // 원래 카메라 사이즈
    private float enlargedCameraSize = 3f; // 확대된 카메라 사이즈

    // 캐릭터와 고양이 간의 상호작용 허용 거리
    private float interactionDistance = 1f;

    void Update()
    {
        // 캐릭터와 고양이 간 거리 계산
        float distance = Vector2.Distance(cat.position, player.position);

        // 고양이와 캐릭터가 가까울 경우
        if (distance <= interactionDistance)
        {
            // 고양이와 카메라 동시에 확대
            cat.localScale = Vector3.Lerp(cat.localScale, new Vector3(enlargedCatScale, enlargedCatScale, 1f), Time.deltaTime * 2f);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, enlargedCameraSize, Time.deltaTime * 2f);

            // 고양이와 가까울 때만 터치 상호작용 허용
            HandleTouchInput();
        }
        else
        {
            // 고양이와 카메라 원래 크기로 복구
            cat.localScale = Vector3.Lerp(cat.localScale, new Vector3(originalCatScale, originalCatScale, 1f), Time.deltaTime * 2f);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, originalCameraSize, Time.deltaTime * 2f);

            // 고양이와 가까울 때만 터치 상호작용 허용
            ShowTextBox("");  // 터치 전에 텍스트 초기화
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0) // 하나 이상의 터치가 있을 때
        {
            Touch touch = Input.GetTouch(0); // 첫 번째 터치 입력 가져오기

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position); // 터치 위치를 월드 좌표로 변환
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject == catHead) // 고양이 머리와 충돌 감지
                    {
                        ShowTextBox("고양이: 머리를 만졌어!");
                    }
                    else if (hit.collider.gameObject == catButt) // 고양이 엉덩이와 충돌 감지
                    {
                        ShowTextBox("고양이: 엉덩이를 만졌어!");
                        ChangeCatPose(); // 고양이 자세 변경
                    }
                }
            }
        }
    }

    private void ShowTextBox(string message)
    {
        // TextMeshPro 텍스트 업데이트
        if (interactionText != null)
        {
            interactionText.text = message; // TMP 텍스트 필드를 업데이트
        }
        else
        {
            Debug.LogWarning("interactionText가 설정되지 않았습니다!"); // TMP 텍스트가 null인 경우 경고 출력
        }
    }

    private void ChangeCatPose()
    {
        // 고양이 자세 변경 로직
        Debug.Log("고양이 자세가 바뀌었어!"); // 자세 변경을 구현 (현재는 디버그 메시지)
    }
}
