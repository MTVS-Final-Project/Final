using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class TouchCharacterMovement : MonoBehaviourPunCallbacks, IPunObservable
{
    public float moveSpeed = 5f;
    public Vector2 movement = new Vector2();
    private Vector3 targetPosition;
    private bool isMoving = false;
    private PhotonView pv;
    Rigidbody2D rb;
    private SpriteRenderer[] spriteRenderers; // 캐릭터의 모든 SpriteRenderer들
    public TMP_Text playerNickname; // 플레이어 닉네임 담는 변수 선언.

    // 고양이 GameObject를 참조하는 변수 추가
    public GameObject cat; // 고양이가 존재하는지 확인할 변수

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 캐릭터의 모든 SpriteRenderer를 찾음 (하위 오브젝트 포함)
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        if (photonView.InstantiationData != null && photonView.InstantiationData.Length > 0)
        {
            string jsonData = (string)photonView.InstantiationData[0];
            CharacterCustomizationData customData = CharacterCustomizationData.FromJson(jsonData);
        }

        //playerNickname.text = photonView.Owner.NickName;
        cat = GameObject.Find("Cat").GetComponent<GameObject>();
    }

    void Update()
    {
        // 윈도우 테스트 용
        if (photonView.IsMine)
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");
            movement.Normalize();
            rb.linearVelocity = movement * moveSpeed;
        }

        if (!photonView.IsMine) return; // 자신의 캐릭터만 제어

        // 활성화된 버튼의 개수를 확인
        int activeButtonCount = CountActiveButtons();

        // 화면에 보이는 버튼이 5개 이상이면 캐릭터 움직임을 막음
        if (activeButtonCount >= 5)
        {
            return;
        }

        // 고양이가 존재하면 이동 중지
        if (cat != null && cat.activeInHierarchy)
        {
            return; // 고양이가 활성화된 경우 캐릭터의 이동을 멈춤
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));
                targetPosition.z = transform.position.z;
                isMoving = true;
            }
        }

        if (isMoving)
        {
            // 이동 방향에 따라 캐릭터의 모든 SpriteRenderer에 반대 flipX를 적용
            if (targetPosition.x > transform.position.x)
            {
                SetCharacterFlip(true); // 오른쪽으로 이동할 때 flipX를 true로 설정
            }
            else if (targetPosition.x < transform.position.x)
            {
                SetCharacterFlip(false); // 왼쪽으로 이동할 때 flipX를 false로 설정
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

    // 캐릭터의 모든 SpriteRenderer에 flipX 적용하는 함수
    void SetCharacterFlip(bool flip)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.flipX = flip;
        }
    }

    // 활성화된 버튼의 개수를 계산하는 함수
    int CountActiveButtons()
    {
        Button[] buttons = FindObjectsOfType<Button>(); // 화면에 있는 모든 버튼을 가져옴
        int activeButtonCount = 0;

        foreach (Button button in buttons)
        {
            if (button.gameObject.activeInHierarchy) // 활성화된 버튼만 카운트
            {
                activeButtonCount++;
            }
        }

        return activeButtonCount;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(isMoving);
            stream.SendNext(targetPosition);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            isMoving = (bool)stream.ReceiveNext();
            targetPosition = (Vector3)stream.ReceiveNext();
        }
    }
}
