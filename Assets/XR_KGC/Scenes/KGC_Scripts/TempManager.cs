using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TempManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static TempManager Instance;
    //플레이어가 가진 모든 정보들을 관리해야되는 스크립트
    private string url = "http://13.124.6.53:8080/api/v1/room";
    //

    //플레이어의 커스텀 값  
    //플레이어의 고양이 종류,그 고양이가 가진 값들.고양이 성격,플레이어에 대한 호감도 등
    //플레이어가 가진 토큰,플레이어가 구입한 가구,플레이어가 가진 아이템
    //플레이어 가구들의 상태 ,고양이 밥그릇이 차있나 줄어들었나, 화장실이 비워져있나 등등
    //플레이어가 수락한 퀘스트들.

    //플레이어의 재산
    public int tokne;

    // 가구 데이터 리스트 (주석 제거),플레이어의 집에 배치된 가구들의 정보
    public List<GaguSave.GaguData> furnitureList = new List<GaguSave.GaguData>();

    void Awake()
    {
        // 이미 인스턴스가 존재하는 경우 이 오브젝트를 파괴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 인스턴스가 없는 경우 이 오브젝트를 인스턴스로 설정하고 파괴되지 않도록 설정
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(SendDataToServer()); 
        }
    }

    public IEnumerator SendDataToServer()
    {
        int ownerId = 7;

        GaguSave.GaguDataListWrapper dataWrapper = new GaguSave.GaguDataListWrapper(ownerId, furnitureList);
        string jsonData = JsonUtility.ToJson(dataWrapper);
        Debug.Log("Sending JSON Data: " + jsonData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data successfully sent to server: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Failed to send data: " + request.error);
        }
    }
}
