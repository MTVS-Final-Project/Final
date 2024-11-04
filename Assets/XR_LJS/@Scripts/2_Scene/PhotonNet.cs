using UnityEngine;
using Photon.Pun;

public class PhotonNet : MonoBehaviourPunCallbacks
{
    public Transform catTransform; // 에디터에서 고양이 객체의 Transform을 연결합니다.

    void Start()
    {
        // RPC 보내는 빈도 설정
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;

        // 고양이 위치 근처의 위치 계산
        Vector3 spawnPosition = GetRandomPositionNearCat();

        // 스폰 위치 확인 로그
        Debug.Log("Spawn Position: " + spawnPosition);

        // 객체 생성
        GameObject playerInstance =  PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);

        // CatController의 player 변수에 자동 할당
        CatController.instance.player = playerInstance;
    }

    // 고양이 위치 근처의 랜덤 위치를 반환하는 메서드
    Vector3 GetRandomPositionNearCat()
    {

        // 고양이의 스케일을 고려하여 오프셋 범위 조정
        float offsetX = Random.Range(-0.5f, 0.5f); // X축 오프셋 범위 축소
        float offsetY = Random.Range(-0.5f, 0.5f); // Y축 오프셋 범위 축소
        float offsetZ = 0; // 2D일 경우 Z축은 0으로 유지

        if (catTransform != null)
        {
            return catTransform.position + new Vector3(offsetX, offsetY, offsetZ);
        }
        else
        {
            Debug.LogError("catTransform is not assigned!");
            return Vector3.zero;
        }
    }

    void Update()
    {
        // Update 로직 추가 가능
    }
}
