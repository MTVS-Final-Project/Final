using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CatData : MonoBehaviour
{
    [System.Serializable]
    public class CatStats
    {
        public string personality;
        public float sleepy;
        public float friendly;
        public float mood;
        public float hunger;
        public float moveTerm;
        public float moveRange;
        public float discharge;
        public float metabolism;
        public float weight;
        public float speed;
        public float eatSpeed;
        public int CatIndex;
        public float age;
        public bool male;

        public CatStats(string personality, float sleepy, float friendly, float mood, float hunger, float moveTerm,
            float moveRange, float discharge, float metabolism, float weight, float speed,
            float eatSpeed, int CatIndex, bool male, float age)
        {
            this.personality = personality;
            this.sleepy = sleepy;
            this.friendly = friendly;
            this.mood = mood;
            this.hunger = hunger;
            this.moveTerm = moveTerm;
            this.moveRange = moveRange;
            this.discharge = discharge;
            this.metabolism = metabolism;
            this.weight = weight;
            this.speed = speed;
            this.eatSpeed = eatSpeed;
            this.CatIndex = CatIndex;
            this.male = male;
            this.age = age;
        }
    }

    public CatStats catStats;

    void Start()
    {
        // CatStats 초기화 (예시로 값을 설정했습니다)
        catStats = new CatStats("Friendly", 100f, 100f, 100f, 100f, 5f, 2f, 0f, 1f, 3f, 1f, 1f, 0, false, 3f);

        // 서버로 CatStats 정보를 전송합니다.
        StartCoroutine(SendCatStatsToServer());
    }

    private IEnumerator SendCatStatsToServer()
    {
        // JSON 형식으로 변환하기 위한 객체
        var catDataForServer = new
        {
            ownerId = 7,
            personality = catStats.personality,
            age = catStats.age,
            weight = catStats.weight,
            male = catStats.male,
            sleepy = catStats.sleepy,
            hunger = catStats.hunger,
            friendly = catStats.friendly,
            mood = catStats.mood,
            metabolism = catStats.metabolism,
            moveTerm = catStats.moveTerm,
            moveRange = catStats.moveRange,
            discharge = catStats.discharge,
            speed = catStats.speed,
            eatSpeed = catStats.eatSpeed,
            catIndex = catStats.CatIndex
        };

        // JSON 문자열로 변환
        string json = JsonUtility.ToJson(catDataForServer);
        Debug.Log("JSON Data: " + json);

        // 서버에 POST 요청 보내기
        UnityWebRequest request = new UnityWebRequest("http://13.124.6.53:8080/api/v1/cat", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // 서버 응답 대기
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Cat stats successfully sent to the server: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Failed to send cat stats to the server: " + request.error);
        }
    }
}
