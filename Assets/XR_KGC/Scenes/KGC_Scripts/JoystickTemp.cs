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
        // 씬에서 조이스틱 찾아서 자동 할당
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

        // Transform을 사용하여 이동
        Vector2 newPosition = (Vector2)playerTransform.position + (movement * Speed * Time.deltaTime);
        playerTransform.position = newPosition;
    }
}
