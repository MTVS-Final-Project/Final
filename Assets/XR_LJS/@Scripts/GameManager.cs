using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{

    void Update()
    {
        

    }
    
    // ���� ���� �޼���
    public void StartGameWithCustomization(Vector3 spawnPosition)
    {

        // �÷��̾� �ν��Ͻ�ȭ
        GameObject player = PhotonNetwork.Instantiate("Avatar", spawnPosition, Quaternion.identity, 0);
    }

}
