using UnityEngine;
using Photon.Pun;

public class PhotonNet : MonoBehaviourPunCallbacks
{
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

            // CatController�� player ������ �ڵ� �Ҵ�
            CatController.instance.player = playerInstance;
        }
        else if (circleTransform != null)
        {
            Vector3 spawnPos = GetRandomPositionNearCircle();
            Debug.Log("spawn pos : " + spawnPos);
            GameObject playerInstance = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
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

    void Update()
    {
        // Update ���� �߰� ����
    }
}
