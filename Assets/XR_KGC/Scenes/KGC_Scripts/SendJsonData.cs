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


    private string url = "https://safe-quickly-lioness.ngrok-free.app/predict-behavior/"; // ������ URL�� ���⿡ �Է��ϼ���

    public void Post()
    {
        // JSON �����͸� ����
        string jsonData = CreateJsonData();

        // �ڷ�ƾ�� ����Ͽ� �����͸� ���� �� ���� �Ľ�
        StartCoroutine(SendDataToServer(jsonData));
    }

    private string CreateJsonData()
    {
        // Ư�� Ű �̸��� ������ JSON ���ڿ� �ۼ�
        return "{\"�Ű���\": 0.1, \"���⼺\": 0.10, \"���輺\": 0.60, \"�浿��\": 0.25, \"��ȣ��\": -0.65}";
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
            Debug.Log("���� ����: " + request.downloadHandler.text);

            // ���� JSON �Ľ�
            ResponseData responseData = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);
            Debug.Log("Predicted Behavior: " + responseData.predicted_behavior);

            requesttext.text = responseData.predicted_behavior.ToString() + request.downloadHandler.text;
        }
        else
        {
            Debug.LogError("���� ����: " + request.error);
        }
    }
}
