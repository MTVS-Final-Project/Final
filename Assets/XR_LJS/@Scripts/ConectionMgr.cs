using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ConectionMgr : MonoBehaviourPunCallbacks
{
    public Text loadingText;
    public Button loadSceneButton; // 버튼에 대한 참조
    void Start()
    {
        // Photon 환경설정을 기반으로 마스터 서버에 접속을 시도
        PhotonNetwork.ConnectUsingSettings();
        loadingText.text = "마스터 서버 접속...";
    }

    void Update()
    {
        
    }

    //마스터 서버에 접속이 되면 호출되는 함수
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        loadingText.text = "마스터 서버 접속";

        // 로비 접속
        JoinLobby();
    }
    public void JoinLobby()
    {
        // 닉네임 설정
        PhotonNetwork.NickName = "이준수";
        // 기본 Lobby 입장
        PhotonNetwork.JoinLobby();
    }

    // 로비에 참여가 성공하면 호출되는 함수
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        loadingText.text = "로비 입장 완료";
        JoinOrCreateRoom();
    }
    
    // Room을 참여하자, 만약에 해당 Room이 없으면 Room을 만들겠다
    public void JoinOrCreateRoom()
    {
        // 방 생성 옵션
        RoomOptions  roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 12;

        // Room 참여 or 생성
        PhotonNetwork.JoinOrCreateRoom("name", roomOptions, TypedLobby.Default);
    }

    // 방 생성 성공했을 때 호출되는 함수
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        loadingText.text = "방 생성 완료";
    }

    // 방 입장 성공했을 때 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        loadingText.text = "방 입장 완료";
        // 버튼을 활성화하여 씬 로딩 가능하게 함

    }
    // 씬 로드를 위한 메서드
    public void LoadSecondScene()
    {
        // 버튼 클릭 시 씬을 로드
        PhotonNetwork.LoadLevel("SecondScene_LJS");
    }
}
