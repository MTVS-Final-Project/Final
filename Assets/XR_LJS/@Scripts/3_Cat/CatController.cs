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
    //public SkeletonAnimation skeletonAnimation; // SkeletonAnimation ������Ʈ ����
    [SerializeField] private Camera cam; // ī�޶� ����
    public float zoomMultiplier = 2.0f;
    public float minZoom = 2f;
    public float maxZoom = 5f;
    private float smoothTime = 0.1f;
    private float clickRadius = 0.3f; // Ŭ�� �ν� ���� �ݰ�
    public float moveSmoothTime = 0.1f; // ī�޶� �̵� �ε巴��

    private Transform catTransform; // Cat ������Ʈ�� Transform
    private float targetZoom;
    private float zoomVelocity = 0f;
    private Vector3 cameraVelocity = Vector3.zero;
    private bool isZoomedIn = false; // ���� ���� ���¸� ����

    public GameObject backButton; // ���ư��� ��ư ������Ʈ
    public GameObject toyButton;
    public GameObject ToyExitButton;
    public GameObject bar;
    public GameObject feather;

    public GameObject player; // Unity Inspector���� �÷��̾� ������Ʈ �Ҵ�
    private float interactionDistance = 0.5f; // ��ȣ�ۿ� ���� �Ÿ�

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

        // ���ư��� ��ư�� �ʱ� ��Ȱ��ȭ
        backButton.SetActive(false);
        toyButton.SetActive(false);
        ToyExitButton.SetActive(false);
        bar.SetActive(false);
        feather.SetActive(false);
    }

    private void Start()
    {
        // Scene�� ��� GameObject�� �����ͼ� �̸��� "Player"�� ���Ե� �� ã�� (���� �ڵ� ����)
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
         
       
        
        // ī�޶��� �� �ε巴�� ��ȯ
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

        // ���� �Ϸ� �� ��ư�� Ȱ��ȭ (���� �ִϸ��̼� �� ��ư Ȱ��ȭ)
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
        backButton.SetActive(false); // �ܾƿ� �� ��ư ��Ȱ��ȭ
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
