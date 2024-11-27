using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TempManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static TempManager Instance;
    //�÷��̾ ���� ��� �������� �����ؾߵǴ� ��ũ��Ʈ
    private string url = "http://13.124.6.53:8080/api/v1/room";
    //

    //�÷��̾��� Ŀ���� ��  
    //�÷��̾��� ����� ����,�� ����̰� ���� ����.����� ����,�÷��̾ ���� ȣ���� ��
    //�÷��̾ ���� ��ū,�÷��̾ ������ ����,�÷��̾ ���� ������
    //�÷��̾� �������� ���� ,����� ��׸��� ���ֳ� �پ�����, ȭ����� ������ֳ� ���
    //�÷��̾ ������ ����Ʈ��.

    //�÷��̾��� ���
    public int tokne;

    // ���� ������ ����Ʈ (�ּ� ����),�÷��̾��� ���� ��ġ�� �������� ����
    public List<GaguSave.GaguData> furnitureList = new List<GaguSave.GaguData>();

    void Awake()
    {
        // �̹� �ν��Ͻ��� �����ϴ� ��� �� ������Ʈ�� �ı�
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // �ν��Ͻ��� ���� ��� �� ������Ʈ�� �ν��Ͻ��� �����ϰ� �ı����� �ʵ��� ����
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
