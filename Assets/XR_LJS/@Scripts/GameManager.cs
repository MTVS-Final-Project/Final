using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{

    void Update()
    {
        

    }
    
    // 게임 시작 메서드
    public void StartGameWithCustomization(Vector3 spawnPosition)
    {

        // 플레이어 인스턴스화
        GameObject player = PhotonNetwork.Instantiate("Avatar", spawnPosition, Quaternion.identity, 0);
    }

}
