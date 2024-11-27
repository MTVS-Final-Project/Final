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



    private string url = "https://safe-quickly-lioness.ngrok-free.app/predict-behavior/"; // ������ URL�� ���⿡ �Է��ϼ���

    private void Start()
    {
        for (int i = 0; i < cats.Count; i++)
        {
            Post(i);
        }
    }
    public void Post(int index)
    {
        // JSON �����͸� ����
        string jsonData = CreateJsonData();

        // �ڷ�ƾ�� ����Ͽ� �����͸� ���� �� ���� �Ľ�
        StartCoroutine(SendDataToServer(jsonData, index));
    }

    private string CreateJsonData()
    {
        float ran1 = Random.Range(-1f, 1f); // �Ű���
        float ran2 = Random.Range(-1f, 1f); // ���⼺
        float ran3 = Random.Range(-1f, 1f); // ���輺
        float ran4 = Random.Range(-1f, 1f); // �浿��
        float ran5 = Random.Range(-1f, 1f); // ��ȣ��

        // JSON ���ڿ� �ۼ�
        return "{\"�Ű���\": " + ran1.ToString("F2") +
               ", \"���⼺\": " + ran2.ToString("F2") +
               ", \"���輺\": " + ran3.ToString("F2") +
               ", \"�浿��\": " + ran4.ToString("F2") +
               ", \"��ȣ��\": " + ran5.ToString("F2") + "}";
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
            Debug.Log("���� ����: " + request.downloadHandler.text);

            // ���� ���ڿ����� ���� ����
            string responseText = request.downloadHandler.text;

            // "predicted_behavior" �� ����
            string predictedBehavior = responseText.Substring(responseText.IndexOf(":") + 2).Trim('}', '"');

            // �ʿ��� ���� ǥ��
            Debug.Log("Predicted Behavior: " + predictedBehavior);
            requesttext = predictedBehavior;
            // answer = predictedBehavior;

            // cats[index]�� �� �Ҵ�
            GameObject cat = cats[index];
            AssignBehaviorToCat(cat, predictedBehavior);

        }
        else
        {
            Debug.LogError("���� ����: " + request.error);
        }

    }
    private void AssignBehaviorToCat(GameObject cat, string behavior)
    {
        // GameObject�� �ش� ���� �Ҵ�
        // ���� ���, cat�� Ư�� ��ũ��Ʈ�� ������ �ִٸ�:
        CatAIFSM catAI = cat.GetComponent<CatAIFSM>();
        if (catAI != null)
        {
            catAI.personality = behavior; // ��ũ��Ʈ�� ���� ����
        }
        else
        {
            Debug.LogWarning("CatBehaviorScript�� " + cat.name + "�� �����ϴ�.");
        }



    }
}
