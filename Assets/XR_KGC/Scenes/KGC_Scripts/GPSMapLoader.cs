using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GPSMapLoader : MonoBehaviour
{
    public GameObject mapPlane; // 지도를 표시할 Plane 오브젝트
    public int zoom = 15; // 지도 줌 레벨
    private const string OSM_TILE_URL = "https://tile.openstreetmap.org/{0}/{1}/{2}.png";

    public bool useManualCoordinates = false; // 수동 좌표 입력 사용 여부
    public double manualLatitude = 37.5665; // 수동 입력 위도 (예: 서울)
    public double manualLongitude = 126.9780; // 수동 입력 경도 (예: 서울)

    private Vector2d lastLocation = new Vector2d(double.MinValue, double.MinValue);
    public float minDistanceUpdate = 5.0f; // 업데이트를 위한 최소 이동 거리 (미터 단위)

    private void Start()
    {
        if (useManualCoordinates)
        {
            StartCoroutine(UpdateManualLocation());
        }
        else
        {
            // GPS 위치 업데이트를 시작합니다.
            StartCoroutine(StartLocationService());
        }
    }

    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("GPS가 사용 설정되지 않았습니다.");
            yield break;
        }

        Input.location.Start();

        // GPS 활성화가 안되었다면 활성화 될 때까지 대기합니다.
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("GPS를 가져올 수 없습니다.");
            yield break;
        }
        else if (Input.location.status != LocationServiceStatus.Running)
        {
            Debug.LogError("GPS가 활성화되지 않았습니다.");
            yield break;
        }
        else
        {
            StartCoroutine(UpdateGPSLocation());
        }
    }

    private IEnumerator UpdateGPSLocation()
    {
        while (true)
        {
            // 현재 위치를 가져옵니다.
            double latitude = Input.location.lastData.latitude;
            double longitude = Input.location.lastData.longitude;
            Vector2d currentLocation = new Vector2d(latitude, longitude);

            lastLocation = currentLocation;
            StartCoroutine(LoadMapTexture(latitude, longitude));
            StartCoroutine(LoadMapTexture(latitude, longitude));

            yield return new WaitForSeconds(1f); // 1초마다 위치 업데이트 확인
        }
    }

    private IEnumerator UpdateManualLocation()
    {
        while (true)
        {
            // 수동으로 입력된 위치를 사용하여 지도를 업데이트합니다.
            StartCoroutine(LoadMapTexture(manualLatitude, manualLongitude));

            yield return new WaitForSeconds(1f); // 1초마다 위치 업데이트
        }
    }

    private IEnumerator LoadMapTexture(double latitude, double longitude)
    {
        // 위도와 경도를 타일 좌표로 변환합니다.
        int tileX, tileY;
        LatLongToTileXY(latitude, longitude, zoom, out tileX, out tileY);

        // OSM 타일 URL을 생성합니다.
        string url = string.Format(OSM_TILE_URL, zoom, tileX, tileY);

        // 타일 이미지를 다운로드합니다.
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("지도 타일을 가져오지 못했습니다: " + www.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                mapPlane.GetComponent<Renderer>().material.mainTexture = texture;
            }
        }
    }

    private void LatLongToTileXY(double latitude, double longitude, int zoom, out int tileX, out int tileY)
    {
        double latRad = latitude * Mathf.Deg2Rad;
        double n = Mathf.Pow(2.0f, zoom);
        tileX = (int)((longitude + 180.0) / 360.0 * n);
        tileY = (int)((1.0 - Mathf.Log(Mathf.Tan((float)latRad) + (1 / Mathf.Cos((float)latRad))) / Mathf.PI) / 2.0 * n);
    }

    private double GetDistanceInMeters(Vector2d point1, Vector2d point2)
    {
        double R = 6371000; // 지구 반지름 (미터)
        double dLat = (point2.x - point1.x) * Mathf.Deg2Rad;
        double dLon = (point2.y - point1.y) * Mathf.Deg2Rad;
        double a = Mathf.Sin((float)dLat / 2) * Mathf.Sin((float)dLat / 2) +
                   Mathf.Cos((float)(point1.x * Mathf.Deg2Rad)) * Mathf.Cos((float)(point2.x * Mathf.Deg2Rad)) *
                   Mathf.Sin((float)dLon / 2) * Mathf.Sin((float)dLon / 2);
        double c = 2 * Mathf.Atan2(Mathf.Sqrt((float)a), Mathf.Sqrt((float)(1 - a)));
        return R * c;
    }

    private void OnDisable()
    {
        Input.location.Stop();
    }

    private struct Vector2d
    {
        public double x, y;

        public Vector2d(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}