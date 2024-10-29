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

    public float tapTimeWindow = 0.5f;
    public int requiredTapCount = 2;

    private int tapCount = 0;
    private float lastTapTime = 0;

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
            HandleTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick();
        }
    }

    private void HandleTouch()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        touchPosition.z = 0f;
        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

        if (hit.collider != null)
        {
            // Check object tags
            if (hit.collider.CompareTag("Head"))
            {
                StartCoroutine(MoveCatAwayOnGround());
                StartCoroutine(ShowScaredReaction());
                ZoomOut();
            }
            else if (hit.collider.CompareTag("Butt"))
            {
                StartCoroutine(ShowFriendlyReaction());
            }
        }
    }

    private void HandleClick()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, Mathf.Infinity, groundLayer);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject == head || hit.collider.gameObject == body)
            {
                ZoomIn();
                initialMousePosition = Input.mousePosition;
            }
            else if (hit.collider.CompareTag("Ground"))
            {
                StartCoroutine(ApproachCatOnGround(hit.point));
            }
        }
    }

    private void HandleRightClick()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, Mathf.Infinity, groundLayer);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject == head)
            {
                ReactToPetting("head", true);
            }
            else if (hit.collider.gameObject == body)
            {
                ReactToPetting("body", false);
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

    private void OnMouseDrag()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        float dragDistance = Vector3.Distance(initialMousePosition, currentMousePosition);

        if (dragDistance < 4.0f)
        {
            if (gameObject == head)
            {
                ReactToPetting("head", true);
            }
            else if (gameObject == body)
            {
                ReactToPetting("body", false);
            }
            ShowWhiteImage();
        }
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
        ShowWhiteImage();
    }

    private void ShowWhiteImage()
    {
        StartCoroutine(HideImage());
    }

    private IEnumerator HideImage()
    {
        yield return new WaitForSeconds(2);
        whiteImageFriendly.SetActive(false);
        whiteImagePicky.SetActive(false);
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

    private IEnumerator ShowScaredReaction()
    {
        whiteImagePicky.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        whiteImagePicky.SetActive(false);
    }

    private IEnumerator ShowFriendlyReaction()
    {
        whiteImageFriendly.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        whiteImageFriendly.SetActive(false);
    }
}