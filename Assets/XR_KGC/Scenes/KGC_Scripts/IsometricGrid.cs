using UnityEngine;

public class IsometricGridWithLines : MonoBehaviour
{
    public GameObject tilePrefab;      // Ÿ�� ������
    public int gridSize = 5;           // �׸��� ũ��
    public float tileWidth = 1.0f;     // Ÿ���� �ʺ�
    public float tileHeight = 0.5f;    // Ÿ���� ����
    public Material lineMaterial;      // ���� �������� ����� ��Ƽ����
    private float yOffset = -0.25f;    // y�� ������
    private float zOffset = -0.02f;     // z�� ������

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
            // ������ �׸���
            DrawLine(
                GetTilePosition(x, 0) + new Vector3(0, yOffset, zOffset),
                GetTilePosition(x, gridSize) + new Vector3(0, yOffset, zOffset)
            );
        }

        for (int y = 0; y <= gridSize; y++)
        {
            // ���� �׸���
            DrawLine(
                GetTilePosition(0, y) + new Vector3(0, yOffset, zOffset),
                GetTilePosition(gridSize, y) + new Vector3(0, yOffset, zOffset)
            );
        }
    }

    Vector3 GetTilePosition(int x, int y)
    {
        // ���̼Ҹ�Ʈ�� ��ġ ���
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
