using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

public class TouchCharacterMovement : MonoBehaviourPunCallbacks, IPunObservable
{
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private Rigidbody2D rb;
    private SpriteRenderer[] spriteRenderers; // 캐릭터의 모든 SpriteRenderer들
    public TMP_Text playerNickname; // 플레이어 닉네임 담는 변수 선언.
    public string restrictedSceneName = "FirstScene_LJS"; // 특정 씬 이름
    public GameObject cat; // 고양이가 존재하는지 확인할 변수
    public GameObject circle; // circle 오브젝트 참조하는 변수
    private bool isFlipped = false; // 캐릭터의 flipX 상태를 나타내는 변수
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (cat == null)
        {
            cat = GameObject.Find("Cat");
        }
        if (circle == null)
        {
            circle = GameObject.Find("Circle");
        }

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        if (photonView.IsMine)
        {
            Debug.Log("플레이어 캐릭터 스크립트가 시작되었습니다.");
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (SceneManager.GetActiveScene().name == restrictedSceneName)
        {
            Debug.Log("이 씬에서는 캐릭터 이동이 제한됩니다.");
            return;
        }

        // Circle 오브젝트가 활성화된 상태에서 마우스 클릭 감지
        if (circle.activeInHierarchy)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                if (hit.collider != null && hit.collider.CompareTag("Ground"))
                {
                    targetPosition = hit.point;
                    targetPosition.z = transform.position.z;
                    isMoving = true;

                    Debug.Log("Ground를 클릭했습니다. 목표 위치가 설정되었습니다: " + targetPosition);
                }
                else
                {
                    Debug.Log("Ground 콜라이더 외부를 클릭했습니다.");
                }
            }
        }
        else if (cat.activeInHierarchy)
        {
            Debug.Log("고양이가 활성화되어 있어 이동이 중지되었습니다.");
            return;
        }

        if (isMoving)
        {
            Debug.Log("목표 위치로 이동 중: " + targetPosition);

            // 이동 방향에 따라 캐릭터의 모든 SpriteRenderer에 flipX 적용
            if (targetPosition.x > transform.position.x)
            {
                SetCharacterFlip(true);
            }
            else if (targetPosition.x < transform.position.x)
            {
                SetCharacterFlip(false);
            }

            // 캐릭터 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            float remainingDistance = Vector3.Distance(transform.position, targetPosition);
            Debug.Log("남은 거리: " + remainingDistance);

            // 목적지에 도착했을 경우 이동 중지
            if (remainingDistance < 0.1f)
            {
                Debug.Log("목표 위치에 도착했습니다.");
                isMoving = false;
            }
        }
    }

    // 캐릭터의 모든 SpriteRenderer에 flipX 적용하는 함수
    void SetCharacterFlip(bool flip)
    {
        isFlipped = flip;
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.flipX = flip;
        }
        Debug.Log("캐릭터 방향이 설정되었습니다: " + (flip ? "오른쪽" : "왼쪽"));
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(isMoving);
            stream.SendNext(targetPosition);
            stream.SendNext(isFlipped);
        }
        else if(stream.IsReading)
        {
            transform.position = (Vector3)stream.ReceiveNext();
            isMoving = (bool)stream.ReceiveNext();
            targetPosition = (Vector3)stream.ReceiveNext();
            isFlipped = (bool)stream.ReceiveNext(); // Receive flipX state

            // Apply received flipX state
            SetCharacterFlip(isFlipped);
        }
    }
}
