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
        // PhotonView�� Observed Components ��Ͽ� Transform�� �߰�
        PhotonView photonView = GetComponent<PhotonView>();

        // Transform ������Ʈ ����ȭ
        if (photonView != null && !photonView.ObservedComponents.Contains(transform))
        {
            photonView.ObservedComponents.Add(transform);
        }

        // PlayerController ��ũ��Ʈ�� ����ȭ�� ��� �߰�
        if (photonView != null && !photonView.ObservedComponents.Contains(this))
        {
            photonView.ObservedComponents.Add(this); // PlayerController�� ����ȭ�� ��� �߰� ����
        }
    }

    void Start()
    {
        // �ν��Ͻ� �����Ͱ� �ִ� ��� Ŀ���͸���¡ ������ ����
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

        // ���⿡ ���� �𵨿� ��Ÿ���� �����ϴ� �ڵ带 �߰��մϴ�.
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // ���� Ŭ���̾�Ʈ�� �����͸� �����ϴ�
            stream.SendNext(hairStyle);
            stream.SendNext(eyeStyle);
            stream.SendNext(mouthStyle);
            stream.SendNext(bodyStyle);
            stream.SendNext(pantStyle);
            stream.SendNext(leftArmStyle);
            stream.SendNext(rightArmStyle);

            // ��ġ�� ȸ�� �����͵� ����ȭ
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // ���� Ŭ���̾�Ʈ���� �����͸� �޽��ϴ�
            hairStyle = (int)stream.ReceiveNext();
            eyeStyle = (int)stream.ReceiveNext();
            mouthStyle = (int)stream.ReceiveNext();
            bodyStyle = (int)stream.ReceiveNext();
            pantStyle = (int)stream.ReceiveNext();
            leftArmStyle = (int)stream.ReceiveNext();
            rightArmStyle = (int)stream.ReceiveNext();

            // ��ġ�� ȸ�� ������ ����ȭ
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
