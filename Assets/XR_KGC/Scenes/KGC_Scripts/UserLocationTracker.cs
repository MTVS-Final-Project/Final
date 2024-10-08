using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class UserLocationTracker : MonoBehaviour
{
    private bool isLocationServiceEnabled;
    public float latitude = 35.8742f;  // public으로 설정하여 Inspector에서 수정 가능
    public float longitude; // public으로 설정하여 Inspector에서 수정 가능

    public GameObject mapPlanePrefab; // Unity Plane의 Prefab
    public int zoomLevel = 15; // 확대/축소 레벨
    private Dictionary<string, Texture2D> tileCache = new Dictionary<string, Texture2D>();

    private Vector2 lastPosition;
    private float updateThreshold = 0.01f; // 위치 변화가 있을 경우 업데이트 기준

    void Start()
    {
#if UNITY_EDITOR
        // 에디터에서는 위치 서비스 대신 가상의 데이터 사용
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
        // 에디터에서는 값을 변경할 수 있으므로 이동 감지를 위해 Update에서 계속 확인
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

        UpdateUserPositionOnMap(latitude, longitude);

        if (Input.GetKeyDown(KeyCode.A))
        {
            latitude += 0.001f;
        }
    }

    void OnValidate()
    {
        // Inspector에서 latitude, longitude 값이 변경될 때 호출됨
        if (Application.isPlaying)
        {
            StartCoroutine(LoadMap(latitude, longitude));
        }
    }

    IEnumerator LoadMap(float latitude, float longitude)
    {
        int tileX = TileX(longitude, zoomLevel);
        int tileY = TileY(latitude, zoomLevel);

        // 현재 위치와 주변 타일들을 캐싱
        int tileRange = 2; // 현재 위치의 주변 2칸씩을 캐싱
        for (int xOffset = -tileRange; xOffset <= tileRange; xOffset++)
        {
            for (int yOffset = -tileRange; yOffset <= tileRange; yOffset++)
            {
                int currentTileX = tileX + xOffset;
                int currentTileY = tileY + yOffset;

                string tileKey = $"{zoomLevel}/{currentTileX}/{currentTileY}";
                Vector3 planePosition = LatLonToUnityPosition(latitude + yOffset * 0.01f, longitude + xOffset * 0.01f);

                // 카메라 영역 안에 Plane이 없으면 생성
                if (!Physics.CheckBox(planePosition, new Vector3(5f, 0.1f, 5f)))
                {
                    GameObject newMapPlane = Instantiate(mapPlanePrefab, planePosition, Quaternion.identity);
                    // Plane은 누구의 자손도 아니므로 부모 설정을 하지 않음

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
                                ApplyTextureToPlane(newMapPlane, texture);
                            }
                            else
                            {
                                Debug.LogError("Failed to load map tile: " + www.error);
                            }
                        }
                    }
                    else
                    {
                        ApplyTextureToPlane(newMapPlane, tileCache[tileKey]);
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

    void UpdateMapTexture(float latitude, float longitude)
    {
        StartCoroutine(LoadMap(latitude, longitude));
    }

    void UpdateUserPositionOnMap(float latitude, float longitude)
    {
        Vector2 currentPosition = new Vector2(latitude, longitude);
        if (Vector2.Distance(currentPosition, lastPosition) > updateThreshold)
        {
            lastPosition = currentPosition;
            UpdateMapTexture(latitude, longitude);
        }
    }

    Vector3 LatLonToUnityPosition(float latitude, float longitude)
    {
        float scaleFactor = 1000.0f; // 축척 비율
        float x = longitude * scaleFactor;
        float z = latitude * scaleFactor;
        return new Vector3(x, 0, z);
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