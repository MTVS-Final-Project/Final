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
    private Vector3 networkPosition;
    private Vector3 networkTargetPosition;
    private Vector3 previousPosition;
    private float networkLag = 10f;
    private float lastPositionSyncTime;
    private float syncInterval = 0.1f;
    private float lastSyncTime = 0f;
    private bool isNetworkMoving = false;

    SkeletonAnimation sk;
    void Start()
    {
        sk = GetComponent<SkeletonAnimation>();
        rb = GetComponent<Rigidbody2D>();

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
            networkTargetPosition = Vector3.zero;
            targetPosition = Vector3.zero;
            previousPosition = Vector3.zero;
        }
        else
        {
            networkPosition = transform.position;
            networkTargetPosition = transform.position;
            targetPosition = transform.position;
            previousPosition = transform.position;
        }

        lastPositionSyncTime = Time.time;
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

        if (currentSceneName != "FirstScene_LJS")
        {
            //// FirstScene_LJS가 아닌 경우에만 마지막 저장 위치 로드
            //LoadLastPosition();
        }

        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("SyncPosition", RpcTarget.All, transform.position, transform.position, false);
        }
    }
    void LoadLastPosition()
    {
        if (PlayerPrefs.HasKey("LastPositionX") && PlayerPrefs.HasKey("LastPositionY"))
        {
            float x = PlayerPrefs.GetFloat("LastPositionX");
            float y = PlayerPrefs.GetFloat("LastPositionY");
            Vector3 lastPos = new Vector3(x, y, transform.position.z);

            transform.position = lastPos;
            targetPosition = lastPos;
            photonView.RPC("SyncPosition", RpcTarget.All, lastPos, lastPos, false);
        }
    }

    void SaveCurrentPosition()
    {
        if (photonView.IsMine)
        {
            PlayerPrefs.SetFloat("LastPositionX", transform.position.x);
            PlayerPrefs.SetFloat("LastPositionY", transform.position.y);
            PlayerPrefs.Save();
        }
    }

   

    void Update()
    {
        if (photonView.IsMine)
        {
            HandleLocalPlayerMovement();

            // �ֱ����� ��ġ ����ȭ
            if (Time.time - lastSyncTime > syncInterval)
            {
                if (Vector3.Distance(transform.position, previousPosition) > 0.001f)
                {
                    photonView.RPC("SyncPosition", RpcTarget.All, transform.position, targetPosition, isMoving);
                    
                    previousPosition = transform.position;

                    StartCoroutine(MoveStop());
                }
                lastSyncTime = Time.time;
                
            }

            if (Time.time - lastPositionSyncTime > 5f)
            {
                SaveCurrentPosition();
                lastPositionSyncTime = Time.time;
            }
        }
        else
        {
            HandleRemotePlayerMovement();
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

                photonView.RPC("UpdateTargetPosition", RpcTarget.All, targetPosition);
            }
        }
    }

    void HandleRemotePlayerMovement()
    {
        if (isNetworkMoving)
        {
            Vector3 moveDirection = (networkTargetPosition - transform.position).normalized;
            Vector3 targetVelocity = moveDirection * moveSpeed;

            Vector3 smoothPosition = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * networkLag);
            transform.position = Vector3.Lerp(smoothPosition, smoothPosition + targetVelocity * Time.deltaTime, 0.5f);

            bool shouldFlip = moveDirection.x > 0;
            SetCharacterFlip(shouldFlip);

            if (Vector3.Distance(transform.position, networkTargetPosition) < 0.1f)
            {
                isNetworkMoving = false;
                transform.position = networkTargetPosition;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * networkLag);
        }
    }

    void MoveToTarget()
    {
        bool shouldFlip = targetPosition.x > transform.position.x;
        SetCharacterFlip(shouldFlip);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
            SaveCurrentPosition();
            photonView.RPC("SyncPosition", RpcTarget.All, transform.position, targetPosition, false);
        }
    }
    public IEnumerator MoveStop()
    {
        sk.AnimationName = "animation";
        yield return new WaitForSeconds(1);
        sk.ClearState();
    }

    [PunRPC]
    void UpdateTargetPosition(Vector3 newPosition)
    {
        targetPosition = newPosition;
        networkTargetPosition = newPosition;
        isMoving = true;
        isNetworkMoving = true;
    }

    [PunRPC]
    void SyncPosition(Vector3 position, Vector3 targetPos, bool moving)
    {
        networkPosition = position;
        networkTargetPosition = targetPos;
        isNetworkMoving = moving;

        if (!photonView.IsMine)
        {
            if (Vector3.Distance(transform.position, position) > 3f)
            {
                transform.position = position;
            }
        }
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
            stream.SendNext(rb.position);
            stream.SendNext(isMoving);
            stream.SendNext(targetPosition);
            stream.SendNext(isFlipped);  // 플립 상태도 전송
            stream.SendNext(networkTargetPosition);
        }
        else
        {
            // 데이터를 수신
            networkPosition = (Vector3)stream.ReceiveNext();
            isNetworkMoving = (bool)stream.ReceiveNext();
            networkTargetPosition = (Vector3)stream.ReceiveNext();
            bool newFlip = (bool)stream.ReceiveNext();  // 플립 상태 수신
            Vector3 receivedTargetPos = (Vector3)stream.ReceiveNext();

            // 네트워크 지연 시간 계산 후 위치 보정
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            networkPosition += new Vector3(rb.linearVelocity.x * lag, rb.linearVelocity.y * lag, 0);

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