using UnityEngine;
using Photon.Pun;

public class PhotonNet : MonoBehaviourPunCallbacks
{
    public Transform catTransform; // �����Ϳ��� ����� ��ü�� Transform�� �����մϴ�.

    void Start()
    {
        // RPC ������ �� ����
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;

        // ����� ��ġ ��ó�� ��ġ ���
        Vector3 spawnPosition = GetRandomPositionNearCat();

        // ���� ��ġ Ȯ�� �α�
        Debug.Log("Spawn Position: " + spawnPosition);

        // ��ü ����
        GameObject playerInstance =  PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);

        // CatController�� player ������ �ڵ� �Ҵ�
        CatController.instance.player = playerInstance;
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

    void Update()
    {
        // Update ���� �߰� ����
    }
}
