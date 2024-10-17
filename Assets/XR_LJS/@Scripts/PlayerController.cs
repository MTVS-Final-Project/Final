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
            stream.SendNext(hairStyle);
            stream.SendNext(eyeStyle);
            stream.SendNext(mouthStyle);
            stream.SendNext(bodyStyle);
            stream.SendNext(pantStyle);
            stream.SendNext(leftArmStyle);
            stream.SendNext(rightArmStyle);
        }
        else
        {
            hairStyle = (int)stream.ReceiveNext();
            eyeStyle = (int)stream.ReceiveNext();
            mouthStyle = (int)stream.ReceiveNext();
            bodyStyle = (int)stream.ReceiveNext();
            pantStyle = (int)stream.ReceiveNext();
            leftArmStyle = (int)stream.ReceiveNext();
            rightArmStyle = (int)stream.ReceiveNext();
        }
    }
}
