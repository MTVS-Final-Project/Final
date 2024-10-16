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
        if (loadingText != null)
            loadingText.text = "마스터 서버 접속중...";
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        loadingText.text = "마스터 서버에 연결됨...";
        JoinLobby();
    }

    public void JoinLobby()
    {
        PhotonNetwork.NickName = "11"; // You might want to set this dynamically
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        loadingText.text = "방 생성 중...";
        JoinOrCreateRoom();
    }

    public void JoinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 12;

        // Join or create a room named "name"
        PhotonNetwork.JoinOrCreateRoom("name", roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        loadingText.text = "방 생성 완료. 방에 접속합니다...";
        LoadSecondScene();
    }

    public override void OnJoinedRoom()
    {
        loadingText.text = "방 접속 완료. 다른 플레이어를 기다리는 중...";
        base.OnJoinedRoom();
        // Optionally, you could also load the scene here if you want immediate transition.
        // LoadSecondScene();
    }

    public void LoadSecondScene()
    {
        // Only load the second scene if we are in a room
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LoadLevel("SecondScene_LJS");
        }
    }
    public void LoadFirstScene()
    {
        PhotonNetwork.LoadLevel("FirstScene_LJS");
    }
}
