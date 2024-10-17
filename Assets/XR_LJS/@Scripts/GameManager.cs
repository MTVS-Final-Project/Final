using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab; // Player prefab

    // 게임 시작 메서드
    public void StartGameWithCustomization(Vector3 spawnPosition)
    {
        // 랜덤 커스터마이징 데이터 생성
        CharacterCustomizationData customData = CharacterCustomizationData.CreateRandom();
        string customDataJson = customData.ToJson();

        // 플레이어 인스턴스화
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity, 0, new object[] { customDataJson });
    }
}
