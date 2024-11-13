using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PhotonNet : MonoBehaviourPunCallbacks, IPunObservable
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

            CustomMizeGive(playerInstance);
            // CatController의 player 변수에 자동 할당
            CatController.instance.player = playerInstance;

        }
        else if (circleTransform != null)
        {
            Vector3 spawnPos = GetRandomPositionNearCircle();
            Debug.Log("spawn pos : " + spawnPos);
            GameObject playerInstance = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
            CustomMizeGive(playerInstance);
            Debug.Log("player : " + playerInstance);
        }
    }

    // 고양이 위치 근처의 랜덤 위치를 반환하는 메서드
    Vector3 GetRandomPositionNearCat()
    {

        // 고양이의 스케일을 고려하여 오프셋 범위 조정
        float offsetX = Random.Range(-0.1f, 0.1f); // X축 오프셋 범위 축소
        float offsetY = Random.Range(-0.1f, 0.1f); // Y축 오프셋 범위 축소
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
    void CustomMizeGive(GameObject player)
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("BodyUI", out object bodyIndex))
            player.transform.Find("Body1").GetComponent<SpriteRenderer>().sprite = bodyOption[(int)bodyIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("EyesUI", out object eyeIndex))
            player.transform.Find("eye").GetComponent<SpriteRenderer>().sprite = eyeOption[(int)eyeIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("MouthUI", out object mouthIndex))
            player.transform.Find("mouth").GetComponent<SpriteRenderer>().sprite = mouthOption[(int)mouthIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("LeftArmUI", out object leftArmIndex))
            player.transform.Find("leftArm1").GetComponent<SpriteRenderer>().sprite = leftarmOption[(int)leftArmIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("rightArmUI", out object rightArmIndex))
            player.transform.Find("rightArmWhite").GetComponent<SpriteRenderer>().sprite = rightarmOption[(int)rightArmIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("leftLegUI", out object leftLegIndex))
            player.transform.Find("leftlegblack").GetComponent<SpriteRenderer>().sprite = leftlegOption[(int)leftLegIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("rightLegUI", out object rightLegIndex))
            player.transform.Find("rightlegblack").GetComponent<SpriteRenderer>().sprite = rightlegOption[(int)rightLegIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PantsUI", out object pantIndex))
            player.transform.Find("blackPant").GetComponent<SpriteRenderer>().sprite = pantOption[(int)pantIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("HairUI", out object hairIndex))
            player.transform.Find("hairblack").GetComponent<SpriteRenderer>().sprite = hairOption[(int)hairIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("leftShoesUI", out object leftShoesIndex))
            player.transform.Find("leftshoesblack").GetComponent<SpriteRenderer>().sprite = leftshoesOption[(int)leftShoesIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("rightShoesUI", out object rightShoesIndex))
            player.transform.Find("rightshoesblack").GetComponent<SpriteRenderer>().sprite = rightshoesOption[(int)rightShoesIndex];
    }

    // Synchronize customization properties over the network
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send customization properties to other players
            stream.SendNext((int)PhotonNetwork.LocalPlayer.CustomProperties["BodyUI"]);
            stream.SendNext((int)PhotonNetwork.LocalPlayer.CustomProperties["EyesUI"]);
            stream.SendNext((int)PhotonNetwork.LocalPlayer.CustomProperties["MouthUI"]);
            stream.SendNext((int)PhotonNetwork.LocalPlayer.CustomProperties["LeftArmUI"]);
            stream.SendNext((int)PhotonNetwork.LocalPlayer.CustomProperties["rightArmUI"]);
            stream.SendNext((int)PhotonNetwork.LocalPlayer.CustomProperties["leftLegUI"]);
            stream.SendNext((int)PhotonNetwork.LocalPlayer.CustomProperties["rightLegUI"]);
            stream.SendNext((int)PhotonNetwork.LocalPlayer.CustomProperties["PantsUI"]);
            stream.SendNext((int)PhotonNetwork.LocalPlayer.CustomProperties["HairUI"]);
            stream.SendNext((int)PhotonNetwork.LocalPlayer.CustomProperties["leftShoesUI"]);
            stream.SendNext((int)PhotonNetwork.LocalPlayer.CustomProperties["rightShoesUI"]);
        }
        else if(stream.IsReading)
        {
            // Receive customization properties from other players
            PhotonNetwork.LocalPlayer.CustomProperties["BodyUI"] = (int)stream.ReceiveNext();
            PhotonNetwork.LocalPlayer.CustomProperties["EyesUI"] = (int)stream.ReceiveNext();
            PhotonNetwork.LocalPlayer.CustomProperties["MouthUI"] = (int)stream.ReceiveNext();
            PhotonNetwork.LocalPlayer.CustomProperties["LeftArmUI"] = (int)stream.ReceiveNext();
            PhotonNetwork.LocalPlayer.CustomProperties["rightArmUI"] = (int)stream.ReceiveNext();
            PhotonNetwork.LocalPlayer.CustomProperties["leftLegUI"] = (int)stream.ReceiveNext();
            PhotonNetwork.LocalPlayer.CustomProperties["rightLegUI"] = (int)stream.ReceiveNext();
            PhotonNetwork.LocalPlayer.CustomProperties["PantsUI"] = (int)stream.ReceiveNext();
            PhotonNetwork.LocalPlayer.CustomProperties["HairUI"] = (int)stream.ReceiveNext();
            PhotonNetwork.LocalPlayer.CustomProperties["leftShoesUI"] = (int)stream.ReceiveNext();
            PhotonNetwork.LocalPlayer.CustomProperties["rightShoesUI"] = (int)stream.ReceiveNext();

            // Apply received customization to update the player's appearance
            CustomMizeGive(this.gameObject);
        }
    }
}