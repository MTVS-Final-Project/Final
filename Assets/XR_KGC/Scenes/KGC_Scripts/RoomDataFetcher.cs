using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

[Serializable]
public class Furniture
{
    public int size;
    public int spriteNumber;
    public float positionX;
    public float positionY;
    public float rotationY;
}

[Serializable]
public class RoomData
{
    public string id;
    public int ownerId;
    public List<Furniture> furnitureList;
    public string createdAt;
}

public class RoomDataFetcher : MonoBehaviour
{
    private string baseUrl = "http://13.124.6.53:8080/api/v1/room"; // ���� URL

    private void Start()
    {
        FetchRoomData("7");
    }

    // �����͸� �������� �Լ�
    public void FetchRoomData(string roomId)
    {
        StartCoroutine(GetRoomDataCoroutine(roomId));
    }

    private IEnumerator GetRoomDataCoroutine(string roomId)
    {
        string url = $"{baseUrl}?roomId={roomId}";// Room ID�� URL�� �߰�
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // ��û ��� ����
            request.SetRequestHeader("Accept", "*/*");

            // ��û ������
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}");
            }
            else
            {
                // ���� ������ �Ľ�
                string jsonResponse = request.downloadHandler.text;
                Debug.Log($"Response JSON: {jsonResponse}");

                RoomData roomData = JsonUtility.FromJson<RoomData>(jsonResponse);

                // TempManager�� furnitureList�� �����͸� �߰�
                TempManager.Instance.furnitureList.Clear(); // ���� �����͸� �ʱ�ȭ
                foreach (var furniture in roomData.furnitureList)
                {
                    // GaguData �����ڸ� ����Ͽ� ��ü ����
                    GaguSave.GaguData gaguData = new GaguSave.GaguData(
                        furniture.size,                     // size
                        furniture.spriteNumber,             // spriteNum
                        furniture.positionX,                // xpos
                        furniture.positionY,                // ypos
                        furniture.rotationY                 // rotY
                    );

                    TempManager.Instance.furnitureList.Add(gaguData);
                }

                Debug.Log("Furniture data updated in TempManager.");
            }
        }
    }

}
