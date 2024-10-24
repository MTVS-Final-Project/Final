using UnityEngine;

public class PlacementAim : MonoBehaviour
{
    // UI의 RectTransform을 참조
    public RectTransform uiElement;
    public Camera mainCamera;
    private LineRenderer lineRenderer;

    void Start()
    {
        // 메인 카메라를 할당 (필요시 인스펙터에서 직접 할당 가능)
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // LineRenderer 컴포넌트 가져오기
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

        // UI 오브젝트의 스크린 위치 가져오기
        Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(mainCamera, uiElement.position);

        // 스크린 위치를 기준으로 월드 위치로 변환
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        // 레이를 시각적으로 그리기
        lineRenderer.SetPosition(0, ray.origin);

        // 레이캐스트를 발사하여 충돌한 오브젝트가 있는지 확인
        if (Physics.Raycast(ray, out hit))
        {
            // 충돌한 게임 오브젝트의 이름 출력
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
