using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    
    private void Start()
    {
        // �÷��̾� �ν��Ͻ�ȭ
        GameObject player = PhotonNetwork.Instantiate("Avatar", transform.position, Quaternion.identity, 0);
        
    }
    void Update()
    {
        

    }
    
   
}
