using Photon.Pun;
using Spine.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RediScene2 : MonoBehaviour
{
    public GameObject charactorSelectUI;
    public Transform catTransform;
    void Start()
    {
        //SkeletonAnimation sk = GetComponent<SkeletonAnimation>();

        //sk.initialSkinName = "1";
        //sk.Initialize(true);

    }

    public void InstantePlayer2()
    {

        //PlayerPrefs.SetInt("selectedCharacter", 2);
        SceneManager.LoadScene("Room_KGC");

    }

    Vector3 GetRandomPositionNearCat()
    {
        float offsetX = Random.Range(-0.1f, 0.1f);
        float offsetY = Random.Range(-0.1f, 0.1f);
        return catTransform != null ? catTransform.position + new Vector3(offsetX, offsetY, 0) : Vector3.zero;
    }
}
