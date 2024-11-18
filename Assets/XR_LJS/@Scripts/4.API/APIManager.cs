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
    private string baseUrl = "http://13.124.6.53:8080/api/v1"; // API 버전을 포함한 기본 주소

    [Serializable]
    public class CustomizationData
    {
        // API 문서에 맞게 필드명 수정
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
        Debug.LogWarning("개발 환경: HTTP 통신이 허용되었습니다.");
    }

    // Photon CustomProperties에서 커스터마이징 데이터 추출
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

    // 캐릭터 데이터 업데이트
    public IEnumerator UpdateCharacterData(string userId, CustomizationData customization)
    {
        string jsonData = JsonUtility.ToJson(customization);
        Debug.Log($"전송할 데이터: {jsonData}");

        string url = $"{baseUrl}/character/{userId}";
        Debug.Log($"요청 URL: {url}");

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
                Debug.Log($"API 응답: {request.downloadHandler.text}");
                var response = JsonUtility.FromJson<ApiResponse<CustomizationData>>(request.downloadHandler.text);
                if (response.success)
                {
                    Debug.Log("캐릭터 정보 업데이트 성공!");
                }
                else
                {
                    Debug.LogError($"API 오류: {response.message}");
                }
            }
            else
            {
                Debug.LogError($"API 요청 실패: {request.error}");
                Debug.LogError($"상세 에러: {request.downloadHandler?.text}");
            }
        }
    }

    // GET: 캐릭터 데이터 조회
    public IEnumerator GetCharacterData(string userId)
    {
        string url = $"{baseUrl}/character/{userId}";
        Debug.Log($"요청 URL: {url}");

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.certificateHandler = new BypassCertificate();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"API 응답: {request.downloadHandler.text}");
                var response = JsonUtility.FromJson<ApiResponse<CustomizationData>>(request.downloadHandler.text);
                if (response.success)
                {
                    Debug.Log($"캐릭터 데이터 조회 성공: {JsonUtility.ToJson(response.data)}");
                }
                else
                {
                    Debug.LogError($"API 오류: {response.message}");
                }
            }
            else
            {
                Debug.LogError($"캐릭터 데이터 조회 실패: {request.error}");
                Debug.LogError($"상세 에러: {request.downloadHandler?.text}");
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