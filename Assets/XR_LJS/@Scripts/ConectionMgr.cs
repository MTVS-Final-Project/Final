using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEngine.SceneManagement;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Button joinButton;    // Gangzang_LJS�� �̵��ϴ� ��ư
    [SerializeField] Button roomButton;    // residential_LJS�� �̵��ϴ� ��ư
    [SerializeField] Button room2Button;

    void Start()
    {
       SetupPhotonNetwork();

        if (joinButton != null)
        {
            joinButton.onClick.AddListener(ConnectToFirstRoom);
           
        }

        if (roomButton != null)
        {
            roomButton.onClick.AddListener(ConnectToSecondRoom);
            

        }
        if (room2Button != null)
        {
            room2Button.onClick.AddListener(ConnectToFirstRoom2);
            

        }

    }

    // Gangzang_LJS�� �̵��ϴ� ù ��° �� ����
    public void ConnectToFirstRoom()
    {

        JoinOrCreateRoom("room1", "Gangzang_LJS");
        //SceneManager.LoadScene("Gangzang_LJS");
    }

    // residential_LJS�� �̵��ϴ� �� ��° �� ����
    public void ConnectToSecondRoom()
    {
        JoinOrCreateRoom("room2", "residential_LJS");
        //SceneManager.LoadScene("residential_LJS");
    }

    public void ConnectToFirstRoom2()
    {
        JoinOrCreateRoom("room2", "residential_L");
        //SceneManager.LoadScene("residential_L");
    }
    public void ConnetToRoom()
    {
        PhotonNetwork.LoadLevel("Room_KGC");
        //SceneManager.LoadScene("Room_KGC");
        // SceneManager.LoadScene(2);
    }
    // PhotonNetwork �⺻ ����
    private void SetupPhotonNetwork()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = "";
        PhotonNetwork.GameVersion = "1.0.0";
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(MethodInfo.GetCurrentMethod().Name);
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnConnected()
    {
        base.OnConnected();

        print(MethodInfo.GetCurrentMethod().Name);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(MethodInfo.GetCurrentMethod().Name);
    }

    private void JoinOrCreateRoom(string roomName, string sceneToLoad)
    {
        print("�� ���� ��...");
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 12,
            IsVisible = true,
            IsOpen = true,
            
        };

        var customProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "SceneToLoad", sceneToLoad }
        };
        roomOptions.CustomRoomProperties = customProperties;

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("�� ���� �Ϸ�. �濡 �����մϴ�...");
        
    }

    public override void OnJoinedRoom()
    {
        print("�� ���� �Ϸ�. \n�ٸ� �÷��̾ ��ٸ��� ��");
        if (!PhotonNetwork.InRoom)
        {
            Debug.LogError("Ŭ���̾�Ʈ�� ���� �뿡 ������� �ʾҽ��ϴ�.");
        }
        base.OnJoinedRoom();

        string sceneToLoad = (string)PhotonNetwork.CurrentRoom.CustomProperties["SceneToLoad"];
        print(sceneToLoad);
        PhotonNetwork.LoadLevel(sceneToLoad);
    }

    public void LoadFirstScene()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.LoadLevel("Room_KGC");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        // ���ο� �÷��̾ �������� �� Ŀ���͸���¡�� ����ȭ
        if (photonView.IsMine)
        {
            photonView.RPC("UpdateCustomization", RpcTarget.AllBuffered);
        }
        print($"{newPlayer.NickName} ���� �����ϼ̽��ϴ�.");
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        print($"{otherPlayer.NickName} ���� ���� �������ϴ�.");
    }
}