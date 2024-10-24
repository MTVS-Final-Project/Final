using System;
using UnityEngine;

public class GridPlacement : MonoBehaviour
{
    public float gridSize = 1.0f; // ������ ũ�� ����
    public Material highlightMaterial; // ���̶���Ʈ ȿ���� ���� ����
    private GameObject highlightObject;
    private Camera mainCamera;
    private bool gridDrawn = false;

    [SerializeField]
    float gridDrawExtent = 5.0f; // �׸����� �ִ� ���� ����
           

    private void Start()
    {
        mainCamera = Camera.main;
        DrawGrid();
        if (mainCamera != null)
        {
            print("ī�޶� ����");
        }
        else
        {
            print("����");
        }
    }

    private void Update()
    {
        HighlightGrid();

        if (Input.GetMouseButtonDown(0)) // ���콺 ���� Ŭ���� ����
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Vector3 targetPosition = hitInfo.point;
                Vector3 snappedPosition = SnapToGrid(targetPosition);

                // ��ü ���� �Ǵ� ��ġ
                GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                newObject.transform.position = snappedPosition;
            }
        }
    }

    private void HighlightGrid()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPosition = hitInfo.point;
            Vector3 snappedPosition = SnapToGrid(targetPosition);

            if (highlightObject == null)
            {
                highlightObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                highlightObject.GetComponent<Collider>().enabled = false; // �浹 ��Ȱ��ȭ
                if (highlightMaterial != null)
                {
                    highlightObject.GetComponent<Renderer>().material = highlightMaterial;
                }
                else
                {
                    Debug.LogWarning("Highlight material is not assigned.");
                }
            }
            highlightObject.transform.position = snappedPosition;
        }
    }

    private void DrawGrid()
    {
        if (!gridDrawn && Application.isPlaying)
        {
            for (float x = -gridDrawExtent; x <= gridDrawExtent; x += gridSize)
            {
                Vector3 startPoint = new Vector3(x, 0, -gridDrawExtent);
                Vector3 endPoint = new Vector3(x, 0, gridDrawExtent);
                CreateLine(startPoint, endPoint);
            }

            for (float z = -gridDrawExtent; z <= gridDrawExtent; z += gridSize)
            {
                Vector3 startPoint = new Vector3(-gridDrawExtent, 0, z);
                Vector3 endPoint = new Vector3(gridDrawExtent, 0, z);
                CreateLine(startPoint, endPoint);
            }
            gridDrawn = true;
        }
    }

    private void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject line = new GameObject("GridLine");
        line.transform.position = start;
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    // ������ ��ġ�� ���ڿ� �°� �����ϴ� �Լ�
    Vector3 SnapToGrid(Vector3 originalPosition)
    {
        float x = Mathf.Round(originalPosition.x / gridSize) * gridSize + (gridSize / 2);
        float y = Mathf.Round(originalPosition.y / gridSize) * gridSize + (gridSize / 2);
        float z = Mathf.Round(originalPosition.z / gridSize) * gridSize + (gridSize / 2);

        return new Vector3(x, y, z);
    }

}