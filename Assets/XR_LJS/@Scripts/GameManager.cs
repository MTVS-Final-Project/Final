using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    
    private void Start()
    {
        // �÷��̾� �ν��Ͻ�ȭ
        GameObject player = PhotonNetwork.Instantiate("Avatar1", transform.position, Quaternion.identity);
        
    }
    void Update()
    {
        

    }
    
   
}
