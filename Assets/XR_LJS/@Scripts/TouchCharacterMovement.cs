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
    private SpriteRenderer[] spriteRenderers; // ĳ������ ��� SpriteRenderer��
    public TMP_Text playerNickname; // �÷��̾� �г��� ��� ���� ����.

    // ����� GameObject�� �����ϴ� ���� �߰�
    public GameObject cat; // ����̰� �����ϴ��� Ȯ���� ����

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // ĳ������ ��� SpriteRenderer�� ã�� (���� ������Ʈ ����)
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
        // ������ �׽�Ʈ ��
        if (photonView.IsMine)
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");
            movement.Normalize();
            rb.linearVelocity = movement * moveSpeed;
        }

        if (!photonView.IsMine) return; // �ڽ��� ĳ���͸� ����

        // Ȱ��ȭ�� ��ư�� ������ Ȯ��
        int activeButtonCount = CountActiveButtons();

        // ȭ�鿡 ���̴� ��ư�� 5�� �̻��̸� ĳ���� �������� ����
        if (activeButtonCount >= 5)
        {
            return;
        }

        // ����̰� �����ϸ� �̵� ����
        if (cat != null && cat.activeInHierarchy)
        {
            return; // ����̰� Ȱ��ȭ�� ��� ĳ������ �̵��� ����
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
            // �̵� ���⿡ ���� ĳ������ ��� SpriteRenderer�� �ݴ� flipX�� ����
            if (targetPosition.x > transform.position.x)
            {
                SetCharacterFlip(true); // ���������� �̵��� �� flipX�� true�� ����
            }
            else if (targetPosition.x < transform.position.x)
            {
                SetCharacterFlip(false); // �������� �̵��� �� flipX�� false�� ����
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

    // ĳ������ ��� SpriteRenderer�� flipX �����ϴ� �Լ�
    void SetCharacterFlip(bool flip)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.flipX = flip;
        }
    }

    // Ȱ��ȭ�� ��ư�� ������ ����ϴ� �Լ�
    int CountActiveButtons()
    {
        Button[] buttons = FindObjectsOfType<Button>(); // ȭ�鿡 �ִ� ��� ��ư�� ������
        int activeButtonCount = 0;

        foreach (Button button in buttons)
        {
            if (button.gameObject.activeInHierarchy) // Ȱ��ȭ�� ��ư�� ī��Ʈ
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
