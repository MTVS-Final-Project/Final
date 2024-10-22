using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    public TMP_InputField createRoomInput;
    public TMP_InputField joinRoomInput;
    public Button createRoomButton;
    public Button joinRoomButton;
    public TMP_Text statusText;
    public GameObject lobbyUI;
    public GameObject connectingUI;
    // InputNickName
    public TMP_InputField inputNickName;

    void Start()
    {
        // UI 초기 설정
        SetUIInteractable(false);
        connectingUI.SetActive(true);
        lobbyUI.SetActive(false);
        statusText.text = "master sever con...";

        // 버튼에 리스너 추가
        createRoomButton.onClick.AddListener(CreateRoom);
        joinRoomButton.onClick.AddListener(JoinRoom);

        // 서버 연결
        PhotonNetwork.ConnectUsingSettings();
        // 닉네임 설정
        PhotonNetwork.NickName = inputNickName.text;
    }

    private void SetUIInteractable(bool interactable)
    {
        createRoomButton.interactable = interactable;
        joinRoomButton.interactable = interactable;
        createRoomInput.interactable = interactable;
        joinRoomInput.interactable = interactable;
    }

    public override void OnConnectedToMaster()
    {
        connectingUI.SetActive(false);
        lobbyUI.SetActive(true);
        SetUIInteractable(true);
        statusText.text = "sever on";
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(createRoomInput.text))
        {
            statusText.text = "room name pl";
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.CreateRoom(createRoomInput.text, roomOptions);
        statusText.text = "room create...";
        SetUIInteractable(false);
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(joinRoomInput.text))
        {
            statusText.text = "room name";
            return;
        }

        PhotonNetwork.JoinRoom(joinRoomInput.text);
        statusText.text = "room j...";
        SetUIInteractable(false);
    }

    public override void OnJoinedRoom()
    {
        statusText.text = "room su!";
        PhotonNetwork.LoadLevel("CharacterCustomization");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        statusText.text = " room: " + message;
        SetUIInteractable(true);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        statusText.text = "room " + message;
        SetUIInteractable(true);
    }
}