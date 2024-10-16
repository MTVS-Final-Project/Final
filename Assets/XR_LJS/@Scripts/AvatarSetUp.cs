using Photon.Pun;
using System.Linq;
using UnityEngine;
using System.Collections;

public class AvatarSetUp : MonoBehaviour
{
    [SerializeField] private PhotonView myPV;
    [SerializeField] private AvatarApiClient apiClient;

    [SerializeField] GameObject skin;
    [SerializeField] GameObject[] eyes;
    [HideInInspector] public int eyesCount;
    [SerializeField] GameObject[] feye;
    [SerializeField] GameObject[] meye;
    [HideInInspector] public int bodyCount;
    [SerializeField] GameObject[] fbody;
    [SerializeField] GameObject[] mbody;
    [HideInInspector] public int hairCount;
    [SerializeField] GameObject[] fhair;
    [SerializeField] GameObject[] mhair;
    [HideInInspector] public int pantCount;
    [SerializeField] GameObject[] fpant;
    [SerializeField] GameObject[] mpant;
    [HideInInspector] public int rightArmCount;
    [SerializeField] GameObject[] frightArm;
    [SerializeField] GameObject[] mrightArm;
    [HideInInspector] public int leftArmCount;
    [SerializeField] GameObject[] fleftArm;
    [SerializeField] GameObject[] mleftArm;
    [HideInInspector] public int rightlegCount;
    [SerializeField] GameObject[] frightleg;
    [SerializeField] GameObject[] mrightleg;
    [HideInInspector] public int leftlegCount;
    [SerializeField] GameObject[] fleftleg;
    [SerializeField] GameObject[] mleftleg;
    [HideInInspector] public int rightShoesCount;
    [SerializeField] GameObject[] frightShoes;
    [SerializeField] GameObject[] mrightshoes;
    [HideInInspector] public int leftShoesCount;
    [SerializeField] GameObject[] fleftShoes;
    [SerializeField] GameObject[] mleftShoes;

    [SerializeField] SpriteRenderer[] skinColor;
    [SerializeField] SpriteRenderer[] eyesColor;
    [SerializeField] SpriteRenderer[] pantColor;
    [SerializeField] SpriteRenderer[] rightArmColor;
    [SerializeField] SpriteRenderer[] leftArmColor;
    [SerializeField] SpriteRenderer[] rightLegColor;
    [SerializeField] SpriteRenderer[] leftLegColor;
    [SerializeField] SpriteRenderer[] rightshoesColor;
    [SerializeField] SpriteRenderer[] leftShoesColor;

    void Start()
    {
        bodyCount = fbody.Length;
        eyesCount = feye.Length;
        hairCount = fhair.Length;
        pantCount = fpant.Length;
        rightArmCount = frightArm.Length;
        leftArmCount = fleftArm.Length;
        rightlegCount = frightleg.Length;
        leftlegCount = fleftleg.Length;
        rightShoesCount = frightShoes.Length;
        leftShoesCount = fleftShoes.Length;

        // ������� �ƹ�Ÿ ������ �ε�
        //StartCoroutine(LoadAvatarData());
    }

    public void SetAvatar(AvatarData avatarData)
    {
        SetActiveGameObject(fbody, mbody, avatarData.bodyId);
        SetActiveGameObject(feye, meye, avatarData.eyesId);
        SetActiveGameObject(fhair, mhair, avatarData.hairId);
        SetActiveGameObject(fpant, mpant, avatarData.pantId);
        SetActiveGameObject(frightArm, mrightArm, avatarData.rightArmId);
        SetActiveGameObject(fleftArm, mleftArm, avatarData.leftArmId);
        SetActiveGameObject(frightleg, mrightleg, avatarData.rightLegId);
        SetActiveGameObject(fleftleg, mleftleg, avatarData.leftLegId);
        SetActiveGameObject(fleftShoes, mleftShoes, avatarData.leftShoesId);
        SetActiveGameObject(frightShoes, mrightshoes, avatarData.rightShoesId);

        SetColor(skinColor, avatarData.skinColorHex);
        SetColor(eyesColor, avatarData.eyesColorHex);
        SetColor(pantColor, avatarData.pantColorHex);
        SetColor(rightArmColor, avatarData.rightArmColorHex);
        SetColor(leftArmColor, avatarData.leftArmColorHex);
        SetColor(rightLegColor, avatarData.rightLegColorHex);
        SetColor(leftLegColor, avatarData.leftLegColorHex);
        SetColor(rightshoesColor, avatarData.rightShoesColorHex);
        SetColor(leftShoesColor, avatarData.leftShoesColorHex);
    }

    private void SetActiveGameObject(GameObject[] female, GameObject[] male, int index)
    {
        for (int i = 0; i < female.Length; i++)
        {
            female[i].SetActive(i == index);
            male[i].SetActive(i == index);
        }
    }

