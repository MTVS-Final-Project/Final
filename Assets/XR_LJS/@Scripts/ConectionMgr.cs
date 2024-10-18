using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ConectionMgr : MonoBehaviourPunCallbacks
{
    [SerializeField] Text loadingText;
    [SerializeField] Button joinButton; // 새로 추가한 버튼

    
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        if (loadingText != null)
            loadingText.text = "마스터 서버 접속중...";

        // 버튼 이벤트 리스너 추가
        if (joinButton != null)
            joinButton.onClick.AddListener(JoinOrCreateRoom);
        ReassignPhotonViewID();
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
        loadingText.text = "결정 버튼을 눌러 방에 참가하세요.";
        joinButton.gameObject.SetActive(true); // 버튼 활성화
    }

    public void JoinOrCreateRoom()
    {
        loadingText.text = "방 생성 중...";
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 12;
        PhotonNetwork.JoinOrCreateRoom("name", roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        loadingText.text = "방 생성 완료. 방에 접속합니다...";
    }

    public override void OnJoinedRoom()
    {
        loadingText.text = "방 접속 완료. \n다른 플레이어를 기다리는 중";
        if (!PhotonNetwork.InRoom)
        {
            Debug.LogError("클라이언트가 같은 룸에 연결되지 않았습니다.");
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
    public void ReassignPhotonViewID()
    {
        // PhotonView를 가져옴
        PhotonView photonView = GetComponent<PhotonView>();

        // 현재 로컬 플레이어가 마스터 클라이언트일 때만 재설정
        if (PhotonNetwork.IsMasterClient)
        {
            // 새로운 View ID 할당
            bool success = PhotonNetwork.AllocateViewID(photonView);

            if (success)
            {
                Debug.Log("Photon View ID가 성공적으로 재할당되었습니다: " + photonView.ViewID);
            }
            else
            {
                Debug.LogError("Photon View ID 할당 실패!");
            }
        }
    }

    public void LoadFirstScene()
    {
        PhotonNetwork.LoadLevel("FirstScene_LJS");
    }
}