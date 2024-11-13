using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PhotonNet : MonoBehaviourPunCallbacks, IPunObservable
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

            CustomMizeGive(playerInstance);
            // CatController�� player ������ �ڵ� �Ҵ�
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

    // ����� ��ġ ��ó�� ���� ��ġ�� ��ȯ�ϴ� �޼���
    Vector3 GetRandomPositionNearCat()
    {

        // ������� �������� ����Ͽ� ������ ���� ����
        float offsetX = Random.Range(-0.1f, 0.1f); // X�� ������ ���� ���
        float offsetY = Random.Range(-0.1f, 0.1f); // Y�� ������ ���� ���
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