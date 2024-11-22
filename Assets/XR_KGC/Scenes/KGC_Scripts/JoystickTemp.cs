using UnityEngine;
using Photon.Pun;

public class joystickTemp : MonoBehaviourPunCallbacks
{
    [SerializeField] private bl_Joystick Joystick;
    [SerializeField] private float Speed = 1f;

    private PhotonView photonView;
    private Vector2 movement;
    private Transform playerTransform;

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
        photonView = GetComponent<PhotonView>();
        playerTransform = transform;
    }

    void Update()
    {
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

        // Transform�� ����Ͽ� �̵�
        Vector2 newPosition = (Vector2)playerTransform.position + (movement * Speed * Time.deltaTime);
        playerTransform.position = newPosition;
    }
}
