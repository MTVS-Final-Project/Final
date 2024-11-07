using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Conet : MonoBehaviourPunCallbacks
{
    

    private void Awake()
    {
        
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("마스터 서버에 연결됨...");
        JoinLobby();
    }

    public void JoinLobby()
    {
        PhotonNetwork.NickName = GenerateRandomNumberNickname(); // 랜덤 숫자 닉네임 설정
        PhotonNetwork.JoinLobby();
    }

    private string GenerateRandomNumberNickname()
    {
        int randomNumber = Random.Range(1000, 9999); // 1000부터 9999 사이의 랜덤 숫자 생성
        return randomNumber.ToString();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비 들어왔음.");
        
    }
    //public void JoinOrCreateRoom()
    //{
    //    print("방 생성 중...");
    //    RoomOptions roomOptions = new RoomOptions();
    //    roomOptions.MaxPlayers = 12;
    //    PhotonNetwork.JoinOrCreateRoom("name", roomOptions, TypedLobby.Default);
    //}


}
