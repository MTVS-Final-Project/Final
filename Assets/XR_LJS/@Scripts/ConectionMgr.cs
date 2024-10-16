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
        loadingText.text = "������ ���� ������...";
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
        loadingText.text = "�� ���� ��..";
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
        loadingText.text = "�� ����";
    }


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        loadingText.text = "�� ���� �Ϸ�";


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