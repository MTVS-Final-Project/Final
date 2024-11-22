using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using SceneManager = UnityEngine.SceneManagement.SceneManager;
using Spine.Unity;
public class TouchCharacterMovement : MonoBehaviourPunCallbacks, IPunObservable
{
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private Rigidbody2D rb;
    private SpriteRenderer[] spriteRenderers;
    public TMP_Text playerNickname;
    public string restrictedSceneName = "FirstScene_LJS";
    public GameObject cat;
    public GameObject circle;
    private bool isFlipped = false;

    // ��Ʈ��ũ ����ȭ�� ���� ������
    public Vector3 networkPosition;
    private Vector3 previousPosition;
    private bool isNetworkMoving = false;

    SkeletonAnimation sk;
    void Start()
    {
        sk = GetComponent<SkeletonAnimation>();

        if (cat == null) cat = GameObject.Find("Cat");
        if (circle == null) circle = GameObject.Find("Circle");

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        // 현재 씬 이름 가져오기
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (currentSceneName == "FirstScene_LJS")
        {
            // FirstScene_LJS 씬에서는 강제로 (0,0,0) 위치 지정
            transform.position = Vector3.zero;
            networkPosition = Vector3.zero;
            targetPosition = Vector3.zero;
            previousPosition = Vector3.zero;
        }
        else
        {
            networkPosition = transform.position;
            targetPosition = transform.position;
            previousPosition = transform.position;
        }

        //lastPositionSyncTime = Time.time;
        if (photonView.IsMine)
        {
            //ApplyCustomization();
            StartCoroutine(LoadPositionAfterSceneLoad());
            Debug.Log("로컬 플레이어 초기화 완료");
        }
    }
    IEnumerator LoadPositionAfterSceneLoad()
    {
        yield return new WaitForSeconds(0.5f);

        string currentSceneName = SceneManager.GetActiveScene().name;


    }


    void Update()
    {
        if (photonView.IsMine)
        {
            HandleLocalPlayerMovement();

            if (Vector3.Distance(transform.position, previousPosition) > 0.001f)
            {
                sk.AnimationName = "animation";
            }
            else
            {
                isMoving = false;
                sk.ClearState();  // 상태 초기화
            }
            previousPosition = transform.position;
        }
        else
        {
            //transform.position = Vector3.Lerp(transform.position, networkPosition, 0.7f);
            transform.position = networkPosition;

            if (Vector3.Distance(transform.position, previousPosition) > 0.001f)
            {
                sk.AnimationName = "animation";
            }
            else
            {
                sk.ClearState();  // 상태 초기화
            }

            previousPosition = transform.position;
        }
    }

    void HandleLocalPlayerMovement()
    {
        if (SceneManager.GetActiveScene().name == restrictedSceneName)
        {
            Debug.Log("이 씬에서는 캐릭터 이동이 제한됩니다.");
            return;
        }

        if (circle != null && circle.activeInHierarchy)
        {
            HandleMouseInput();
        }
        else if (cat != null && cat.activeInHierarchy)
        {

            return;
        }

        if (isMoving)
        {
            MoveToTarget();
        }
    }

    void HandleMouseInput()
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

            }
        }
    }

    void MoveToTarget()
    {
        bool shouldFlip = targetPosition.x > transform.position.x;
        SetCharacterFlip(shouldFlip);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }


    void SetCharacterFlip(bool flip)
    {
        if (isFlipped != flip)
        {
            isFlipped = flip;
            photonView.RPC("SyncFlip", RpcTarget.All, flip);
        }
    }

    [PunRPC]
    void SyncFlip(bool flip)
    {
        isFlipped = flip;
        // Spine Skeleton을 플립
        if (sk != null)
        {
            sk.skeleton.ScaleX = flip ? -1f : 1f;  // 플립 상태에 따라 ScaleX 변경
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(isMoving);
            stream.SendNext(isFlipped);  // 플립 상태도 전송
        }
        else
        {
            // 데이터를 수신
            networkPosition = (Vector3)stream.ReceiveNext();
            isNetworkMoving = (bool)stream.ReceiveNext();
            bool newFlip = (bool)stream.ReceiveNext();  // 플립 상태 수신

            // 수신한 플립 상태에 따라 캐릭터 플립 처리
            if (isFlipped != newFlip)
            {
                SetCharacterFlip(newFlip);  // SetCharacterFlip 호출하여 플립 상태 적용
            }

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.gameObject.name);
    }

}