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
        print("�� ���� ��...");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 12;
        PhotonNetwork.JoinOrCreateRoom("name2", roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("�� ���� �Ϸ�. �濡 �����մϴ�...");
        PhotonNetwork.LoadLevel("residential_LJS");
    }

    public override void OnJoinedRoom()
    {
        print("�� ���� �Ϸ�. \n�ٸ� �÷��̾ ��ٸ��� ��");
        if (!PhotonNetwork.InRoom)
        {
            Debug.LogError("Ŭ���̾�Ʈ�� ���� �뿡 ������� �ʾҽ��ϴ�.");
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
