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
    // �� �κ�
    public SpriteRenderer bodyPart;
    public Sprite[] bodyOption;

    // ��
    public SpriteRenderer eyePart;
    public Sprite[] eyeOption;

    // ��
    public SpriteRenderer mouthPart;
    public Sprite[] mouthOption;

    // ���� ��
    public SpriteRenderer leftarmPart;
    public Sprite[] leftarmOption;

    //������ ��
    public SpriteRenderer rightarmPart;
    public Sprite[] rightarmOption;

    // ����
    public SpriteRenderer pantPart;
    public Sprite[] pantOption;

    // ���
    public SpriteRenderer hairPart;
    public Sprite[] hairOption;

    //���� �Ź�
    public SpriteRenderer leftshoesPart;
    public Sprite[] leftshoesOption;

    // ������ �Ź�
    public SpriteRenderer rightshoesPart;
    public Sprite[] rightshoesOption;

    // ���� �ٸ�
    public SpriteRenderer leftlegPart;
    public Sprite[] leftlegOption;

    // ������ �ٸ�
    public SpriteRenderer rightlegPart;
    public Sprite[] rightlegOption;

    public Transform catTransform;
    public Transform circleTransform;
    private Dictionary<string, int> customizationData;

    // ���� Ŀ���͸���¡ ���� ������ ������
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
                CustomMizeGive(playerInstance);  // �� ĳ���Ϳ� Ŀ���͸���¡ ����
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
                CustomMizeGive(playerInstance);  // �� ĳ���Ϳ� Ŀ���͸���¡ ����
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
        // Body (����)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("BodyUI", out object bodyIndex))
            player.transform.Find("Body1").GetComponent<SpriteRenderer>().sprite = bodyOption[(int)bodyIndex];

        // Eyes (��)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("EyesUI", out object eyeIndex))
            player.transform.Find("eye").GetComponent<SpriteRenderer>().sprite = eyeOption[(int)eyeIndex];

        // Mouth (��)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("MouthUI", out object mouthIndex))
            player.transform.Find("mouth").GetComponent<SpriteRenderer>().sprite = mouthOption[(int)mouthIndex];

        // Left Arm (���� ��)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("LeftArmUI", out object leftArmIndex))
            player.transform.Find("leftArm").GetComponent<SpriteRenderer>().sprite = leftarmOption[(int)leftArmIndex];

        // Right Arm (������ ��)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("rightArmUI", out object rightArmIndex))
            player.transform.Find("rightArm").GetComponent<SpriteRenderer>().sprite = rightarmOption[(int)rightArmIndex];

        // Left Leg (���� �ٸ�)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("leftLegUI", out object leftLegIndex))
            player.transform.Find("leftLeg").GetComponent<SpriteRenderer>().sprite = leftlegOption[(int)leftLegIndex];

        // Right Leg (������ �ٸ�)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("rightLegUI", out object rightLegIndex))
            player.transform.Find("rightLeg").GetComponent<SpriteRenderer>().sprite = rightlegOption[(int)rightLegIndex];

        // Pants (����)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PantsUI", out object pantIndex))
            player.transform.Find("pants").GetComponent<SpriteRenderer>().sprite = pantOption[(int)pantIndex];

        // Hair (���)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("HairUI", out object hairIndex))
            player.transform.Find("hair").GetComponent<SpriteRenderer>().sprite = hairOption[(int)hairIndex];

        // Left Shoe (���� �Ź�)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("leftShoesUI", out object leftShoesIndex))
            player.transform.Find("leftShoe").GetComponent<SpriteRenderer>().sprite = leftshoesOption[(int)leftShoesIndex];

        // Right Shoe (������ �Ź�)
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("rightShoesUI", out object rightShoesIndex))
            player.transform.Find("rightShoe").GetComponent<SpriteRenderer>().sprite = rightshoesOption[(int)rightShoesIndex];
    }
    private int GetUserId()
    {
        // PhotonNetwork.LocalPlayer.UserId�� string�̹Ƿ� ���մϴ�.
        // UserId�� "1"�̸� 1�� ��ȯ, �׷��� ������ 2�� ��ȯ
        // ù ��° �÷��̾�� ActorNumber�� 1, �� ��°�� ActorNumber�� 2�� ����
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            return 1; // ù ��° �÷��̾�� 1
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            return 2; // �� ��° �÷��̾�� 2
        }
        // �⺻������ 2�� ��ȯ�ϰų� ���� ó�� ������ �߰��� �� �ֽ��ϴ�.
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
        // ���� �� ������Ʈ
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

        // Ŀ���͸���¡ �����͸� ���� �÷��̾ ����
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

        // CustomProperties ������Ʈ
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

        // �ٸ� Ŭ���̾�Ʈ�� ����ȭ
        photonView.RPC("ApplyCustomization", RpcTarget.Others, bodyIndex, eyeIndex, mouthIndex, leftArmIndex, rightArmIndex,
                       leftLegIndex, rightLegIndex, pantIndex, hairIndex, leftShoesIndex, rightShoesIndex);

        // ��� �ݿ�
        CustomMizeGive(gameObject);
    }


    [PunRPC]
    public void ApplyCustomization(int bodyIndex, int eyeIndex, int mouthIndex, int leftArmIndex, int rightArmIndex,
                              int leftLegIndex, int rightLegIndex, int pantIndex, int hairIndex,
                              int leftShoesIndex, int rightShoesIndex)
    {
        // ���޹��� �����ͷ� Ŀ���͸���¡ ����
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

        // CustomProperties ������Ʈ
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

        // ĳ���� �ݿ�
        CustomMizeGive(gameObject);
    }

    //// IPunObservable ����
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        // ������ ����
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
    //        // ������ ����
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

    //        // ���� �����ͷ� Ŀ���͸���¡ ������Ʈ
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
            // �� Ŀ���͸���¡ ������ ������Ʈ
            CustomMizeGive(gameObject);
        }
        else
        {
            // �ٸ� �÷��̾��� Ŀ���͸���¡ ������ ������Ʈ
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

        // Body (����)
        if (props.TryGetValue("BodyUI", out object bodyIndex))
        {
            SpriteRenderer bodyRenderer = playerInstance.transform.Find("Body1")?.GetComponent<SpriteRenderer>();
            if (bodyRenderer != null) bodyRenderer.sprite = bodyOption[(int)bodyIndex];
        }

        // Eyes (��)
        if (props.TryGetValue("EyesUI", out object eyeIndex))
        {
            SpriteRenderer eyeRenderer = playerInstance.transform.Find("eye")?.GetComponent<SpriteRenderer>();
            if (eyeRenderer != null) eyeRenderer.sprite = eyeOption[(int)eyeIndex];
        }

        // Mouth (��)
        if (props.TryGetValue("MouthUI", out object mouthIndex))
        {
            SpriteRenderer mouthRenderer = playerInstance.transform.Find("mouth")?.GetComponent<SpriteRenderer>();
            if (mouthRenderer != null) mouthRenderer.sprite = mouthOption[(int)mouthIndex];
        }

        // Left Arm (���� ��)
        if (props.TryGetValue("LeftArmUI", out object leftArmIndex))
        {
            SpriteRenderer leftArmRenderer = playerInstance.transform.Find("leftArm")?.GetComponent<SpriteRenderer>();
            if (leftArmRenderer != null) leftArmRenderer.sprite = leftarmOption[(int)leftArmIndex];
        }

        // Right Arm (������ ��)
        if (props.TryGetValue("rightArmUI", out object rightArmIndex))
        {
            SpriteRenderer rightArmRenderer = playerInstance.transform.Find("rightArm")?.GetComponent<SpriteRenderer>();
            if (rightArmRenderer != null) rightArmRenderer.sprite = rightarmOption[(int)rightArmIndex];
        }

        // Left Leg (���� �ٸ�)
        if (props.TryGetValue("leftLegUI", out object leftLegIndex))
        {
            SpriteRenderer leftLegRenderer = playerInstance.transform.Find("leftLeg")?.GetComponent<SpriteRenderer>();
            if (leftLegRenderer != null) leftLegRenderer.sprite = leftlegOption[(int)leftLegIndex];
        }

        // Right Leg (������ �ٸ�)
        if (props.TryGetValue("rightLegUI", out object rightLegIndex))
        {
            SpriteRenderer rightLegRenderer = playerInstance.transform.Find("rightLeg")?.GetComponent<SpriteRenderer>();
            if (rightLegRenderer != null) rightLegRenderer.sprite = rightlegOption[(int)rightLegIndex];
        }

        // Pants (����)
        if (props.TryGetValue("PantsUI", out object pantIndex))
        {
            SpriteRenderer pantsRenderer = playerInstance.transform.Find("pants")?.GetComponent<SpriteRenderer>();
            if (pantsRenderer != null) pantsRenderer.sprite = pantOption[(int)pantIndex];
        }

        // Hair (���)
        if (props.TryGetValue("HairUI", out object hairIndex))
        {
            SpriteRenderer hairRenderer = playerInstance.transform.Find("hair")?.GetComponent<SpriteRenderer>();
            if (hairRenderer != null) hairRenderer.sprite = hairOption[(int)hairIndex];
        }

        // Left Shoe (���� �Ź�)
        if (props.TryGetValue("leftShoesUI", out object leftShoesIndex))
        {
            SpriteRenderer leftShoeRenderer = playerInstance.transform.Find("leftShoe")?.GetComponent<SpriteRenderer>();
            if (leftShoeRenderer != null) leftShoeRenderer.sprite = leftshoesOption[(int)leftShoesIndex];
        }

        // Right Shoe (������ �Ź�)
        if (props.TryGetValue("rightShoesUI", out object rightShoesIndex))
        {
            SpriteRenderer rightShoeRenderer = playerInstance.transform.Find("rightShoe")?.GetComponent<SpriteRenderer>();
            if (rightShoeRenderer != null) rightShoeRenderer.sprite = rightshoesOption[(int)rightShoesIndex];
        }
    }
    private GameObject FindPlayerObject(int actorNumber)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // Player �±װ� �ִٰ� ����
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