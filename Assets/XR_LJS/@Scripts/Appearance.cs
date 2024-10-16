using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Appearance : MonoBehaviourPunCallbacks
{
    public SpriteRenderer part;
    public Sprite[] option;
    private int _index;

    private PhotonView _photonView;

    private void Awake()
    {
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

        if (_photonView != null && _photonView.IsMine && PhotonNetwork.IsMessageQueueRunning)
        {
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
            {
                { "AppearanceIndex", Index }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
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
        if (_photonView != null && !_photonView.IsMine && targetPlayer == _photonView.Owner && changedProps.TryGetValue("AppearanceIndex", out object newIndex))
        {
            Index = (int)newIndex;
        }
    }
}