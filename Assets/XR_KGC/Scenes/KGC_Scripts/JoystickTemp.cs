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
        // 씬에서 조이스틱 찾아서 자동 할당
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

        // 혹시 조이스틱을 찾지 못했다면 다시 한번 시도
        if (Joystick == null)
        {
            Joystick = FindObjectOfType<bl_Joystick>();
            if (Joystick == null) return; // 조이스틱이 없으면 움직임 처리하지 않음
        }

        // 조이스틱 입력값 가져오기
        float horizontalInput = Joystick.Horizontal;
        float verticalInput = Joystick.Vertical;

        movement = new Vector2(horizontalInput, verticalInput);

        // 입력값이 1을 초과하면 정규화
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        // Rigidbody2D의 velocity를 설정하여 이동
        rb.linearVelocity = movement * Speed;
    }
}
