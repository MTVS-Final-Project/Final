using UnityEngine;
using System.Collections;
using Spine.Unity;

public class CatController : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2.0f;
    public float doubleClickTimeLimit = 1f;

    private float lastClickTime = 0f;
    private Collider2D headCollider;
    private Collider2D bodyCollider;
    private Vector3 headOriginalOffset;
    private Vector3 bodyOriginalOffset;
    private SkeletonAnimation skeletonAnimation; // SkeletonAnimation 컴포넌트 참조
    [SerializeField] private Camera cam; // 카메라 참조
    public float zoomMultiplier = 2.0f;
    public float minZoom = 2f;
    public float maxZoom = 5f;
    public float smoothTime = 0.3f;
    public float clickRadius = 1.0f; // 클릭 인식 범위 반경
    public float moveSmoothTime = 0.3f; // 카메라 이동 부드럽기

    private Transform catTransform; // Cat 오브젝트의 Transform
    private float targetZoom;
    private float zoomVelocity = 0f;
    private Vector3 cameraVelocity = Vector3.zero;

    private void Awake()
    {
        // SkeletonAnimation 컴포넌트 가져오기
        skeletonAnimation = GetComponent<SkeletonAnimation>();

        // 초기 위치 설정
        if (skeletonAnimation != null)
        {
            skeletonAnimation.skeleton.ScaleX = -1f; // Initial Flip X 활성화 (오른쪽을 바라보게 함)
        }

        headCollider = transform.Find("Head").GetComponent<Collider2D>();
        bodyCollider = transform.Find("Body").GetComponent<Collider2D>();

        // 초기 위치 오프셋 저장
        headOriginalOffset = headCollider.offset;
        bodyOriginalOffset = bodyCollider.offset;

        player = GameObject.Find("Avatar1").GetComponent<Transform>();
    }

    private void Start()
    {
        GameObject catObject = GameObject.FindGameObjectWithTag("Cat");
        if (catObject != null)
        {
            catTransform = catObject.transform;
        }

        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        HandleClick();

        // 카메라의 줌 부드럽게 전환
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, smoothTime);

        // 카메라 확대 상태라면 고양이를 따라 이동
        if (cam.orthographicSize <= minZoom + 0.1f) // 확대 완료 근처에서 고양이 위치로 이동
        {
            Vector3 targetPosition = new Vector3(catTransform.position.x, catTransform.position.y, cam.transform.position.z);
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPosition, ref cameraVelocity, moveSmoothTime);
        }
    }

    private void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float timeSinceLastClick = Time.time - lastClickTime;

            if (timeSinceLastClick <= doubleClickTimeLimit)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider != null && hit.collider.CompareTag("Ground"))
                {
                    StartCoroutine(MoveTowards(hit.point));
                }
            }
            lastClickTime = Time.time;
        }

        if (Input.GetMouseButtonDown(0) && catTransform != null) // 마우스 왼쪽 버튼 클릭 감지
        {
            Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0f;

            float distanceToCat = Vector3.Distance(mouseWorldPosition, catTransform.position);

            if (distanceToCat <= clickRadius) // 클릭이 고양이 주변에 있는지 확인
            {
                // Cat 오브젝트 근처 클릭 시 확대 설정
                targetZoom = minZoom;
            }
        }
    }

    private IEnumerator MoveTowards(Vector3 targetPosition)
    {
        float duration = 1.0f;
        float elapsed = 0f;
        Vector3 startingPosition = transform.position;
        Vector3 direction = (targetPosition - startingPosition).normalized;

        // 이동 방향에 따라 SkeletonAnimation의 ScaleX 설정
        FlipSkeletonAnimation(direction.x > 0); // 오른쪽이면 flipRight=true, 왼쪽이면 false

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    private void FlipSkeletonAnimation(bool flipRight)
    {
        if (skeletonAnimation != null)
        {
            skeletonAnimation.skeleton.ScaleX = flipRight ? 1f : -1f; // ScaleX 속성으로 방향 반전
        }

        // Collider의 offset 위치를 flip 방향에 따라 반전
        headCollider.offset = flipRight ? new Vector2(-headOriginalOffset.x, headOriginalOffset.y) : headOriginalOffset;
        bodyCollider.offset = flipRight ? new Vector2(-bodyOriginalOffset.x, bodyOriginalOffset.y) : bodyOriginalOffset;
    }
}
