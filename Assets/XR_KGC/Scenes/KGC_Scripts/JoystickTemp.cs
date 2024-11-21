using UnityEngine;
using Photon.Pun;

public class joystickTemp : MonoBehaviourPunCallbacks
{
    [SerializeField] private bl_Joystick Joystick;
    [SerializeField] private float Speed = 1f;

    private Rigidbody2D rb;
    private PhotonView photonView;
    private Vector2 movement;

    void Awake()
    {
        // ������ ���̽�ƽ ã�Ƽ� �ڵ� �Ҵ�
        if (Joystick == null)
        {
            Joystick = FindObjectOfType<bl_Joystick>();
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();

        if (rb != null)
        {
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        // Ȥ�� ���̽�ƽ�� ã�� ���ߴٸ� �ٽ� �ѹ� �õ�
        if (Joystick == null)
        {
            Joystick = FindObjectOfType<bl_Joystick>();
            if (Joystick == null) return; // ���̽�ƽ�� ������ ������ ó������ ����
        }

        // ���̽�ƽ �Է°� ��������
        float horizontalInput = Joystick.Horizontal;
        float verticalInput = Joystick.Vertical;

        movement = new Vector2(horizontalInput, verticalInput);

        // �Է°��� 1�� �ʰ��ϸ� ����ȭ
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        // Rigidbody2D�� velocity�� �����Ͽ� �̵�
        rb.linearVelocity = movement * Speed;
    }
}
