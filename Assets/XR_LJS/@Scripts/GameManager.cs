using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab; // Player prefab

    // 게임 시작 메서드
    public void StartGameWithCustomization(Vector3 spawnPosition)
    {

        // 플레이어 인스턴스화
        GameObject player = PhotonNetwork.Instantiate("PlayerPrefab", spawnPosition, Quaternion.identity, 0);
    }

}
