using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GPSMapLoader : MonoBehaviour
{
    public GameObject mapPlane; // ������ ǥ���� Plane ������Ʈ
    public int zoom = 15; // ���� �� ����
    private const string OSM_TILE_URL = "https://tile.openstreetmap.org/{0}/{1}/{2}.png";

    public bool useManualCoordinates = false; // ���� ��ǥ �Է� ��� ����
    public double manualLatitude = 37.5665; // ���� �Է� ���� (��: ����)
    public double manualLongitude = 126.9780; // ���� �Է� �浵 (��: ����)

    private Vector2d lastLocation = new Vector2d(double.MinValue, double.MinValue);
    public float minDistanceUpdate = 5.0f; // ������Ʈ�� ���� �ּ� �̵� �Ÿ� (���� ����)

    private void Start()
    {
        if (useManualCoordinates)
        {
            StartCoroutine(UpdateManualLocation());
        }
        else
        {
            // GPS ��ġ ������Ʈ�� �����մϴ�.
            StartCoroutine(StartLocationService());
        }
    }

    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("GPS�� ��� �������� �ʾҽ��ϴ�.");
            yield break;
        }

        Input.location.Start();

        // GPS Ȱ��ȭ�� �ȵǾ��ٸ� Ȱ��ȭ �� ������ ����մϴ�.
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("GPS�� ������ �� �����ϴ�.");
            yield break;
        }
        else if (Input.location.status != LocationServiceStatus.Running)
        {
            Debug.LogError("GPS�� Ȱ��ȭ���� �ʾҽ��ϴ�.");
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
            // ���� ��ġ�� �����ɴϴ�.
            double latitude = Input.location.lastData.latitude;
            double longitude = Input.location.lastData.longitude;
            Vector2d currentLocation = new Vector2d(latitude, longitude);

            lastLocation = currentLocation;
            StartCoroutine(LoadMapTexture(latitude, longitude));
            StartCoroutine(LoadMapTexture(latitude, longitude));

            yield return new WaitForSeconds(1f); // 1�ʸ��� ��ġ ������Ʈ Ȯ��
        }
    }

    private IEnumerator UpdateManualLocation()
    {
        while (true)
        {
            // �������� �Էµ� ��ġ�� ����Ͽ� ������ ������Ʈ�մϴ�.
            StartCoroutine(LoadMapTexture(manualLatitude, manualLongitude));

            yield return new WaitForSeconds(1f); // 1�ʸ��� ��ġ ������Ʈ
        }
    }

    private IEnumerator LoadMapTexture(double latitude, double longitude)
    {
        // ������ �浵�� Ÿ�� ��ǥ�� ��ȯ�մϴ�.
        int tileX, tileY;
        LatLongToTileXY(latitude, longitude, zoom, out tileX, out tileY);

        // OSM Ÿ�� URL�� �����մϴ�.
        string url = string.Format(OSM_TILE_URL, zoom, tileX, tileY);

        // Ÿ�� �̹����� �ٿ�ε��մϴ�.
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("���� Ÿ���� �������� ���߽��ϴ�: " + www.error);
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
        double R = 6371000; // ���� ������ (����)
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