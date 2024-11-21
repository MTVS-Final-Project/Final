// PhotonNet.cs
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Net;
using ExitGames.Client.Photon;

public class PhotonNet : MonoBehaviourPunCallbacks//, IPunObservable
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

    // 현재 커스터마이징 값을 저장할 변수들
    private int currentBodyIndex;
    private int currentEyeIndex;
    private int currentMouthIndex;
    private int currentLeftArmIndex;
    private int currentRightArmIndex;
    private int currentLeftLegIndex;
    private int currentRightLegIndex;
    private int currentPantIndex;
    private int currentHairIndex;
    private int currentLeftShoesIndex;
    private int currentRightShoesIndex;
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
                CustomMizeGive(playerInstance);  // 내 캐릭터에 커스터마이징 적용
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
                CustomMizeGive(playerInstance);  // 내 캐릭터에 커스터마이징 적용
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
        // Body (몸통)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("BodyUI", out object bodyIndex))
            player.transform.Find("Body1").GetComponent<SpriteRenderer>().sprite = bodyOption[(int)bodyIndex];

        // Eyes (눈)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("EyesUI", out object eyeIndex))
            player.transform.Find("eye").GetComponent<SpriteRenderer>().sprite = eyeOption[(int)eyeIndex];

        // Mouth (입)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("MouthUI", out object mouthIndex))
            player.transform.Find("mouth").GetComponent<SpriteRenderer>().sprite = mouthOption[(int)mouthIndex];

        // Left Arm (왼쪽 팔)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("LeftArmUI", out object leftArmIndex))
            player.transform.Find("leftArm").GetComponent<SpriteRenderer>().sprite = leftarmOption[(int)leftArmIndex];

        // Right Arm (오른쪽 팔)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("rightArmUI", out object rightArmIndex))
            player.transform.Find("rightArm").GetComponent<SpriteRenderer>().sprite = rightarmOption[(int)rightArmIndex];

        // Left Leg (왼쪽 다리)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("leftLegUI", out object leftLegIndex))
            player.transform.Find("leftLeg").GetComponent<SpriteRenderer>().sprite = leftlegOption[(int)leftLegIndex];

        // Right Leg (오른쪽 다리)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("rightLegUI", out object rightLegIndex))
            player.transform.Find("rightLeg").GetComponent<SpriteRenderer>().sprite = rightlegOption[(int)rightLegIndex];

        // Pants (팬츠)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PantsUI", out object pantIndex))
            player.transform.Find("pants").GetComponent<SpriteRenderer>().sprite = pantOption[(int)pantIndex];

        // Hair (헤어)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("HairUI", out object hairIndex))
            player.transform.Find("hair").GetComponent<SpriteRenderer>().sprite = hairOption[(int)hairIndex];

        // Left Shoe (왼쪽 신발)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("leftShoesUI", out object leftShoesIndex))
            player.transform.Find("leftShoe").GetComponent<SpriteRenderer>().sprite = leftshoesOption[(int)leftShoesIndex];

        // Right Shoe (오른쪽 신발)
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
        // 현재 값 업데이트
        currentBodyIndex = bodyIndex;
        currentEyeIndex = eyeIndex;
        currentMouthIndex = mouthIndex;
        currentLeftArmIndex = leftArmIndex;
        currentRightArmIndex = rightArmIndex;
        currentLeftLegIndex = leftLegIndex;
        currentRightLegIndex = rightLegIndex;
        currentPantIndex = pantIndex;
        currentHairIndex = hairIndex;
        currentLeftShoesIndex = leftShoesIndex;
        currentRightShoesIndex = rightShoesIndex;

        // 커스터마이징 데이터를 로컬 플레이어에 저장
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

        // CustomProperties 업데이트
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

        // 다른 클라이언트와 동기화
        photonView.RPC("ApplyCustomization", RpcTarget.Others, bodyIndex, eyeIndex, mouthIndex, leftArmIndex, rightArmIndex,
                       leftLegIndex, rightLegIndex, pantIndex, hairIndex, leftShoesIndex, rightShoesIndex);

        // 즉시 반영
        CustomMizeGive(gameObject);
    }


    [PunRPC]
    public void ApplyCustomization(int bodyIndex, int eyeIndex, int mouthIndex, int leftArmIndex, int rightArmIndex,
                              int leftLegIndex, int rightLegIndex, int pantIndex, int hairIndex,
                              int leftShoesIndex, int rightShoesIndex)
    {
        // 전달받은 데이터로 커스터마이징 적용
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

        // CustomProperties 업데이트
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

        // 캐릭터 반영
        CustomMizeGive(gameObject);
    }

    //// IPunObservable 구현
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        // 데이터 전송
    //        stream.SendNext(currentBodyIndex);
    //        stream.SendNext(currentEyeIndex);
    //        stream.SendNext(currentMouthIndex);
    //        stream.SendNext(currentLeftArmIndex);
    //        stream.SendNext(currentRightArmIndex);
    //        stream.SendNext(currentLeftLegIndex);
    //        stream.SendNext(currentRightLegIndex);
    //        stream.SendNext(currentPantIndex);
    //        stream.SendNext(currentHairIndex);
    //        stream.SendNext(currentLeftShoesIndex);
    //        stream.SendNext(currentRightShoesIndex);
    //    }
    //    else
    //    {
    //        // 데이터 수신
    //        currentBodyIndex = (int)stream.ReceiveNext();
    //        currentEyeIndex = (int)stream.ReceiveNext();
    //        currentMouthIndex = (int)stream.ReceiveNext();
    //        currentLeftArmIndex = (int)stream.ReceiveNext();
    //        currentRightArmIndex = (int)stream.ReceiveNext();
    //        currentLeftLegIndex = (int)stream.ReceiveNext();
    //        currentRightLegIndex = (int)stream.ReceiveNext();
    //        currentPantIndex = (int)stream.ReceiveNext();
    //        currentHairIndex = (int)stream.ReceiveNext();
    //        currentLeftShoesIndex = (int)stream.ReceiveNext();
    //        currentRightShoesIndex = (int)stream.ReceiveNext();

    //        // 받은 데이터로 커스터마이징 업데이트
    //        UpdateCustomization(currentBodyIndex, currentEyeIndex, currentMouthIndex,
    //                          currentLeftArmIndex, currentRightArmIndex, currentLeftLegIndex,
    //                          currentRightLegIndex, currentPantIndex, currentHairIndex,
    //                          currentLeftShoesIndex, currentRightShoesIndex);
    //    }
    //}


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer == PhotonNetwork.LocalPlayer)
        {
            // 내 커스터마이징 정보를 업데이트
            CustomMizeGive(gameObject);
        }
        else
        {
            // 다른 플레이어의 커스터마이징 정보를 업데이트
            GameObject playerObject = FindPlayerObject(targetPlayer.ActorNumber);
            if (playerObject != null)
            {
                ApplyCustomizationFromProperties(playerObject, changedProps);
            }
        }
    }

    private void ApplyCustomizationFromProperties(GameObject playerObject, ExitGames.Client.Photon.Hashtable props)
    {
        PhotonView photonView = playerObject.GetComponent<PhotonView>();
        if (photonView == null) return;

        GameObject playerInstance = GameObject.Find($"Player(Clone)");
        if (playerInstance == null) return;

        // Body (몸통)
        if (props.TryGetValue("BodyUI", out object bodyIndex))
        {
            SpriteRenderer bodyRenderer = playerInstance.transform.Find("Body1")?.GetComponent<SpriteRenderer>();
            if (bodyRenderer != null) bodyRenderer.sprite = bodyOption[(int)bodyIndex];
        }

        // Eyes (눈)
        if (props.TryGetValue("EyesUI", out object eyeIndex))
        {
            SpriteRenderer eyeRenderer = playerInstance.transform.Find("eye")?.GetComponent<SpriteRenderer>();
            if (eyeRenderer != null) eyeRenderer.sprite = eyeOption[(int)eyeIndex];
        }

        // Mouth (입)
        if (props.TryGetValue("MouthUI", out object mouthIndex))
        {
            SpriteRenderer mouthRenderer = playerInstance.transform.Find("mouth")?.GetComponent<SpriteRenderer>();
            if (mouthRenderer != null) mouthRenderer.sprite = mouthOption[(int)mouthIndex];
        }

        // Left Arm (왼쪽 팔)
        if (props.TryGetValue("LeftArmUI", out object leftArmIndex))
        {
            SpriteRenderer leftArmRenderer = playerInstance.transform.Find("leftArm")?.GetComponent<SpriteRenderer>();
            if (leftArmRenderer != null) leftArmRenderer.sprite = leftarmOption[(int)leftArmIndex];
        }

        // Right Arm (오른쪽 팔)
        if (props.TryGetValue("rightArmUI", out object rightArmIndex))
        {
            SpriteRenderer rightArmRenderer = playerInstance.transform.Find("rightArm")?.GetComponent<SpriteRenderer>();
            if (rightArmRenderer != null) rightArmRenderer.sprite = rightarmOption[(int)rightArmIndex];
        }

        // Left Leg (왼쪽 다리)
        if (props.TryGetValue("leftLegUI", out object leftLegIndex))
        {
            SpriteRenderer leftLegRenderer = playerInstance.transform.Find("leftLeg")?.GetComponent<SpriteRenderer>();
            if (leftLegRenderer != null) leftLegRenderer.sprite = leftlegOption[(int)leftLegIndex];
        }

        // Right Leg (오른쪽 다리)
        if (props.TryGetValue("rightLegUI", out object rightLegIndex))
        {
            SpriteRenderer rightLegRenderer = playerInstance.transform.Find("rightLeg")?.GetComponent<SpriteRenderer>();
            if (rightLegRenderer != null) rightLegRenderer.sprite = rightlegOption[(int)rightLegIndex];
        }

        // Pants (팬츠)
        if (props.TryGetValue("PantsUI", out object pantIndex))
        {
            SpriteRenderer pantsRenderer = playerInstance.transform.Find("pants")?.GetComponent<SpriteRenderer>();
            if (pantsRenderer != null) pantsRenderer.sprite = pantOption[(int)pantIndex];
        }

        // Hair (헤어)
        if (props.TryGetValue("HairUI", out object hairIndex))
        {
            SpriteRenderer hairRenderer = playerInstance.transform.Find("hair")?.GetComponent<SpriteRenderer>();
            if (hairRenderer != null) hairRenderer.sprite = hairOption[(int)hairIndex];
        }

        // Left Shoe (왼쪽 신발)
        if (props.TryGetValue("leftShoesUI", out object leftShoesIndex))
        {
            SpriteRenderer leftShoeRenderer = playerInstance.transform.Find("leftShoe")?.GetComponent<SpriteRenderer>();
            if (leftShoeRenderer != null) leftShoeRenderer.sprite = leftshoesOption[(int)leftShoesIndex];
        }

        // Right Shoe (오른쪽 신발)
        if (props.TryGetValue("rightShoesUI", out object rightShoesIndex))
        {
            SpriteRenderer rightShoeRenderer = playerInstance.transform.Find("rightShoe")?.GetComponent<SpriteRenderer>();
            if (rightShoeRenderer != null) rightShoeRenderer.sprite = rightshoesOption[(int)rightShoesIndex];
        }
    }
    private GameObject FindPlayerObject(int actorNumber)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // Player 태그가 있다고 가정
        foreach (GameObject player in players)
        {
            PhotonView view = player.GetComponent<PhotonView>();
            if (view != null && view.Owner != null && view.Owner.ActorNumber == actorNumber)
            {
                return player;
            }
        }
        return null;
    }
}