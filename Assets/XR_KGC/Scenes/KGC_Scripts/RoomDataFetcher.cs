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
    private string baseUrl = "http://13.124.6.53:8080/api/v1/room"; // 서버 URL

    private void Start()
    {
        FetchRoomData("7");
    }

    // 데이터를 가져오는 함수
    public void FetchRoomData(string roomId)
    {
        StartCoroutine(GetRoomDataCoroutine(roomId));
    }

    private IEnumerator GetRoomDataCoroutine(string roomId)
    {
        string url = $"{baseUrl}?roomId={roomId}";// Room ID를 URL에 추가
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // 요청 헤더 설정
            request.SetRequestHeader("Accept", "*/*");

            // 요청 보내기
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}");
            }
            else
            {
                // 응답 데이터 파싱
                string jsonResponse = request.downloadHandler.text;
                Debug.Log($"Response JSON: {jsonResponse}");

                RoomData roomData = JsonUtility.FromJson<RoomData>(jsonResponse);

                // TempManager의 furnitureList에 데이터를 추가
                TempManager.Instance.furnitureList.Clear(); // 기존 데이터를 초기화
                foreach (var furniture in roomData.furnitureList)
                {
                    // GaguData 생성자를 사용하여 객체 생성
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
