using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab; // Player prefab

    // ���� ���� �޼���
    public void StartGameWithCustomization(Vector3 spawnPosition)
    {

        // �÷��̾� �ν��Ͻ�ȭ
        GameObject player = PhotonNetwork.Instantiate("PlayerPrefab", spawnPosition, Quaternion.identity, 0);
    }

}
