using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CatInteraction : MonoBehaviour
{
    public Transform player; // 캐릭터의 Transform
    public Transform cat; // 고양이의 Transform
    public GameObject catHead; // 고양이 머리 부분
    public GameObject catButt; // 고양이 엉덩이 부분

    public TMP_Text interactionText; // TextMeshProUGUI 컴포넌트 연결 (UI 텍스트)

    private float originalCatScale = 1f; // 원래 고양이 크기
    private float enlargedCatScale = 1.5f; // 확대된 고양이 크기
    private float originalCameraSize = 5f; // 원래 카메라 사이즈
    private float enlargedCameraSize = 3f; // 확대된 카메라 사이즈

    private float interactionDistance = 1f; // 캐릭터와 고양이 간의 상호작용 허용 거리
    private bool isDragging = false; // 드래그 중인지 여부 확인
    private GameObject draggedObject; // 드래그 중인 오브젝트 참조

    private float doubleTapTime = 0.3f; // 더블 탭 간격 제한 시간
    private float lastTapTime = 0f; // 마지막 탭/클릭 시간
    private Vector3 targetPosition; // 고양이가 이동할 목표 위치
    private bool isMoving = false; // 고양이가 이동 중인지 여부

    private void Awake()
    {
        player = Resources.Load("Avatar1").GetComponent<Transform>();
        cat = GetComponent<Transform>();
    }

    void Update()
    {
        // 캐릭터와 고양이 간 거리 계산
        float distance = Vector2.Distance(cat.position, player.position);

        // 상호작용 거리 내에 있을 때 고양이 확대 및 카메라 축소
        if (distance <= interactionDistance)
        {
            cat.localScale = Vector3.Lerp(cat.localScale, new Vector3(enlargedCatScale, enlargedCatScale, 1f), Time.deltaTime * 2f);
            

            HandleTouchInput(); // 입력 처리
        }
        else
        {
            cat.localScale = Vector3.Lerp(cat.localScale, new Vector3(originalCatScale, originalCatScale, 1f), Time.deltaTime * 2f);
            

            ShowTextBox("");  // 텍스트 초기화
        }

        // 고양이가 이동 중일 때, 목표 위치로 이동
        if (isMoving)
        {
            cat.position = Vector3.MoveTowards(cat.position, targetPosition, Time.deltaTime * 2f);

            // 목표 위치에 도착하면 이동 중지
            if (Vector3.Distance(cat.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

    private void HandleTouchInput()
    {
        // 터치 입력 처리
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == catHead)
                {
                    isDragging = true; // 드래그 시작
                    draggedObject = hit.collider.gameObject;
                    ShowTextBox("고양이: 머리를 만졌어!");
                }

                // 더블 터치 감지 및 이동 처리
                if (hit.collider != null && hit.collider.gameObject == catButt)
                {
                    if (Time.time - lastTapTime < doubleTapTime)
                    {
                        // 엉덩이를 두 번 터치하면 해당 위치로 이동
                        targetPosition = hit.collider.transform.position;
                        isMoving = true;
                        ShowTextBox("고양이: 엉덩이를 만졌어! 이동 중...");
                    }
                    lastTapTime = Time.time;
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging && draggedObject == catHead)
            {
                // 드래그 중일 때 텍스트 업데이트
                ShowTextBox("고양이: 머리를 드래그 중이야!");
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false; // 드래그 종료
                draggedObject = null;
            }
        }

        // 마우스 입력 처리
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == catHead)
            {
                isDragging = true; // 마우스 드래그 시작
                draggedObject = hit.collider.gameObject;
                ShowTextBox("고양이: 머리를 만졌어!");
            }

            // 더블 클릭 감지 및 이동 처리
            if (hit.collider != null && hit.collider.gameObject == catButt)
            {
                if (Time.time - lastTapTime < doubleTapTime)
                {
                    // 엉덩이를 두 번 클릭하면 해당 위치로 이동
                    targetPosition = hit.collider.transform.position;
                    isMoving = true;
                    ShowTextBox("고양이: 엉덩이를 만졌어! 이동 중...");
                }
                lastTapTime = Time.time;
            }
        }
        else if (Input.GetMouseButton(0) && isDragging && draggedObject == catHead)
        {
            // 마우스 드래그 중일 때 텍스트 업데이트
            ShowTextBox("고양이: 머리를 만졌어!");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false; // 마우스 드래그 종료
            draggedObject = null;
        }
    }

    private void ShowTextBox(string message)
    {
        if (interactionText != null)
        {
            interactionText.text = message;
        }
        else
        {
            Debug.LogWarning("interactionText가 설정되지 않았습니다!");
        }
    }
}
