using Photon.Pun;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RediScene1 : MonoBehaviour
{
    public GameObject charactorSelectUI;
    public Transform catTransform;
    Dictionary<string, SkeletonAnimation> currentSk;
    void Start()
    {

        
    }

    public void InstantePlayer2()
    {
        PlayerPrefs.SetInt("selectedCharacter", 1);
        SceneManager.LoadScene("Room_KGC");
    }
    Vector3 GetRandomPositionNearCat()
    {
        float offsetX = Random.Range(-0.1f, 0.1f);
        float offsetY = Random.Range(-0.1f, 0.1f);
        return catTransform != null ? catTransform.position + new Vector3(offsetX, offsetY, 0) : Vector3.zero;
    }
}
