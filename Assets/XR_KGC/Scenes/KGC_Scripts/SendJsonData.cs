using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using System.Collections.Generic;



[System.Serializable]
public class ResponseData
{
    public float predicted_behavior;
}


public class SendJsonData : MonoBehaviour
{
    public List<GameObject> cats = new List<GameObject>();

    public string requesttext;
    //public string answer;



    private string url = "https://safe-quickly-lioness.ngrok-free.app/predict-behavior/"; // 서버의 URL을 여기에 입력하세요

    private void Start()
    {
        for (int i = 0; i < cats.Count; i++)
        {
            Post(i);
        }
    }
    public void Post(int index)
    {
        // JSON 데이터를 생성
        string jsonData = CreateJsonData();

        // 코루틴을 사용하여 데이터를 전송 및 응답 파싱
        StartCoroutine(SendDataToServer(jsonData, index));
    }

    private string CreateJsonData()
    {
        float ran1 = Random.Range(-1f, 1f); // 신경증
        float ran2 = Random.Range(-1f, 1f); // 외향성
        float ran3 = Random.Range(-1f, 1f); // 지배성
        float ran4 = Random.Range(-1f, 1f); // 충동성
        float ran5 = Random.Range(-1f, 1f); // 우호성

        // JSON 문자열 작성
        return "{\"신경증\": " + ran1.ToString("F2") +
               ", \"외향성\": " + ran2.ToString("F2") +
               ", \"지배성\": " + ran3.ToString("F2") +
               ", \"충동성\": " + ran4.ToString("F2") +
               ", \"우호성\": " + ran5.ToString("F2") + "}";
    }

    private IEnumerator SendDataToServer(string jsonData, int index)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("전송 성공: " + request.downloadHandler.text);

            // 응답 문자열에서 값만 추출
            string responseText = request.downloadHandler.text;

            // "predicted_behavior" 값 추출
            string predictedBehavior = responseText.Substring(responseText.IndexOf(":") + 2).Trim('}', '"');

            // 필요한 값만 표시
            Debug.Log("Predicted Behavior: " + predictedBehavior);
            requesttext = predictedBehavior;
            // answer = predictedBehavior;

            // cats[index]에 값 할당
            GameObject cat = cats[index];
            AssignBehaviorToCat(cat, predictedBehavior);

        }
        else
        {
            Debug.LogError("전송 실패: " + request.error);
        }

    }
    private void AssignBehaviorToCat(GameObject cat, string behavior)
    {
        // GameObject에 해당 값을 할당
        // 예를 들어, cat이 특정 스크립트를 가지고 있다면:
        CatAIFSM catAI = cat.GetComponent<CatAIFSM>();
        if (catAI != null)
        {
            catAI.personality = behavior; // 스크립트에 값을 전달
        }
        else
        {
            Debug.LogWarning("CatBehaviorScript가 " + cat.name + "에 없습니다.");
        }



    }
}
