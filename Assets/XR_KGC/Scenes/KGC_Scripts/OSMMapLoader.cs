using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class OSMMapLoader : MonoBehaviour
{
    public GameObject mapPlane; // Unity Plane 오브젝트
    public int zoomLevel = 15; // 확대/축소 레벨 (높을수록 해상도 높아짐)
    private Dictionary<string, Texture2D> tileCache = new Dictionary<string, Texture2D>();

       public float latitude = 37.7749f;
       public float longitude = -122.4194f;
    // 타일 캐싱 범위 설정 
    private int tileRange = 2;

    void Start()
    {
        // 사용자의 위치 정보를 가져옵니다. 여기서는 가상 위치 데이터를 사용합니다.

        StartCoroutine(LoadMap(latitude, longitude));
    }

    IEnumerator LoadMap(float latitude, float longitude)
    {
        int tileX = TileX(longitude, zoomLevel);
        int tileY = TileY(latitude, zoomLevel);

        // 현재 위치와 주변 타일들을 함께 캐싱
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

                            if (xOffset == 0 && yOffset == 0)
                            {
                                // 현재 위치의 타일을 Plane에 적용
                                ApplyTextureToPlane(texture);
                            }
                        }
                        else
                        {
                            Debug.LogError("Failed to load map tile: " + www.error);
                        }
                    }
                }
                else
                {
                    if (xOffset == 0 && yOffset == 0)
                    {
                        // 현재 위치의 타일을 캐시에서 Plane에 적용
                        ApplyTextureToPlane(tileCache[tileKey]);
                    }
                }
            }
        }
    }

    void ApplyTextureToPlane(Texture2D texture)
    {
        if (mapPlane != null)
        {
            Renderer renderer = mapPlane.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.mainTexture = texture;
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
