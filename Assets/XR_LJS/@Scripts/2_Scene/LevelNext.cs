using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNext : MonoBehaviourPunCallbacks
{
    public void FirstScene()
    {
        SceneManager.LoadScene("Room_KGC");
    }
}