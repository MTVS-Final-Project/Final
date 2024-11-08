using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.IO;
using Photon.Realtime;
public class PhotonNet : MonoBehaviourPunCallbacks
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


    public Transform catTransform; // �����Ϳ��� ����� ��ü�� Transform�� �����մϴ�.
    public Transform circleTransform;
    void Start()
    {
        // RPC ������ �� ����
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
        if (catTransform != null)
        {
            // ����� ��ġ ��ó�� ��ġ ���
            Vector3 spawnPosition = GetRandomPositionNearCat();

            // ���� ��ġ Ȯ�� �α�
            Debug.Log("Spawn Position: " + spawnPosition);

            // ��ü ����
            GameObject playerInstance = PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);

            ddddd(playerInstance);
            // CatController�� player ������ �ڵ� �Ҵ�
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

    // ����� ��ġ ��ó�� ���� ��ġ�� ��ȯ�ϴ� �޼���
    Vector3 GetRandomPositionNearCat()
    {

        // ������� �������� ����Ͽ� ������ ���� ����
        float offsetX = Random.Range(-0.5f, 0.5f); // X�� ������ ���� ���
        float offsetY = Random.Range(-0.5f, 0.5f); // Y�� ������ ���� ���
        float offsetZ = 0; // 2D�� ��� Z���� 0���� ����

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
        // Update ���� �߰� dddd

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

        // ���Ǵ� ���// �װſ� ���缭 ���Ǹ� �ٲ۴�
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
