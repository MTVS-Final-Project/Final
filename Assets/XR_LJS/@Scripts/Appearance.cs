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
        // 외형 인덱스가 저장된 Custom Properties를 불러옴
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
            part.sprite = option[index]; // 스프라이트 업데이트
        }
    }

    // 외형 변경 시 Custom Properties도 갱신
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

        // 변경 사항을 Photon Custom Properties에 저장
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
        {
            { "AppearanceIndex", index }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    // Photon의 Player Properties 업데이트를 위한 콜백 함수
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
