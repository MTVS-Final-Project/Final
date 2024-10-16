using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ConectionMgr : MonoBehaviourPunCallbacks
{
    [SerializeField] Text loadingText;
    void Start()
    {
        
        PhotonNetwork.ConnectUsingSettings();
        if(loadingText != null )
        loadingText.text = "付胶磐 辑滚 立加吝...";
    }

    void Update()
    {

    }

    
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        loadingText.text = "...";

        JoinLobby();
    }
    public void JoinLobby()
    {
        
        PhotonNetwork.NickName = "11";
        
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        loadingText.text = "规 积己 吝..";
        JoinOrCreateRoom();
    }

    public void JoinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 12;

        PhotonNetwork.JoinOrCreateRoom("name", roomOptions, TypedLobby.Default);
    }


    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        loadingText.text = "规 积己";
    }


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        loadingText.text = "规 立加 肯丰";


    }

    public void LoadSecondScene()
    {
        PhotonNetwork.LoadLevel("SecondScene_LJS");
    }

    public void LoadFirstScene()
    {
        PhotonNetwork.LoadLevel("FirstScene_LJS");
    }
}