using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Rendering;

public class Appearance : MonoBehaviourPunCallbacks
{
    public static Appearance appearance;
    public SpriteRenderer part;
    public Sprite[] option;
    private int _index = 0; // 기본값을 0으로 설정

    private PhotonView _photonView;

    // HashTable을 읽어온다
    // HashTable에서 데이터를 읽어온다
    //

    private void Awake()
    {
        appearance = this;
        _photonView = GetComponent<PhotonView>();
    }

    public int Index
    {
        get => _index;
        set
        {
            _index = Mathf.Clamp(value, 0, option.Length - 1);
            UpdateSprite();
        }
    }

    void Start()
    {
        if (_photonView == null)
        {
            return;
        }

        if (_photonView.IsMine)
        {
            if (PhotonNetwork.LocalPlayer != null && PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("AppearanceIndex", out object savedIndex))
            {
                Index = (int)savedIndex;
            }
        }
        else if (_photonView.Owner != null && _photonView.Owner.CustomProperties.TryGetValue("AppearanceIndex", out object remoteIndex))
        {
            Index = (int)remoteIndex;
        }

    }

    // 번호에 맞춰서 이미지 변경
    private void UpdateSprite()
    {
        if (part != null && Index >= 0 && Index < option.Length)
        {
            part.sprite = option[Index];
        }
    }

    [PunRPC]
    public void Swap()
    {
        
        Index = (Index + 1) % option.Length;
        if (part.gameObject.CompareTag("Hair"))
        {
            print("hair만");
            int hairyellow = 0;
            if (Index == 0 && option[hairyellow])
            {
                PlayerPrefs.SetInt("selectedCharacter", 1);  // "SKPlayer" 선택
                print("SKPlayer을 저장");
            }
            else if (Index == 1 && option[1])
            {
                PlayerPrefs.SetInt("selectedCharacter", 2);  // "SKPlayer2" 선택
                print("SKPlayer2를 저장");
            }
        }

        if (_photonView != null && _photonView.IsMine && PhotonNetwork.IsMessageQueueRunning)
        {
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
            {
                { gameObject.name, Index }
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

            Debug.Log("Custom properties Set: " + gameObject.name + ", " + Index);
            //PhotonNetwork.LocalPlayer.CustomProperties[gameObject.name];
        }

    }

    public void SwapLocalAndSync()
    {
        if (_photonView != null && PhotonNetwork.IsMessageQueueRunning)
        {
            _photonView.RPC("Swap", RpcTarget.All);
        }
        else
        {
            Debug.LogWarning("Cannot sync appearance change: PhotonView is null or network is not ready.");
            Swap(); // Fall back to local swap if network is not available
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (_photonView != null && !_photonView.IsMine && targetPlayer == _photonView.Owner && changedProps.TryGetValue(gameObject.name, out object newIndex))
        {
            Index = (int)newIndex;
        }
    }
}