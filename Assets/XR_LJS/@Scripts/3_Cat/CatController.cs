using UnityEngine;
using System.Collections;

public class CatController : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2.0f;
    public float doubleClickTimeLimit = 1f;

    private float lastClickTime = 0f;
    private SpriteRenderer[] spriteRenderers;
    private Collider2D headCollider;
    private Collider2D bodyCollider;
    private Vector3 headOriginalOffset;
    private Vector3 bodyOriginalOffset;
    [SerializeField] private Camera cam; // ī�޶� ����
    public float zoomMultiplier = 2.0f;
    public float minZoom = 2f;
    public float maxZoom = 5f;
    public float smoothTime = 0.3f;
    public float clickRadius = 1.0f; // Ŭ�� �ν� ���� �ݰ�
    public float moveSmoothTime = 0.3f; // ī�޶� �̵� �ε巴��

    private Transform catTransform; // Cat ������Ʈ�� Transform
    private float targetZoom;
    private float zoomVelocity = 0f;
    private Vector3 cameraVelocity = Vector3.zero;
    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        headCollider = transform.Find("Head").GetComponent<Collider2D>();
        bodyCollider = transform.Find("Body").GetComponent<Collider2D>();

        // �ʱ� ��ġ ������ ����
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
        if (Input.GetMouseButtonDown(0) && catTransform != null) // ���콺 ���� ��ư Ŭ�� ����
        {
            Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0f;

            float distanceToCat = Vector3.Distance(mouseWorldPosition, catTransform.position);

            if (distanceToCat <= clickRadius) // Ŭ���� ����� �ֺ��� �ִ��� Ȯ��
            {
                // Cat ������Ʈ ��ó Ŭ�� �� Ȯ�� ����
                targetZoom = minZoom;
            }
        }

        // ī�޶��� �� �ε巴�� ��ȯ
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, smoothTime);

        // ī�޶� Ȯ�� ���¶�� ����̸� ���� �̵�
        if (cam.orthographicSize <= minZoom + 0.1f) // Ȯ�� �Ϸ� ��ó���� ����� ��ġ�� �̵�
        {
            Vector3 targetPosition = new Vector3(catTransform.position.x, catTransform.position.y, cam.transform.position.z);
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPosition, ref cameraVelocity, moveSmoothTime);
        }
    }

    private IEnumerator MoveTowards(Vector3 targetPosition)
    {
        float duration = 1.0f;
        float elapsed = 0f;
        Vector3 startingPosition = transform.position;
        Vector3 direction = (targetPosition - startingPosition).normalized;

        // �̵� ���⿡ ���� ��������Ʈ �� Collider ��ġ ���� ����
        if (direction.x < 0)
        {
            FlipSpriteAndCollider(false); // ����
        }
        else if (direction.x > 0)
        {
            FlipSpriteAndCollider(true); // ������
        }

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    private void FlipSpriteAndCollider(bool flipRight)
    {
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.flipX = flipRight;
        }

        // Collider�� offset ��ġ�� flip ���⿡ ���� ����
        headCollider.offset = flipRight ? new Vector2(-headOriginalOffset.x, headOriginalOffset.y) : headOriginalOffset;
        bodyCollider.offset = flipRight ? new Vector2(-bodyOriginalOffset.x, bodyOriginalOffset.y) : bodyOriginalOffset;
    }
}
