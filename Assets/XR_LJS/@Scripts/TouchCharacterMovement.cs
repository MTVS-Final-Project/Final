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
    private SpriteRenderer[] spriteRenderers; // ĳ������ ��� SpriteRenderer��
    public TMP_Text playerNickname; // �÷��̾� �г��� ��� ���� ����.
    public string restrictedSceneName = "FirstScene_LJS"; // Ư�� �� �̸�
    public GameObject cat; // ����̰� �����ϴ��� Ȯ���� ����
    public GameObject circle; // circle ������Ʈ �����ϴ� ����
    private bool isFlipped = false; // ĳ������ flipX ���¸� ��Ÿ���� ����
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
            Debug.Log("�÷��̾� ĳ���� ��ũ��Ʈ�� ���۵Ǿ����ϴ�.");
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (SceneManager.GetActiveScene().name == restrictedSceneName)
        {
            Debug.Log("�� �������� ĳ���� �̵��� ���ѵ˴ϴ�.");
            return;
        }

        // Circle ������Ʈ�� Ȱ��ȭ�� ���¿��� ���콺 Ŭ�� ����
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

                    Debug.Log("Ground�� Ŭ���߽��ϴ�. ��ǥ ��ġ�� �����Ǿ����ϴ�: " + targetPosition);
                }
                else
                {
                    Debug.Log("Ground �ݶ��̴� �ܺθ� Ŭ���߽��ϴ�.");
                }
            }
        }
        else if (cat.activeInHierarchy)
        {
            Debug.Log("����̰� Ȱ��ȭ�Ǿ� �־� �̵��� �����Ǿ����ϴ�.");
            return;
        }

        if (isMoving)
        {
            Debug.Log("��ǥ ��ġ�� �̵� ��: " + targetPosition);

            // �̵� ���⿡ ���� ĳ������ ��� SpriteRenderer�� flipX ����
            if (targetPosition.x > transform.position.x)
            {
                SetCharacterFlip(true);
            }
            else if (targetPosition.x < transform.position.x)
            {
                SetCharacterFlip(false);
            }

            // ĳ���� �̵�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            float remainingDistance = Vector3.Distance(transform.position, targetPosition);
            Debug.Log("���� �Ÿ�: " + remainingDistance);

            // �������� �������� ��� �̵� ����
            if (remainingDistance < 0.1f)
            {
                Debug.Log("��ǥ ��ġ�� �����߽��ϴ�.");
                isMoving = false;
            }
        }
    }

    // ĳ������ ��� SpriteRenderer�� flipX �����ϴ� �Լ�
    void SetCharacterFlip(bool flip)
    {
        isFlipped = flip;
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.flipX = flip;
        }
        Debug.Log("ĳ���� ������ �����Ǿ����ϴ�: " + (flip ? "������" : "����"));
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
