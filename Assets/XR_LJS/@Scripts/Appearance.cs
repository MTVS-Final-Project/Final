using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Appearance : MonoBehaviourPunCallbacks
{
    public SpriteRenderer part;
    public Sprite[] option;
    public int index;

    void Start()
    {
        // ���� �ε����� ����� Custom Properties�� �ҷ���
        if (!photonView.IsMine && PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("AppearanceIndex"))
        {
            index = (int)PhotonNetwork.LocalPlayer.CustomProperties["AppearanceIndex"];
            part.sprite = option[index];
        }
    }

    void Update()
    {
        if (index >= 0 && index < option.Length)
        {
            part.sprite = option[index]; // ��������Ʈ ������Ʈ
        }
    }

    // ���� ���� �� Custom Properties�� ����
    public void Swap()
    {
        if (index < option.Length - 1)
        {
            index++;
        }
        else
        {
            index = 0;
        }

        // ���� ������ Photon Custom Properties�� ����
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
        {
            { "AppearanceIndex", index }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    // Photon�� Player Properties ������Ʈ�� ���� �ݹ� �Լ�
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("AppearanceIndex"))
        {
            int newIndex = (int)changedProps["AppearanceIndex"];
            if (targetPlayer == photonView.Owner)
            {
                index = newIndex;
                part.sprite = option[index];
            }
        }
    }
}
