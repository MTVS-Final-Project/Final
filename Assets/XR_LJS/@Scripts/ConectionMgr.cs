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
            loadingText.text = "������ ���� ������...";
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        loadingText.text = "������ ������ �����...";
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
        loadingText.text = "�� ���� ��...";
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
        loadingText.text = "�� ���� �Ϸ�. �濡 �����մϴ�...";
        LoadSecondScene();
    }

    public override void OnJoinedRoom()
    {
        loadingText.text = "�� ���� �Ϸ�. �ٸ� �÷��̾ ��ٸ��� ��...";
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
