using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
public class ConectionMG : MonoBehaviourPunCallbacks
{
    [SerializeField] Button roomButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (roomButton != null)
        {
            roomButton.onClick.AddListener(OnConnected3);
            roomButton.onClick.AddListener(JoinOrCreateRoom2);
        }
    }
    public void OnConnected3()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = "";
        PhotonNetwork.GameVersion = "1.0.0";
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinOrCreateRoom2()
    {
        print("방 생성 중...");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 12;
        PhotonNetwork.JoinOrCreateRoom("name2", roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방 생성 완료. 방에 접속합니다...");
        PhotonNetwork.LoadLevel("residential_LJS");
    }

    public override void OnJoinedRoom()
    {
        print("방 접속 완료. \n다른 플레이어를 기다리는 중");
        if (!PhotonNetwork.InRoom)
        {
            Debug.LogError("클라이언트가 같은 룸에 연결되지 않았습니다.");
        }
        base.OnJoinedRoom();

    }

    public void LoadFirstScene()
    {
        PhotonNetwork.LeaveRoom();

    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.LoadLevel("Room_KGC");
    }
}
