using UnityEngine;
using System.Collections;
using UnityEngine.XR.ARSubsystems;
using Spine.Unity;

public class PetInteraction : MonoBehaviour
{
    public CatAIFSM catAI; //고양이 AI참조용

    private Vector3 initialMousePosition;
    public GameObject head;
    public GameObject body;
    // --------------------------------------------- 고양이 반응 UI
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

    public int headClickCount = 0;      // 머리 클릭 횟수
    public int bodyClickCount = 0;      // 몸통 클릭 횟수

    public SkeletonAnimation sk;

    public float headClickCounter = 1;
    public float bodyClickCounter = 1;

    public GameObject mainCam;
    public Vector3 camStartPos;
    public float dragDis;



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
        if (headClickCount > 0) //머리 클릭이 시작됐으면 카운터 시작
        {
            headClickCounter -= Time.deltaTime;
            if (headClickCounter <= 0)
            {
                headClickCount = 0;
                headClickCounter = 1;
            }
        }
        if (bodyClickCount > 0) //몸통 클릭이 시작됐으면 카운터 시작
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
                // 머리 클릭 감지
                if (hit.collider.gameObject == head)
                {
                    headClickCount++;
                    bodyClickCount = 0; // 다른 부분의 클릭 카운트 초기화

                    if (headClickCount == 3)  // 머리 2회 클릭
                    {
                        //ShowPinkyReaction(); 고양이의 친밀도에 따라서 달라지는 반응, 기본적으로 부정적인데 친하면 안싫어함
                        if (catAI.friendly > 80)
                        {
                            Ignore();
                            catAI.mood -= 5;
                        }
                        else if (catAI.friendly > 60)
                        {
                            StartCoroutine(ShowNegative());
                        }
                        else //if (catAI.friendly > 40) //안 친하면 그냥 부정적임.
                        {
                            ShowPinkyReaction();
                        }

                        headClickCount = 0;
                        headClickCounter = 1;
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

                    if (bodyClickCount == 3)  // 몸통 2회 클릭
                    {
                        if (catAI.friendly > 80)
                        {
                            StartCoroutine(SuperHappy());
                        }
                        else if (catAI.friendly > 60)
                        {
                            ShowFriendlyReaction();//고양이 친밀도에 따라 달라지는 반응,기본적으로 긍정적임

                        }
                        else // (catAI.friendly > 40) //친밀도가 낮으면 그냥 부정적
                        {
                            ShowPinkyReaction();
                        }



                        bodyClickCount = 0;
                        bodyClickCounter = 1;
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
            dragDis = Vector3.Distance(initialMousePosition, currentMousePosition);

            if (dragDis > 5.0f)  // 드래그 거리 기준
            {
                // 머리를 드래그한 경우 //기본적으로 긍정적
                if (headClickCount > 0)
                {

                    //sk.AnimationName = "Love";
                   // ShowFriendlyReaction();
                    if (catAI.friendly > 80)
                    {
                        StartCoroutine(SuperHappy());
                    }
                    else if (catAI.friendly > 60)
                    {
                        ShowFriendlyReaction();//고양이 친밀도에 따라 달라지는 반응,기본적으로 긍정적임

                    }
                    else  //(catAI.friendly > 40) //친밀도가 낮으면 그냥 부정적
                    {
                        ShowPinkyReaction();
                    }
                    headClickCount = 0;
                    dragDis = 0;
                }
                // 몸통을 드래그한 경우
                else if (bodyClickCount > 0)
                {
                    //ShowPinkyReaction(); //기본이 부정
                    if (catAI.friendly > 80)
                    {
                        Ignore();
                        catAI.mood -= 5;

                    }
                    else if (catAI.friendly > 60)
                    {
                        StartCoroutine(ShowNegative());
                    }
                    else  //(catAI.friendly > 40) //안 친하면 그냥 부정적임.
                    {
                        ShowPinkyReaction();
                    }
                    bodyClickCount = 0;
                    dragDis = 0;
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
    public void LetsPlay()
    {
        StartCoroutine(ShowPlay());
    }
    public void GiveMeFood()
    {
        StartCoroutine(Showhungry());
    }

    private void ShowPinkyReaction()
    {
        sk.AnimationName = "Walking";
        catAI.mood -= 20;
        annoying.SetActive(true);
        happy.SetActive(false);
        // 코루틴들을 변수에 저장하여 추적
        StartCoroutine(HideImageAndKeepButtonsHidden(annoying));
        StartCoroutine(MoveCatAwayOnGround());
        // 스파인 애니메이션 멈추기
        StartCoroutine(StopAnimationAfterReaction());
        CatController.instance.ZoomOut();                          //줌아웃되면서
        mainCam.transform.position = camStartPos;                  //메인카메라 다시 중앙으로
        DisableAllButtons();

    }
    public void Negative()
    {
        StartCoroutine(ShowNegative()); 
    }
    public IEnumerator ShowPlay()
    {
        wantPlay.SetActive(true);
        yield return new WaitForSeconds(5);
        wantPlay.SetActive(false);
    }
    public IEnumerator ShowNegative()
    {
        sk.AnimationName = "HAAAAAAAA";
        catAI.mood -= 10;
        negative.SetActive(true);
        CatController.instance.ZoomOut();                          //줌아웃되면서
        yield return new WaitForSeconds(2);
        sk.AnimationName = "Idle";
        negative.SetActive(false);
    }
    public void SuperH()
    {
        StartCoroutine(SuperHappy()); 
    }
    public IEnumerator SuperHappy()
    {
        sk.AnimationName = "Love";
        catAI.mood += 20;
        superHappy.SetActive(true);
        yield return new WaitForSeconds(2);
        sk.AnimationName = "Idle";
        superHappy.SetActive(false);
    }

    public void Ignore()
    {
        StartCoroutine(ShowIgnore());
    }
    private IEnumerator ShowIgnore()
    {
        ignoreImage.SetActive(true);
        yield return new WaitForSeconds(2);
        ignoreImage.SetActive(false);
    }
    public void ShowFriendlyReaction()
    {
        sk.AnimationName = "Love";
        catAI.mood += 10;
        happy.SetActive(true);
        annoying.SetActive(false);
        StartCoroutine(HideImageAndKeepButtonsShown(happy));
        // 반응이 끝난 후 애니메이션 멈추기
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
    private IEnumerator StopAnimationAfterReaction()
    {
        // 반응이 끝난 후 일정 시간 뒤에 애니메이션 멈추기
        yield return new WaitForSeconds(1f); // 예시로 1초 기다림
        sk.AnimationName = "Idle";
        sk.state.ClearTrack(0); // 트랙 0에서 애니메이션을 멈춤
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