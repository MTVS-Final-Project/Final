using UnityEngine;

public class IsometricGridWithLines : MonoBehaviour
{
    public GameObject tilePrefab;      // 타일 프리팹
    public int gridSize = 5;           // 그리드 크기
    public float tileWidth = 1.0f;     // 타일의 너비
    public float tileHeight = 0.5f;    // 타일의 높이
    public Material lineMaterial;      // 라인 렌더러에 사용할 머티리얼
    private float yOffset = -0.25f;    // y축 보정값
    private float zOffset = -0.02f;     // z축 보정값

    void Start()
    {
        CreateIsometricGrid();
        DrawGridLines();
    }

    void CreateIsometricGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                float posX = (x - y) * tileWidth / 2;
                float posY = (x + y) * tileHeight / 2;

                Vector3 tilePosition = new Vector3(posX, posY, 0);
                Instantiate(tilePrefab, tilePosition, Quaternion.identity, transform);
            }
        }
    }

    void DrawGridLines()
    {
        for (int x = 0; x <= gridSize; x++)
        {
            // 수직선 그리기
            DrawLine(
                GetTilePosition(x, 0) + new Vector3(0, yOffset, zOffset),
                GetTilePosition(x, gridSize) + new Vector3(0, yOffset, zOffset)
            );
        }

        for (int y = 0; y <= gridSize; y++)
        {
            // 수평선 그리기
            DrawLine(
                GetTilePosition(0, y) + new Vector3(0, yOffset, zOffset),
                GetTilePosition(gridSize, y) + new Vector3(0, yOffset, zOffset)
            );
        }
    }

    Vector3 GetTilePosition(int x, int y)
    {
        // 아이소메트릭 위치 계산
        float posX = (x - y) * tileWidth / 2;
        float posY = (x + y) * tileHeight / 2;
        return new Vector3(posX, posY, 0);
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("GridLine");
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        lineRenderer.material = lineMaterial;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
    }
}
