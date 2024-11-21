using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Spine.Unity;

public class CatController : MonoBehaviour
{

    public static CatController instance;
    //public Transform player;
    public float moveSpeed = 2.0f;
    public float doubleClickTimeLimit = 0.5f;

    private float lastClickTime = 0f;
    private CircleCollider2D headCollider;
    private BoxCollider2D bodyCollider;
    private Vector3 headOriginalOffset;
    private Vector3 bodyOriginalOffset;
    //public SkeletonAnimation skeletonAnimation; // SkeletonAnimation 컴포넌트 참조
    [SerializeField] private Camera cam; // 카메라 참조
    public float zoomMultiplier = 2.0f;
    public float minZoom = 2f;
    public float maxZoom = 5f;
    private float smoothTime = 0.1f;
    private float clickRadius = 0.3f; // 클릭 인식 범위 반경
    public float moveSmoothTime = 0.1f; // 카메라 이동 부드럽기

    private Transform catTransform; // Cat 오브젝트의 Transform
    private float targetZoom;
    private float zoomVelocity = 0f;
    private Vector3 cameraVelocity = Vector3.zero;
    private bool isZoomedIn = false; // 현재 줌인 상태를 추적

    public GameObject backButton; // 돌아가기 버튼 오브젝트
    public GameObject toyButton;
    public GameObject ToyExitButton;
    public GameObject bar;
    public GameObject feather;

    public GameObject player; // Unity Inspector에서 플레이어 오브젝트 할당
    private float interactionDistance = 0.5f; // 상호작용 가능 거리

    public SkeletonAnimation anim;

    public bool modifying;
    private void Awake()
    {
        instance = this;
        //skeletonAnimation = GetComponent<SkeletonAnimation>();
        //if (skeletonAnimation != null)
        //{
        //    skeletonAnimation.skeleton.ScaleX = -1f;
        //}

        //headCollider = transform.Find("Head").GetComponent<CircleCollider2D>();
        //bodyCollider = transform.Find("Body").GetComponent<BoxCollider2D>();

        //headOriginalOffset = headCollider.offset;
        //bodyOriginalOffset = bodyCollider.offset;

        //player = GameObject.Find("ChairDinningB").GetComponent<Transform>();

        // 돌아가기 버튼을 초기 비활성화
        backButton.SetActive(false);
        toyButton.SetActive(false);
        ToyExitButton.SetActive(false);
        bar.SetActive(false);
        feather.SetActive(false);
    }

    private void Start()
    {
        // Scene의 모든 GameObject를 가져와서 이름에 "Player"가 포함된 것 찾기 (이전 코드 유지)
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.name.Contains("Player"))
            {
                player = obj;
                break;
            }
        }

        GameObject catObject = GameObject.FindGameObjectWithTag("Cat");
        if (catObject != null)
        {
            catTransform = catObject.transform;
        }

        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        print(anim);
        if (!modifying)
        {
        HandleClick();

        }
         
       
        
        // 카메라의 줌 부드럽게 전환
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, smoothTime);

        if (isZoomedIn && cam.orthographicSize <= minZoom + 0.1f)
        {
            Vector3 targetPosition = new Vector3(catTransform.position.x, catTransform.position.y, cam.transform.position.z);
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPosition, ref cameraVelocity, moveSmoothTime);
        }


    }

    public void CallCat()
        {
            StartCoroutine(MoveTowards(player.transform.position- new Vector3(0.2f,0.2f,0)));
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
                    if (cam.orthographicSize > 4)
                    {
                        StartCoroutine(MoveTowards(hit.point));
                    }
                }
            }
            lastClickTime = Time.time;
        }

        if (Input.GetMouseButtonDown(0) && catTransform != null && IsPlayerInRange())
        {
            Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0f;

            float distanceToCat = Vector3.Distance(mouseWorldPosition, catTransform.position);

            if (distanceToCat <= clickRadius)
            {
                print(clickRadius);
                StartCoroutine(ZoomIn());
            }
        }
    }

    private bool IsPlayerInRange()
    {
        if (player == null) return false;
        float distance = Vector2.Distance(transform.position, player.transform.position);
        return distance <= interactionDistance;
    }

    private IEnumerator ZoomIn()
    {
        targetZoom = minZoom;
        isZoomedIn = true;

        // 줌인 완료 후 버튼을 활성화 (줌인 애니메이션 후 버튼 활성화)
        yield return new WaitForSeconds(smoothTime);
        backButton.SetActive(true);
        toyButton.SetActive(true);
    }

    public void ZoomOut()
    {
        StartCoroutine(ZoomOutCoroutine());
    }

    public IEnumerator ZoomOutCoroutine()
    {
        targetZoom = maxZoom;
        isZoomedIn = false;
        backButton.SetActive(false); // 줌아웃 시 버튼 비활성화
        toyButton.SetActive(false);
        ToyExitButton.SetActive(false);
        bar.SetActive(false);
        feather.SetActive(false);
        yield return new WaitForSeconds(smoothTime);
    }

    public void CatGo(Transform t)
    {
        StartCoroutine(MoveTowards(t.position));
    }
    public IEnumerator MoveTowards(Vector3 targetPosition)
    {
        anim.AnimationName = "Walking";
        float duration = 1.0f;
        float elapsed = 0f;
        Vector3 startingPosition = transform.position;
        Vector3 direction = (targetPosition - startingPosition).normalized;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        anim.AnimationName = "Idle";
    }

    public IEnumerator JumpUp(Vector3 targetPosition)
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Vector3 startingPosition = transform.position;
        Vector3 direction = (targetPosition - startingPosition).normalized;



        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }


}
