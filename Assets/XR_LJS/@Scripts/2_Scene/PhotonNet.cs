// PhotonNet.cs
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Net;

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

    public Transform catTransform;
    public Transform circleTransform;
    private Dictionary<string, int> customizationData;

    void Start()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;

        if (catTransform != null)
        {
            Vector3 spawnPosition = GetRandomPositionNearCat();
            GameObject playerInstance = PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);
            if (playerInstance.GetComponent<PhotonView>().IsMine)
            {
                LoadCharacterData();
                CustomMizeGive(playerInstance);
            }
            CatController.instance.player = playerInstance;
        }
        else if (circleTransform != null)
        {
            Vector3 spawnPos = GetRandomPositionNearCircle();
            GameObject playerInstance = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
            if (playerInstance.GetComponent<PhotonView>().IsMine)
            {
                LoadCharacterData();
                CustomMizeGive(playerInstance);
            }
        }
    }
        Vector3 GetRandomPositionNearCat()
    {
        float offsetX = Random.Range(-0.1f, 0.1f);
        float offsetY = Random.Range(-0.1f, 0.1f);
        return catTransform != null ? catTransform.position + new Vector3(offsetX, offsetY, 0) : Vector3.zero;
    }

    Vector3 GetRandomPositionNearCircle()
    {
        float offX = Random.Range(-0.1f, 0.1f);
        float offY = Random.Range(-0.1f, 0.1f);
        return circleTransform != null ? circleTransform.position + new Vector3(offX, offY, 0) : Vector3.zero;
    }

    public void CustomMizeGive(GameObject player)
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("BodyUI", out object bodyIndex))
            player.transform.Find("Body1").GetComponent<SpriteRenderer>().sprite = bodyOption[(int)bodyIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("EyesUI", out object eyeIndex))
            player.transform.Find("eye").GetComponent<SpriteRenderer>().sprite = eyeOption[(int)eyeIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("MouthUI", out object mouthIndex))
            player.transform.Find("mouth").GetComponent<SpriteRenderer>().sprite = mouthOption[(int)mouthIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("LeftArmUI", out object leftArmIndex))
            player.transform.Find("leftArm").GetComponent<SpriteRenderer>().sprite = leftarmOption[(int)leftArmIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("rightArmUI", out object rightArmIndex))
            player.transform.Find("rightArm").GetComponent<SpriteRenderer>().sprite = rightarmOption[(int)rightArmIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("leftLegUI", out object leftLegIndex))
            player.transform.Find("leftLeg").GetComponent<SpriteRenderer>().sprite = leftlegOption[(int)leftLegIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("rightLegUI", out object rightLegIndex))
            player.transform.Find("rightLeg").GetComponent<SpriteRenderer>().sprite = rightlegOption[(int)rightLegIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PantsUI", out object pantIndex))
            player.transform.Find("pants").GetComponent<SpriteRenderer>().sprite = pantOption[(int)pantIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("HairUI", out object hairIndex))
            player.transform.Find("hair").GetComponent<SpriteRenderer>().sprite = hairOption[(int)hairIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("leftShoesUI", out object leftShoesIndex))
            player.transform.Find("leftShoe").GetComponent<SpriteRenderer>().sprite = leftshoesOption[(int)leftShoesIndex];

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("rightShoesUI", out object rightShoesIndex))
            player.transform.Find("rightShoe").GetComponent<SpriteRenderer>().sprite = rightshoesOption[(int)rightShoesIndex];
    }

    private int GetUserId()
    {
        // PhotonNetwork.LocalPlayer.UserId는 string이므로 비교합니다.
        // UserId가 "1"이면 1을 반환, 그렇지 않으면 2를 반환
        // 첫 번째 플레이어는 ActorNumber가 1, 두 번째는 ActorNumber가 2로 설정
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            return 1; // 첫 번째 플레이어는 1
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            return 2; // 두 번째 플레이어는 2
        }
        // 기본적으로 2를 반환하거나 에러 처리 로직을 추가할 수 있습니다.
        return 2;
    }
    private void LoadCharacterData()
    {
        int userId = GetUserId();
        var customProperties = PhotonNetwork.LocalPlayer.CustomProperties;
        var customizationData = APIManager.Instance.ExtractCustomizationFromPhoton(customProperties);
        StartCoroutine(APIManager.Instance.UpdateCharacterData(userId, customizationData));
    }

    [PunRPC]
    public void UpdateCustomization(int bodyIndex, int eyeIndex, int mouthIndex, int leftArmIndex, int rightArmIndex,
                                int leftLegIndex, int rightLegIndex, int pantIndex, int hairIndex,
                                int leftShoesIndex, int rightShoesIndex)
    {
        var properties = new ExitGames.Client.Photon.Hashtable()
    {
        { "BodyUI", bodyIndex },
        { "EyesUI", eyeIndex },
        { "MouthUI", mouthIndex },
        { "LeftArmUI", leftArmIndex },
        { "rightArmUI", rightArmIndex },
        { "leftLegUI", leftLegIndex },
        { "rightLegUI", rightLegIndex },
        { "PantsUI", pantIndex },
        { "HairUI", hairIndex },
        { "leftShoesUI", leftShoesIndex },
        { "rightShoesUI", rightShoesIndex }
    };

        // Custom Properties 업데이트
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

        // 커스터마이징을 모든 클라이언트에 동기화
        if (photonView.IsMine)
        {
            var customizationData = new APIManager.CustomizationData
            {
                skin = bodyIndex,
                eye = eyeIndex,
                mouth = mouthIndex,
                leftArm = leftArmIndex,
                rightArm = rightArmIndex,
                leftLeg = leftLegIndex,
                rightLeg = rightLegIndex,
                pants = pantIndex,
                hair = hairIndex,
                leftShoe = leftShoesIndex,
                rightShoe = rightShoesIndex
            };

            int userId = GetUserId();
            StartCoroutine(APIManager.Instance.UpdateCharacterData(userId, customizationData));
        }

        // 동기화 후 모든 클라이언트에 반영
        CustomMizeGive(gameObject);
    }

}