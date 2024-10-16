using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AvatarApiClient : MonoBehaviour
{
    private const string BASE_URL = "http://64.176.228.230:8756/";

    public IEnumerator GetAvatarData(string userId, System.Action<AvatarData> callback)
    {
        string url = $"{BASE_URL}/avatar/{userId}";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {www.error}");
            }
            else
            {
                AvatarData avatarData = JsonUtility.FromJson<AvatarData>(www.downloadHandler.text);
                callback(avatarData);
            }
        }
    }

    public IEnumerator UpdateAvatarData(string userId, AvatarData avatarData, System.Action<bool> callback)
    {
        string url = $"{BASE_URL}/avatar/{userId}";
        string jsonData = JsonUtility.ToJson(avatarData);
        using (UnityWebRequest www = UnityWebRequest.Put(url, jsonData))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {www.error}");
                callback(false);
            }
            else
            {
                callback(true);
            }
        }
    }
}