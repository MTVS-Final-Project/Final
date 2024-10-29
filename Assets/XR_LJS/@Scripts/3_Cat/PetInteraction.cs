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

    private bool isZoomedIn = false;
    private Vector3 originalCameraPosition;
    private float originalZoom;

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
        else if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick();
        }
    }

    private void HandleClick()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject == head || hit.collider.gameObject == body)
            {
                ZoomIn();
                initialMousePosition = Input.mousePosition;
            }
            else if (hit.collider.CompareTag("Ground"))
            {
                StartCoroutine(ApproachCat(hit.point));
            }
        }
    }

    private void HandleRightClick()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

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

    private IEnumerator ApproachCat(Vector3 targetPosition)
    {
        Vector3 startPos = transform.position;
        float distance = Vector3.Distance(startPos, targetPosition);

        if (distance > approachDistance)
        {
            float elapsed = 0;
            float duration = distance / 2.0f;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(startPos, targetPosition, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPosition;
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
                StartCoroutine(MoveCatAway());
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

    private IEnumerator MoveCatAway()
    {
        Vector3 startPosition = transform.parent.position;
        Vector3 targetPosition = startPosition + new Vector3(1, 1, 0);
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.parent.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 정확한 최종 위치로 설정
        transform.parent.position = targetPosition;
    }
}