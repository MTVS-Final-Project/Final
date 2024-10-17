using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab; // Player prefab

    // ���� ���� �޼���
    public void StartGameWithCustomization(Vector3 spawnPosition)
    {
        // ���� Ŀ���͸���¡ ������ ����
        CharacterCustomizationData customData = CharacterCustomizationData.CreateRandom();
        string customDataJson = customData.ToJson();

        // �÷��̾� �ν��Ͻ�ȭ
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity, 0, new object[] { customDataJson });
    }
}
