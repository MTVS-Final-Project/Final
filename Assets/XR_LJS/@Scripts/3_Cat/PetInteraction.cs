using UnityEngine;
using System.Collections;

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

    private int headClickCount = 0;      // �Ӹ� Ŭ�� Ƚ��
    private int bodyClickCount = 0;      // ���� Ŭ�� Ƚ��

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
                // �Ӹ� Ŭ�� ����
                if (hit.collider.gameObject == head)
                {
                    headClickCount++;
                    bodyClickCount = 0; // �ٸ� �κ��� Ŭ�� ī��Ʈ �ʱ�ȭ

                    if (headClickCount == 2)  // �Ӹ� 2ȸ Ŭ��
                    {
                        ShowPinkyReaction();
                        headClickCount = 0;
                    }
                    else
                    {
                        initialMousePosition = Input.mousePosition;  // �巡�� ���� �ʱ� ��ġ ����
                    }
                }
                // ���� Ŭ�� ����
                else if (hit.collider.gameObject == body)
                {
                    bodyClickCount++;
                    headClickCount = 0; // �ٸ� �κ��� Ŭ�� ī��Ʈ �ʱ�ȭ

                    if (bodyClickCount == 2)  // ���� 2ȸ Ŭ��
                    {
                        ShowFriendlyReaction();
                        bodyClickCount = 0;
                    }
                    else
                    {
                        initialMousePosition = Input.mousePosition;  // �巡�� ���� �ʱ� ��ġ ����
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

            if (dragDistance > 5.0f)  // �巡�� �Ÿ� ����
            {
                // �Ӹ��� �巡���� ���
                if (headClickCount > 0)
                {
                    ShowFriendlyReaction();
                    headClickCount = 0;
                }
                // ������ �巡���� ���
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

    private void ZoomIn()
    {
        if (!isZoomedIn)
        {
            cam.orthographicSize = minZoom;
            cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);
            isZoomedIn = true;
            backButton.SetActive(true);
        }
    }

    public void ZoomOut()
    {
        cam.orthographicSize = originalZoom;
        cam.transform.position = originalCameraPosition;
        isZoomedIn = false;
        backButton.SetActive(false);
    }

    

    private void ReactToPetting(string part, bool positiveReaction)
    {
        whiteImageFriendly.SetActive(false);
        whiteImagePicky.SetActive(false);

        if (positiveReaction)
        {
            whiteImageFriendly.SetActive(true);
        }
        else
        {
            whiteImagePicky.SetActive(true);
            if (part == "body" || part == "head")
            {
                StartCoroutine(MoveCatAwayOnGround());
                ZoomOut();
            }
        }
    }

    private void ShowPinkyReaction()
    {
        whiteImagePicky.SetActive(true);
        whiteImageFriendly.SetActive(false);
        StartCoroutine(HideImage(whiteImagePicky));
    }

    private void ShowFriendlyReaction()
    {
        whiteImageFriendly.SetActive(true);
        whiteImagePicky.SetActive(false);
        StartCoroutine(HideImage(whiteImageFriendly));
    }

    private IEnumerator HideImage(GameObject image)
    {
        yield return new WaitForSeconds(2);  // 2�ʰ� �̹��� ǥ��
        image.SetActive(false);
    }

    private IEnumerator MoveCatAwayOnGround()
    {
        Vector3 startPosition = transform.parent.position;
        Vector3 awayDirection = new Vector3(moveAwayDistance, 0, 0); // Move only horizontally
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
                yield return null;
            }

            transform.parent.position = targetPosition;
        }
    }

    
}