using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Tracker : MonoBehaviour
{
    private bool isLocationServiceEnabled;
    public float latitude = 35.8742f;
    public float longitude;

    public GameObject mapPlane; // Unity Plane 오브젝트
    public int zoomLevel = 15; // 확대/축소 레벨
    private Dictionary<string, Texture2D> tileCache = new Dictionary<string, Texture2D>();

    private Vector2 lastPosition;
    private float updateThreshold = 0.01f;

    void Start()
    {
#if UNITY_EDITOR
        isLocationServiceEnabled = true;
        lastPosition = new Vector2(latitude, longitude);
#else
        StartCoroutine(StartLocationService());
#endif
    }

    IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Location services are not enabled by the user.");
            yield break;
        }

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0)
        {
            Debug.LogError("Timed out while initializing location services.");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location.");
            yield break;
        }
        else
        {
            isLocationServiceEnabled = true;
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            lastPosition = new Vector2(latitude, longitude);
            StartCoroutine(LoadMap(latitude, longitude));
        }
    }

    void Update()
    {
#if UNITY_EDITOR
#else
        if (isLocationServiceEnabled)
        {
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
        }
#endif

        Vector2 currentPosition = new Vector2(latitude, longitude);
        if (Vector2.Distance(currentPosition, lastPosition) > updateThreshold)
        {
            lastPosition = currentPosition;
            StartCoroutine(LoadMap(latitude, longitude));
        }
    }

    IEnumerator LoadMap(float latitude, float longitude)
    {
        int tileX = TileX(longitude, zoomLevel);
        int tileY = TileY(latitude, zoomLevel);

        // 100 제곱킬로미터 범위의 지도 데이터를 미리 다운로드
        int tileRange = 10; // 약 100 제곱킬로미터를 커버할 수 있는 타일 범위
        for (int xOffset = -tileRange; xOffset <= tileRange; xOffset++)
        {
            for (int yOffset = -tileRange; yOffset <= tileRange; yOffset++)
            {
                int currentTileX = tileX + xOffset;
                int currentTileY = tileY + yOffset;

                string tileKey = $"{zoomLevel}/{currentTileX}/{currentTileY}";

                if (!tileCache.ContainsKey(tileKey))
                {
                    string url = $"https://tile.openstreetmap.org/{zoomLevel}/{currentTileX}/{currentTileY}.png";
                    Debug.Log("Requesting map tile from URL: " + url);

                    using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
                    {
                        yield return www.SendWebRequest();

                        if (www.result == UnityWebRequest.Result.Success)
                        {
                            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                            tileCache[tileKey] = texture;
                            if (xOffset == 0 && yOffset == 0) ApplyTextureToPlane(mapPlane, texture);
                        }
                        else
                        {
                            Debug.LogError("Failed to load map tile: " + www.error);
                        }
                    }
                }
            }
        }
    }

    void ApplyTextureToPlane(GameObject plane, Texture2D texture)
    {
        if (plane != null)
        {
            Renderer renderer = plane.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.mainTexture = texture;
                renderer.material.mainTextureScale = new Vector2(1, 1); // UV 좌표 스케일 설정
            }
            else
            {
                Debug.LogError("The specified GameObject does not have a Renderer component.");
            }
        }
        else
        {
            Debug.LogError("Map Plane GameObject is not assigned.");
        }
    }

    int TileX(double lon, int zoom)
    {
        return Mathf.Clamp((int)((lon + 180.0) / 360.0 * (1 << zoom)), 0, (1 << zoom) - 1);
    }

    int TileY(double lat, int zoom)
    {
        float latFloat = (float)lat;
        int maxTile = (1 << zoom) - 1;
        return Mathf.Clamp((int)((1.0f - Mathf.Log(Mathf.Tan(latFloat * Mathf.PI / 180.0f) + 1.0f / Mathf.Cos(latFloat * Mathf.PI / 180.0f)) / Mathf.PI) / 2.0f * (1 << zoom)), 0, maxTile);
    }
}