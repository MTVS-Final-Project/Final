using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    
    private void Start()
    {
        // 플레이어 인스턴스화
        GameObject player = PhotonNetwork.Instantiate("Avatar1", transform.position, Quaternion.identity);
        
    }
    void Update()
    {
        

    }
    
   
}
