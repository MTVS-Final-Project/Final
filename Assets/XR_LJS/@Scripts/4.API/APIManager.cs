using System;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using UnityEditor;

public class APIManager : MonoBehaviour
{
    public static APIManager Instance;
    private string baseUrl = "http://13.124.6.53:8080/api/v1"; // API ������ ������ �⺻ �ּ�

    [Serializable]
    public class CustomizationData
    {
        // API ������ �°� �ʵ�� ����
        public int skin = 0;
        public int hair = 0;
        public int eye = 0;
        public int mouth = 0;
        public int leftArm = 0;
        public int rightArm = 0;
        public int pants = 0;
        public int leftLeg = 0;
        public int rightLeg = 0;
        public int leftShoe = 0;
        public int rightShoe = 0;
    }

    [Serializable]
    public class ApiResponse<T>
    {
        public bool success;
        public T data;
        public string message;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupSecuritySettings();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void SetupSecuritySettings()
    {
        PlayerSettings.insecureHttpOption = InsecureHttpOption.AlwaysAllowed;
        Debug.LogWarning("���� ȯ��: HTTP ����� ���Ǿ����ϴ�.");
    }

    // Photon CustomProperties���� Ŀ���͸���¡ ������ ����
    public CustomizationData ExtractCustomizationFromPhoton(ExitGames.Client.Photon.Hashtable properties)
    {
        var data = new CustomizationData
        {
            skin = properties.TryGetValue("BodyUI", out object body) ? (int)body : 0,
            hair = properties.TryGetValue("HairUI", out object hair) ? (int)hair : 0,
            eye = properties.TryGetValue("EyesUI", out object eye) ? (int)eye : 0,
            mouth = properties.TryGetValue("MouthUI", out object mouth) ? (int)mouth : 0,
            leftArm = properties.TryGetValue("LeftArmUI", out object leftArm) ? (int)leftArm : 0,
            rightArm = properties.TryGetValue("rightArmUI", out object rightArm) ? (int)rightArm : 0,
            pants = properties.TryGetValue("PantsUI", out object pants) ? (int)pants : 0,
            leftLeg = properties.TryGetValue("leftLegUI", out object leftLeg) ? (int)leftLeg : 0,
            rightLeg = properties.TryGetValue("rightLegUI", out object rightLeg) ? (int)rightLeg : 0,
            leftShoe = properties.TryGetValue("leftShoesUI", out object leftShoe) ? (int)leftShoe : 0,
            rightShoe = properties.TryGetValue("rightShoesUI", out object rightShoe) ? (int)rightShoe : 0
        };
        return data;
    }

    // ĳ���� ������ ������Ʈ
    public IEnumerator UpdateCharacterData(string userId, CustomizationData customization)
    {
        string jsonData = JsonUtility.ToJson(customization);
        Debug.Log($"������ ������: {jsonData}");

        string url = $"{baseUrl}/character/{userId}";
        Debug.Log($"��û URL: {url}");

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.certificateHandler = new BypassCertificate();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"API ����: {request.downloadHandler.text}");
                var response = JsonUtility.FromJson<ApiResponse<CustomizationData>>(request.downloadHandler.text);
                if (response.success)
                {
                    Debug.Log("ĳ���� ���� ������Ʈ ����!");
                }
                else
                {
                    Debug.LogError($"API ����: {response.message}");
                }
            }
            else
            {
                Debug.LogError($"API ��û ����: {request.error}");
                Debug.LogError($"�� ����: {request.downloadHandler?.text}");
            }
        }
    }

    // GET: ĳ���� ������ ��ȸ
    public IEnumerator GetCharacterData(string userId)
    {
        string url = $"{baseUrl}/character/{userId}";
        Debug.Log($"��û URL: {url}");

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.certificateHandler = new BypassCertificate();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"API ����: {request.downloadHandler.text}");
                var response = JsonUtility.FromJson<ApiResponse<CustomizationData>>(request.downloadHandler.text);
                if (response.success)
                {
                    Debug.Log($"ĳ���� ������ ��ȸ ����: {JsonUtility.ToJson(response.data)}");
                }
                else
                {
                    Debug.LogError($"API ����: {response.message}");
                }
            }
            else
            {
                Debug.LogError($"ĳ���� ������ ��ȸ ����: {request.error}");
                Debug.LogError($"�� ����: {request.downloadHandler?.text}");
            }
        }
    }
}

public class BypassCertificate : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}