    private void SetColor(SpriteRenderer[] renderers, string colorHex)
    {
        Color color = AvatarData.HexToColor(colorHex);
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.color = color;
        }
    }

    public AvatarData GetCurrentAvatarData()
    {
        return new AvatarData
        {
            bodyId = GetActiveIndex(fbody),
            eyesId = GetActiveIndex(feye),
            hairId = GetActiveIndex(fhair),
            pantId = GetActiveIndex(fpant),
            rightArmId = GetActiveIndex(frightArm),
            leftArmId = GetActiveIndex(fleftArm),
            rightLegId = GetActiveIndex(frightleg),
            leftLegId = GetActiveIndex(fleftleg),
            rightShoesId = GetActiveIndex(frightShoes),
            leftShoesId = GetActiveIndex(fleftShoes),
            skinColorHex = AvatarData.ColorToHex(skinColor[0].color),
            eyesColorHex = AvatarData.ColorToHex(eyesColor[0].color),
            pantColorHex = AvatarData.ColorToHex(pantColor[0].color),
            rightArmColorHex = AvatarData.ColorToHex(rightArmColor[0].color),
            leftArmColorHex = AvatarData.ColorToHex(leftArmColor[0].color),
            rightLegColorHex = AvatarData.ColorToHex(rightLegColor[0].color),
            leftLegColorHex = AvatarData.ColorToHex(leftLegColor[0].color),
            rightShoesColorHex = AvatarData.ColorToHex(rightshoesColor[0].color),
            leftShoesColorHex = AvatarData.ColorToHex(leftShoesColor[0].color)
        };
    }

    private int GetActiveIndex(GameObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].activeSelf)
            {
                return i;
            }
        }
        return 0; // �⺻��
    }

    // �ƹ�Ÿ �����͸� ������ �����ϴ� �޼���
    public void SaveAvatarData()
    {
        AvatarData data = GetCurrentAvatarData();
        string json = JsonUtility.ToJson(data);
        string userId = PhotonNetwork.LocalPlayer.UserId; // Photon���� ����� ID ��������

        //StartCoroutine(apiClient.SaveAvatarData(userId, json, (success) => {
        //    if (success)
        //    {
        //        Debug.Log("Avatar data saved successfully");
        //        SyncAvatarToOthers(); // ���� ���� �� �ٸ� �÷��̾��� ����ȭ
        //    }
        //    else
        //    {
        //        Debug.LogError("Failed to save avatar data");
        //    }
        //}));
    }

    // �������� �ƹ�Ÿ �����͸� �޾� �����ϴ� �޼���
    //private IEnumerator LoadAvatarData()
    //{
    //    string userId = PhotonNetwork.LocalPlayer.UserId; // Photon���� ����� ID ��������

        //yield return StartCoroutine(apiClient.LoadAvatarData(userId, (json) => {
        //    if (json != null)
        //    {
        //        AvatarData data = JsonUtility.FromJson<AvatarData>(json);
        //        SetAvatar(data);
        //        Debug.Log("Avatar data loaded and applied successfully");
        //    }
        //    else
        //    {
        //        Debug.LogError("Failed to load avatar data");
        //    }
        //}));
    }

    // Photon�� ���� �ٸ� �÷��̾�� �ƹ�Ÿ ������ ����
    //[PunRPC]
    //private void SyncAvatarData(string avatarJson)
    //{
    //    AvatarData data = JsonUtility.FromJson<AvatarData>(avatarJson);
    //    SetAvatar(data);
    //}

    //// ���� �÷��̾��� �ƹ�Ÿ ��������� �ٸ� �÷��̾�鿡�� ����ȭ
    //public void SyncAvatarToOthers()
    //{
    //    if (myPV.IsMine)
    //    {
    //        string avatarJson = JsonUtility.ToJson(GetCurrentAvatarData());
    //        myPV.RPC("SyncAvatarData", RpcTarget.Others, avatarJson);
    //    }
    //}

    //// �ƹ�Ÿ ���� ���� �޼��� (��: �Ӹ� ����)
    //public void ChangeHair(int index)
    //{
    //    SetActiveGameObject(fhair, mhair, index);
    //    SaveAvatarData(); // ������� ���� �� ����ȭ
    //}

    //// �ƹ�Ÿ ���� ���� �޼��� (��: �Ǻλ� ����)
    //public void ChangeSkinColor(Color color)
    //{
    //    SetColor(skinColor, AvatarData.ColorToHex(color));
    //    SaveAvatarData(); // ������� ���� �� ����ȭ
    //}

    //// ��Ÿ �ƹ�Ÿ ���� �� ���� ���� �޼����...
