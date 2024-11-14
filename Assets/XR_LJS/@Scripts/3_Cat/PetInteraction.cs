using UnityEngine;
using System.Collections;
using UnityEngine.XR.ARSubsystems;
using Spine.Unity;

public class PetInteraction : MonoBehaviour
{
    private Vector3 initialMousePosition;
    public GameObject head;
    public GameObject body;
    public GameObject whiteImageFriendly;
    public GameObject whiteImagePicky;
    public GameObject backButton;
    [SerializeField] private Camera cam;
    public float minZoom = 2f;
    public float maxZoom = 5f;
    public float approachDistance = 3.0f;
    public float moveAwayDistance = 2f;
    public float moveAwayDuration = 0.5f;

    // Ground layer reference
    public LayerMask groundLayer;

    private bool isZoomedIn = false;
    private Vector3 originalCameraPosition;
    private float originalZoom;

    private int headClickCount = 0;      // 머리 클릭 횟수
    private int bodyClickCount = 0;      // 몸통 클릭 횟수

    public SkeletonAnimation sk;

    private void Awake()
    {
    }
    private void Start()
    {
        whiteImageFriendly.SetActive(false);
        whiteImagePicky.SetActive(false);
        backButton.SetActive(false);
        originalCameraPosition = cam.transform.position;
        originalZoom = cam.orthographicSize;
    }

    private void Update()
    {
        if (sk == null)
        {
            sk = GameObject.Find("Cat").GetComponent<SkeletonAnimation>();
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    

    private void HandleClick()
    {
        if (cam.orthographicSize <= 3)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                // 머리 클릭 감지
                if (hit.collider.gameObject == head)
                {
                    headClickCount++;
                    bodyClickCount = 0; // 다른 부분의 클릭 카운트 초기화

                    if (headClickCount == 2)  // 머리 2회 클릭
                    {
                        ShowPinkyReaction();
                        headClickCount = 0;
                    }
                    else
                    {
                        initialMousePosition = Input.mousePosition;  // 드래그 감지 초기 위치 설정
                    }
                }
                // 몸통 클릭 감지
                else if (hit.collider.gameObject == body)
                {
                    bodyClickCount++;
                    headClickCount = 0; // 다른 부분의 클릭 카운트 초기화

                    if (bodyClickCount == 2)  // 몸통 2회 클릭
                    {
                        ShowFriendlyReaction();
                        bodyClickCount = 0;
                    }
                    else
                    {
                        initialMousePosition = Input.mousePosition;  // 드래그 감지 초기 위치 설정
                    }
                }
            }
        }
    }

    private void OnMouseDrag()
    {
        if (cam.orthographicSize <= 3)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            float dragDistance = Vector3.Distance(initialMousePosition, currentMousePosition);

            if (dragDistance > 5.0f)  // 드래그 거리 기준
            {
                // 머리를 드래그한 경우
                if (headClickCount > 0)
                {

                    //sk.AnimationName = "Love";
                    ShowFriendlyReaction();
                    headClickCount = 0;
                }
                // 몸통을 드래그한 경우
                else if (bodyClickCount > 0)
                {
                    ShowPinkyReaction();
                    bodyClickCount = 0;
                }
            }
        }
    }

    private IEnumerator ApproachCatOnGround(Vector3 targetPosition)
    {
        // Ensure the target position is on the ground
        RaycastHit2D groundHit = Physics2D.Raycast(targetPosition, Vector2.zero, Mathf.Infinity, groundLayer);
        if (groundHit.collider != null)
        {
            Vector3 startPos = transform.position;
            targetPosition.z = startPos.z; // Maintain the same Z position
            float distance = Vector3.Distance(startPos, targetPosition);

            if (distance > approachDistance)
            {
                float elapsed = 0;
                float duration = distance / 2.0f;

                while (elapsed < duration)
                {
                    Vector3 newPos = Vector3.Lerp(startPos, targetPosition, elapsed / duration);
                    // Ensure movement stays on ground
                    RaycastHit2D moveCheck = Physics2D.Raycast(newPos, Vector2.zero, Mathf.Infinity, groundLayer);
                    if (moveCheck.collider != null)
                    {
                        transform.position = newPos;
                    }
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                transform.position = targetPosition;
            }
        }
    }


    private void ShowPinkyReaction()
    {
        whiteImagePicky.SetActive(true);
        whiteImageFriendly.SetActive(false);
        // 코루틴들을 변수에 저장하여 추적
        StartCoroutine(HideImageAndKeepButtonsHidden(whiteImagePicky));
        StartCoroutine(MoveCatAwayOnGround());
        CatController.instance.ZoomOut();
        DisableAllButtons();
        
    }

    private void ShowFriendlyReaction()
    {
        sk.AnimationName = "Love";

        whiteImageFriendly.SetActive(true);
        whiteImagePicky.SetActive(false);
        StartCoroutine(HideImageAndKeepButtonsShown(whiteImageFriendly));
    }

    private IEnumerator HideImageAndKeepButtonsShown(GameObject image)
    {
        // 이미지를 숨기는 로직
        yield return new WaitForSeconds(1f); // 예시 대기 시간
        image.SetActive(false);

        // Friendly 반응에서는 버튼들이 보이도록 활성화
        CatController.instance.backButton.SetActive(true);
        CatController.instance.toyButton.SetActive(true);

    }

    private void DisableAllButtons()
    {
        CatController.instance.backButton.SetActive(false);
        CatController.instance.toyButton.SetActive(false);
        CatController.instance.ToyExitButton.SetActive(false);
        CatController.instance.bar.SetActive(false);
        CatController.instance.feather.SetActive(false);
    }


    private IEnumerator HideImageAndKeepButtonsHidden(GameObject image)
    {
        // 기존 HideImage 로직
        yield return new WaitForSeconds(1f); // 예시 대기 시간
        image.SetActive(false);

        // 버튼들이 계속 비활성화 상태로 유지되도록 확인
        DisableAllButtons();
    }

    private IEnumerator MoveCatAwayOnGround()
    {
        Vector3 startPosition = transform.parent.position;

        // x축과 y축 방향으로 무작위 이동 값을 설정합니다.
        float randomX = Random.Range(-moveAwayDistance, moveAwayDistance);
        float randomY = Random.Range(-moveAwayDistance, moveAwayDistance);
        Vector3 awayDirection = new Vector3(randomX, randomY, 0); // 랜덤 방향으로 이동
        Vector3 targetPosition = startPosition + awayDirection;

        // Verify target position is on ground
        RaycastHit2D groundCheck = Physics2D.Raycast(targetPosition, Vector2.zero, Mathf.Infinity, groundLayer);
        if (groundCheck.collider != null)
        {
            float elapsed = 0f;

            while (elapsed < moveAwayDuration)
            {
                Vector3 newPos = Vector3.Lerp(startPosition, targetPosition, elapsed / moveAwayDuration);
                // Ensure movement stays on ground
                RaycastHit2D moveCheck = Physics2D.Raycast(newPos, Vector2.zero, Mathf.Infinity, groundLayer);
                if (moveCheck.collider != null)
                {
                    transform.parent.position = newPos;
                }
                elapsed += Time.deltaTime;
                DisableAllButtons();
                yield return null;
            }

            transform.parent.position = targetPosition;
            
        }
    }



}