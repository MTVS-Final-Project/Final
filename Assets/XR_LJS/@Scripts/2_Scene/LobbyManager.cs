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

    void Start()
    {
        // UI �ʱ� ����
        SetUIInteractable(false);
        connectingUI.SetActive(true);
        lobbyUI.SetActive(false);
        statusText.text = "������ ������ ���� ��...";

        // ��ư�� ������ �߰�
        createRoomButton.onClick.AddListener(CreateRoom);
        joinRoomButton.onClick.AddListener(JoinRoom);

        // ���� ����
        PhotonNetwork.ConnectUsingSettings();
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
        statusText.text = "������ �����";
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(createRoomInput.text))
        {
            statusText.text = "�� �̸��� �Է��ϼ���";
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.CreateRoom(createRoomInput.text, roomOptions);
        statusText.text = "�� ���� ��...";
        SetUIInteractable(false);
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(joinRoomInput.text))
        {
            statusText.text = "�� �̸��� �Է��ϼ���";
            return;
        }

        PhotonNetwork.JoinRoom(joinRoomInput.text);
        statusText.text = "�� ���� ��...";
        SetUIInteractable(false);
    }

    public override void OnJoinedRoom()
    {
        statusText.text = "�� ���� ����!";
        PhotonNetwork.LoadLevel("CharacterCustomization");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        statusText.text = "�� ���� ����: " + message;
        SetUIInteractable(true);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        statusText.text = "�� ���� ����: " + message;
        SetUIInteractable(true);
    }
}