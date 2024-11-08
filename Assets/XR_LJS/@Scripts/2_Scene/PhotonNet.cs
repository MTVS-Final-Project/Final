using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.IO;
using Photon.Realtime;
public class PhotonNet : MonoBehaviourPunCallbacks
{
    // 몸 부분
    public SpriteRenderer bodyPart;
    public Sprite[] bodyOption;

    // 눈
    public SpriteRenderer eyePart;
    public Sprite[] eyeOption;
    // 입
    public SpriteRenderer mouthPart;
    public Sprite[] mouthOption;
    // 왼쪽 팔
    public SpriteRenderer leftarmPart;
    public Sprite[] leftarmOption;
    
    //오른쪽 팔
    public SpriteRenderer rightarmPart;
    public Sprite[] rightarmOption;

    // 팬츠
    public SpriteRenderer pantPart;
    public Sprite[] pantOption;

    // 헤어
    public SpriteRenderer hairPart;
    public Sprite[] hairOption;

    //왼쪽 신발
    public SpriteRenderer leftshoesPart;
    public Sprite[] leftshoesOption;

    // 오른쪽 신발
    public SpriteRenderer rightshoesPart;
    public Sprite[] rightshoesOption;

    // 왼쪽 다리
    public SpriteRenderer leftlegPart;
    public Sprite[] leftlegOption;

    // 오른쪽 다리
    public SpriteRenderer rightlegPart;
    public Sprite[] rightlegOption;


    public Transform catTransform; // 에디터에서 고양이 객체의 Transform을 연결합니다.
    public Transform circleTransform;
    void Start()
    {
        // RPC 보내는 빈도 설정
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
        if (catTransform != null)
        {
            // 고양이 위치 근처의 위치 계산
            Vector3 spawnPosition = GetRandomPositionNearCat();

            // 스폰 위치 확인 로그
            Debug.Log("Spawn Position: " + spawnPosition);

            // 객체 생성
            GameObject playerInstance = PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);

            ddddd(playerInstance);
            // CatController의 player 변수에 자동 할당
            CatController.instance.player = playerInstance;

        }
        else if (circleTransform != null)
        {
            Vector3 spawnPos = GetRandomPositionNearCircle();
            Debug.Log("spawn pos : " + spawnPos);
            GameObject playerInstance = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
            ddddd(playerInstance);
            Debug.Log("player : " + playerInstance);
        }
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

    Vector3 GetRandomPositionNearCircle()
    {
        float offX = Random.Range(-0.1f, 0.1f);
        float offY = Random.Range(-0.1f, 0.1f);
        float offZ = 0;

        if (circleTransform != null)
        {
            return circleTransform.position + new Vector3(offX, offY, offZ);
        }
        else
        {
            Debug.LogError("circleTransform is not assigned!");
            return Vector3.zero;
        }
    }
    //private void UpdateSprite()
    //{
    //    if (part != null && Index >= 0 && Index < option.Length)
    //    {
    //        part.sprite = option[Index];
    //    }
    //}
    void ddddd(GameObject player)
    {
        // Update 로직 추가 dddd

        print(PhotonNetwork.LocalPlayer.CustomProperties["BodyUI"]);
        print(PhotonNetwork.LocalPlayer.CustomProperties["EyesUI"]);
        print(PhotonNetwork.LocalPlayer.CustomProperties["MouthUI"]);
        print(PhotonNetwork.LocalPlayer.CustomProperties["LeftArmUI"]);
        print(PhotonNetwork.LocalPlayer.CustomProperties["rightArmUI"]);
        print(PhotonNetwork.LocalPlayer.CustomProperties["leftLegUI"]);
        print(PhotonNetwork.LocalPlayer.CustomProperties["rightLegUI"]);
        print(PhotonNetwork.LocalPlayer.CustomProperties["PantsUI"]);
        print(PhotonNetwork.LocalPlayer.CustomProperties["HairUI"]);
        print(PhotonNetwork.LocalPlayer.CustomProperties["leftShoesUI"]);
        print(PhotonNetwork.LocalPlayer.CustomProperties["rightShoesUI"]);

        // 상의는 몇번// 그거에 맞춰서 상의를 바꾼다
        player.transform.Find("Body1").GetComponent<SpriteRenderer>().sprite = bodyOption[(int)PhotonNetwork.LocalPlayer.CustomProperties["BodyUI"]];
        player.transform.Find("eye").GetComponent<SpriteRenderer>().sprite = eyeOption[(int)PhotonNetwork.LocalPlayer.CustomProperties["EyesUI"]];
        player.transform.Find("mouth").GetComponent<SpriteRenderer>().sprite = mouthOption[(int)PhotonNetwork.LocalPlayer.CustomProperties["MouthUI"]];
        player.transform.Find("leftArm1").GetComponent<SpriteRenderer>().sprite = leftarmOption[(int)PhotonNetwork.LocalPlayer.CustomProperties["LeftArmUI"]];
        player.transform.Find("rightArmWhite").GetComponent<SpriteRenderer>().sprite = rightarmOption[(int)PhotonNetwork.LocalPlayer.CustomProperties["rightArmUI"]];
        player.transform.Find("leftlegblack").GetComponent<SpriteRenderer>().sprite = leftlegOption[(int)PhotonNetwork.LocalPlayer.CustomProperties["leftLegUI"]];
        player.transform.Find("rightlegblack").GetComponent<SpriteRenderer>().sprite = rightlegOption[(int)PhotonNetwork.LocalPlayer.CustomProperties["rightLegUI"]];
        player.transform.Find("blackPant").GetComponent<SpriteRenderer>().sprite = pantOption[(int)PhotonNetwork.LocalPlayer.CustomProperties["PantsUI"]];
        player.transform.Find("hairblack").GetComponent<SpriteRenderer>().sprite = hairOption[(int)PhotonNetwork.LocalPlayer.CustomProperties["HairUI"]];
        player.transform.Find("leftshoesblack").GetComponent<SpriteRenderer>().sprite = leftshoesOption[(int)PhotonNetwork.LocalPlayer.CustomProperties["leftShoesUI"]];
        player.transform.Find("rightshoesblack").GetComponent<SpriteRenderer>().sprite = rightshoesOption[(int)PhotonNetwork.LocalPlayer.CustomProperties["rightShoesUI"]];

        
    }
}
