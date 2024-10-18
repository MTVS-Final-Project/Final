using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public int hairStyle;
    public int eyeStyle;
    public int mouthStyle;
    public int bodyStyle;
    public int pantStyle;
    public int leftArmStyle;
    public int rightArmStyle;

    private void Awake()
    {
        // PhotonView의 Observed Components 목록에 Transform을 추가
        PhotonView photonView = GetComponent<PhotonView>();

        // Transform 컴포넌트 동기화
        if (photonView != null && !photonView.ObservedComponents.Contains(transform))
        {
            photonView.ObservedComponents.Add(transform);
        }

        // PlayerController 스크립트를 동기화할 경우 추가
        if (photonView != null && !photonView.ObservedComponents.Contains(this))
        {
            photonView.ObservedComponents.Add(this); // PlayerController도 동기화할 경우 추가 가능
        }
    }

    void Start()
    {
        // 인스턴스 데이터가 있는 경우 커스터마이징 데이터 적용
        if (photonView.InstantiationData != null && photonView.InstantiationData.Length > 0)
        {
            string jsonData = (string)photonView.InstantiationData[0];
            CharacterCustomizationData customData = CharacterCustomizationData.FromJson(jsonData);
            ApplyCustomization(customData);
        }
    }

    private void ApplyCustomization(CharacterCustomizationData customData)
    {
        hairStyle = customData.hairStyle;
        eyeStyle = customData.eyeStyle;
        mouthStyle = customData.mouthStyle;
        bodyStyle = customData.bodyStyle;
        pantStyle = customData.pantStyle;
        leftArmStyle = customData.leftArmStyle;
        rightArmStyle = customData.rightArmStyle;

        // 여기에 실제 모델에 스타일을 적용하는 코드를 추가합니다.
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 로컬 클라이언트의 데이터를 보냅니다
            stream.SendNext(hairStyle);
            stream.SendNext(eyeStyle);
            stream.SendNext(mouthStyle);
            stream.SendNext(bodyStyle);
            stream.SendNext(pantStyle);
            stream.SendNext(leftArmStyle);
            stream.SendNext(rightArmStyle);

            // 위치와 회전 데이터도 동기화
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // 원격 클라이언트에서 데이터를 받습니다
            hairStyle = (int)stream.ReceiveNext();
            eyeStyle = (int)stream.ReceiveNext();
            mouthStyle = (int)stream.ReceiveNext();
            bodyStyle = (int)stream.ReceiveNext();
            pantStyle = (int)stream.ReceiveNext();
            leftArmStyle = (int)stream.ReceiveNext();
            rightArmStyle = (int)stream.ReceiveNext();

            // 위치와 회전 데이터 동기화
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
