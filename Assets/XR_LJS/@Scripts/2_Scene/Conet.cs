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
        print("������ ������ �����...");
        JoinLobby();
    }

    public void JoinLobby()
    {
        PhotonNetwork.NickName = GenerateRandomNumberNickname(); // ���� ���� �г��� ����
        PhotonNetwork.JoinLobby();
    }

    private string GenerateRandomNumberNickname()
    {
        int randomNumber = Random.Range(1000, 9999); // 1000���� 9999 ������ ���� ���� ����
        return randomNumber.ToString();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("�κ� ������.");
        
    }
    //public void JoinOrCreateRoom()
    //{
    //    print("�� ���� ��...");
    //    RoomOptions roomOptions = new RoomOptions();
    //    roomOptions.MaxPlayers = 12;
    //    PhotonNetwork.JoinOrCreateRoom("name", roomOptions, TypedLobby.Default);
    //}


}
