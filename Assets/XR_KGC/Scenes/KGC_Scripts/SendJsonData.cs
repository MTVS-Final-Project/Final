using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using UnityEngine.UI;



[System.Serializable]
public class ResponseData
{
    public float predicted_behavior;
}


public class SendJsonData : MonoBehaviour
{


     public Text requesttext;


    private string url = "https://safe-quickly-lioness.ngrok-free.app/predict-behavior/"; // 서버의 URL을 여기에 입력하세요

    public void Post()
    {
        // JSON 데이터를 생성
        string jsonData = CreateJsonData();

        // 코루틴을 사용하여 데이터를 전송 및 응답 파싱
        StartCoroutine(SendDataToServer(jsonData));
    }

    private string CreateJsonData()
    {
        // 특수 키 이름을 포함한 JSON 문자열 작성
        return "{\"신경증\": 0.1, \"외향성\": 0.10, \"지배성\": 0.60, \"충동성\": 0.25, \"우호성\": -0.65}";
    }

    private IEnumerator SendDataToServer(string jsonData)
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

            // 응답 JSON 파싱
            ResponseData responseData = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);
            Debug.Log("Predicted Behavior: " + responseData.predicted_behavior);

            requesttext.text = responseData.predicted_behavior.ToString() + request.downloadHandler.text;
        }
        else
        {
            Debug.LogError("전송 실패: " + request.error);
        }
    }
}
