using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    
    private void Start()
    {
        // 플레이어 인스턴스화
        GameObject player = PhotonNetwork.Instantiate("Avatar", transform.position, Quaternion.identity, 0);
        
    }
    void Update()
    {
        

    }
    
   
}
