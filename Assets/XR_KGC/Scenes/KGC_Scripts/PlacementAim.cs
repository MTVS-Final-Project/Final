using UnityEngine;

public class PlacementAim : MonoBehaviour
{
    // UI�� RectTransform�� ����
    public RectTransform uiElement;
    public Camera mainCamera;
    private LineRenderer lineRenderer;

    void Start()
    {
        // ���� ī�޶� �Ҵ� (�ʿ�� �ν����Ϳ��� ���� �Ҵ� ����)
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // LineRenderer ������Ʈ ��������
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    void Update()
    {
        if (mainCamera == null || lineRenderer == null || uiElement == null)
        {
            Debug.LogError("Main camera, LineRenderer, or UI element is not assigned.");
            return;
        }

        // UI ������Ʈ�� ��ũ�� ��ġ ��������
        Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(mainCamera, uiElement.position);

        // ��ũ�� ��ġ�� �������� ���� ��ġ�� ��ȯ
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        // ���̸� �ð������� �׸���
        lineRenderer.SetPosition(0, ray.origin);

        // ����ĳ��Ʈ�� �߻��Ͽ� �浹�� ������Ʈ�� �ִ��� Ȯ��
        if (Physics.Raycast(ray, out hit))
        {
            // �浹�� ���� ������Ʈ�� �̸� ���
            Debug.Log("Hit GameObject: " + hit.collider.gameObject.name);
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            Debug.Log("No GameObject hit.");
            lineRenderer.SetPosition(1, ray.origin + ray.direction * 100);
        }
    }
}
