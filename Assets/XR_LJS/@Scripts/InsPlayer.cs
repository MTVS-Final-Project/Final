using Photon.Pun;
using UnityEngine;

public class InsPlayer : MonoBehaviour
{
    public GameObject charactorSelectUI;
    public Transform circleTransform;

    private void Start()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
    }
    public void InstantePlayer()
    {
        // 포톤으로 캐릭터 생성 되는 코드
        if (circleTransform != null)
        {
            Vector3 spawnPos = GetRandomPositionNearCircle();
            GameObject playerInstance = PhotonNetwork.Instantiate("SKPlayer", spawnPos, Quaternion.identity);
        }
        charactorSelectUI.SetActive(false);
    }

    Vector3 GetRandomPositionNearCircle()
    {
        float offX = Random.Range(-0.1f, 0.1f);
        float offY = Random.Range(-0.1f, 0.1f);
        return circleTransform != null ? circleTransform.position + new Vector3(offX, offY, 0) : Vector3.zero;
    }
}
