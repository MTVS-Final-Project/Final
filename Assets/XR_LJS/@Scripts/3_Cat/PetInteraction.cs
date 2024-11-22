using UnityEngine;
using System.Collections;
using UnityEngine.XR.ARSubsystems;
using Spine.Unity;

public class PetInteraction : MonoBehaviour
{
    public CatAIFSM catAI; //����� AI������

    private Vector3 initialMousePosition;
    public GameObject head;
    public GameObject body;
   // --------------------------------------------- ����� ���� UI
    public GameObject happy; //friendly
    public GameObject annoying; //picky
    public GameObject ignoreImage;
    public GameObject superHappy;
    public GameObject negative;
    public GameObject hungry;
    public GameObject wantPlay;
    public GameObject DirtToilet;

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

    public int headClickCount = 0;      // �Ӹ� Ŭ�� Ƚ��
    public int bodyClickCount = 0;      // ���� Ŭ�� Ƚ��

    public SkeletonAnimation sk;

    public float headClickCounter = 1;
    public float bodyClickCounter = 1;

    public GameObject mainCam;
    public Vector3 camStartPos;
    

    private void Awake()
    {
    }
    private void Start()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main.gameObject;
            camStartPos = mainCam.transform.position;
        }
        happy.SetActive(false);
        annoying.SetActive(false);
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
        if (headClickCount > 0) //�Ӹ� Ŭ���� ���۵����� ī���� ����
        {
            headClickCounter -= Time.deltaTime;
            if (headClickCounter <= 0)
            {
                headClickCount = 0;
                headClickCounter = 1;
            }
        }
        if (bodyClickCount > 0) //���� Ŭ���� ���۵����� ī���� ����
        {
            bodyClickCounter -= Time.deltaTime;
            if (bodyClickCounter <= 0)
            {
                bodyClickCount = 0;
                bodyClickCounter = 1;
            }
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

                    if (headClickCount == 3)  // �Ӹ� 2ȸ Ŭ��
                    {
                        ShowPinkyReaction();
                        headClickCount = 0;
                        headClickCounter = 1;
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

                    if (bodyClickCount == 3)  // ���� 2ȸ Ŭ��
                    {
                        ShowFriendlyReaction();
                        bodyClickCount = 0;
                        bodyClickCounter = 1;
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

                    //sk.AnimationName = "Love";
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

    public void GiveMeFood()
    {
        StartCoroutine(Showhungry());
    }

    private void ShowPinkyReaction()
    {
        sk.AnimationName = "Walking";
        annoying.SetActive(true);
        happy.SetActive(false);
        // �ڷ�ƾ���� ������ �����Ͽ� ����
        StartCoroutine(HideImageAndKeepButtonsHidden(annoying));
        StartCoroutine(MoveCatAwayOnGround());
        // ������ �ִϸ��̼� ���߱�
        StartCoroutine(StopAnimationAfterReaction());
        CatController.instance.ZoomOut();                          //�ܾƿ��Ǹ鼭
        mainCam.transform.position = camStartPos;                  //����ī�޶� �ٽ� �߾�����
        DisableAllButtons();
        
    }


    private void ShowFriendlyReaction()
    {
        sk.AnimationName = "Love";

        happy.SetActive(true);
        annoying.SetActive(false);
        StartCoroutine(HideImageAndKeepButtonsShown(happy));
        // ������ ���� �� �ִϸ��̼� ���߱�
        StartCoroutine(StopAnimationAfterReaction());
    }

    private IEnumerator Showhungry()
    {
        hungry.SetActive(true);
        yield return new WaitForSeconds(2f);
        hungry.SetActive(false);
    }
    private IEnumerator HideImageAndKeepButtonsShown(GameObject image)
    {
        // �̹����� ����� ����
        yield return new WaitForSeconds(1f); // ���� ��� �ð�
        image.SetActive(false);

        // Friendly ���������� ��ư���� ���̵��� Ȱ��ȭ
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
        // ���� HideImage ����
        yield return new WaitForSeconds(1f); // ���� ��� �ð�
        image.SetActive(false);

        // ��ư���� ��� ��Ȱ��ȭ ���·� �����ǵ��� Ȯ��
        DisableAllButtons();
    }
    private IEnumerator StopAnimationAfterReaction()
    {
        // ������ ���� �� ���� �ð� �ڿ� �ִϸ��̼� ���߱�
        yield return new WaitForSeconds(1f); // ���÷� 1�� ��ٸ�
        sk.state.ClearTrack(0); // Ʈ�� 0���� �ִϸ��̼��� ����
    }
    private IEnumerator MoveCatAwayOnGround()
    {
        Vector3 startPosition = transform.parent.position;

        // x��� y�� �������� ������ �̵� ���� �����մϴ�.
        float randomX = Random.Range(-moveAwayDistance, moveAwayDistance);
        float randomY = Random.Range(-moveAwayDistance, moveAwayDistance);
        Vector3 awayDirection = new Vector3(randomX, randomY, 0); // ���� �������� �̵�
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