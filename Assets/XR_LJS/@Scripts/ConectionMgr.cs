using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ConectionMgr : MonoBehaviourPunCallbacks
{
    public Text loadingText;
    public Button loadSceneButton; // ��ư�� ���� ����
    void Start()
    {
        // Photon ȯ�漳���� ������� ������ ������ ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
        loadingText.text = "������ ���� ����...";
    }

    void Update()
    {
        
    }

    //������ ������ ������ �Ǹ� ȣ��Ǵ� �Լ�
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        loadingText.text = "������ ���� ����";

        // �κ� ����
        JoinLobby();
    }
    public void JoinLobby()
    {
        // �г��� ����
        PhotonNetwork.NickName = "���ؼ�";
        // �⺻ Lobby ����
        PhotonNetwork.JoinLobby();
    }

    // �κ� ������ �����ϸ� ȣ��Ǵ� �Լ�
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        loadingText.text = "�κ� ���� �Ϸ�";
        JoinOrCreateRoom();
    }
    
    // Room�� ��������, ���࿡ �ش� Room�� ������ Room�� ����ڴ�
    public void JoinOrCreateRoom()
    {
        // �� ���� �ɼ�
        RoomOptions  roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 12;

        // Room ���� or ����
        PhotonNetwork.JoinOrCreateRoom("name", roomOptions, TypedLobby.Default);
    }

    // �� ���� �������� �� ȣ��Ǵ� �Լ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        loadingText.text = "�� ���� �Ϸ�";
    }

    // �� ���� �������� �� ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        loadingText.text = "�� ���� �Ϸ�";
        // ��ư�� Ȱ��ȭ�Ͽ� �� �ε� �����ϰ� ��

    }
    // �� �ε带 ���� �޼���
    public void LoadSecondScene()
    {
        // ��ư Ŭ�� �� ���� �ε�
        PhotonNetwork.LoadLevel("SecondScene_LJS");
    }
}
