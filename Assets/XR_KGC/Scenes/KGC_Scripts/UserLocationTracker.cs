using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class UserLocationTracker : MonoBehaviour
{
    private bool isLocationServiceEnabled;
    public float latitude = 35.8742f;  // public���� �����Ͽ� Inspector���� ���� ����
    public float longitude; // public���� �����Ͽ� Inspector���� ���� ����

    public GameObject mapPlanePrefab; // Unity Plane�� Prefab
    public int zoomLevel = 15; // Ȯ��/��� ����
    private Dictionary<string, Texture2D> tileCache = new Dictionary<string, Texture2D>();

    private Vector2 lastPosition;
    private float updateThreshold = 0.01f; // ��ġ ��ȭ�� ���� ��� ������Ʈ ����

    void Start()
    {
#if UNITY_EDITOR
        // �����Ϳ����� ��ġ ���� ��� ������ ������ ���
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
        // �����Ϳ����� ���� ������ �� �����Ƿ� �̵� ������ ���� Update���� ��� Ȯ��
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
        // Inspector���� latitude, longitude ���� ����� �� ȣ���
        if (Application.isPlaying)
        {
            StartCoroutine(LoadMap(latitude, longitude));
        }
    }

    IEnumerator LoadMap(float latitude, float longitude)
    {
        int tileX = TileX(longitude, zoomLevel);
        int tileY = TileY(latitude, zoomLevel);

        // ���� ��ġ�� �ֺ� Ÿ�ϵ��� ĳ��
        int tileRange = 2; // ���� ��ġ�� �ֺ� 2ĭ���� ĳ��
        for (int xOffset = -tileRange; xOffset <= tileRange; xOffset++)
        {
            for (int yOffset = -tileRange; yOffset <= tileRange; yOffset++)
            {
                int currentTileX = tileX + xOffset;
                int currentTileY = tileY + yOffset;

                string tileKey = $"{zoomLevel}/{currentTileX}/{currentTileY}";
                Vector3 planePosition = LatLonToUnityPosition(latitude + yOffset * 0.01f, longitude + xOffset * 0.01f);

                // ī�޶� ���� �ȿ� Plane�� ������ ����
                if (!Physics.CheckBox(planePosition, new Vector3(5f, 0.1f, 5f)))
                {
                    GameObject newMapPlane = Instantiate(mapPlanePrefab, planePosition, Quaternion.identity);
                    // Plane�� ������ �ڼյ� �ƴϹǷ� �θ� ������ ���� ����

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
        float scaleFactor = 1000.0f; // ��ô ����
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