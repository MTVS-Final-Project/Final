using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ConectionMgr : MonoBehaviourPunCallbacks
{
    [SerializeField] Text loadingText;
    [SerializeField] Button joinButton; // ���� �߰��� ��ư

    
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        if (loadingText != null)
            loadingText.text = "������ ���� ������...";

        // ��ư �̺�Ʈ ������ �߰�
        if (joinButton != null)
            joinButton.onClick.AddListener(JoinOrCreateRoom);
        
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
        loadingText.text = "���� ��ư�� ���� �濡 �����ϼ���.";
        joinButton.gameObject.SetActive(true); // ��ư Ȱ��ȭ
    }

    public void JoinOrCreateRoom()
    {
        loadingText.text = "�� ���� ��...";
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 12;
        PhotonNetwork.JoinOrCreateRoom("name", roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        loadingText.text = "�� ���� �Ϸ�. �濡 �����մϴ�...";
    }

    public override void OnJoinedRoom()
    {
        loadingText.text = "�� ���� �Ϸ�. \n�ٸ� �÷��̾ ��ٸ��� ��";
        if (!PhotonNetwork.InRoom)
        {
            Debug.LogError("Ŭ���̾�Ʈ�� ���� �뿡 ������� �ʾҽ��ϴ�.");
        }
        base.OnJoinedRoom();
        LoadSecondScene();
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