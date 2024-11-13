using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Camera mainCamera;
    public bool isDragging = false;
    private Vector3 offset;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        // ��ġ �Ǵ� ���콺 �Է� ����
#if UNITY_EDITOR
        // ���콺 �Է� ó�� (������ �׽�Ʈ ��)
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0)||!isDragging)
        {
            isDragging = false;
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            DragObjectTo(Input.mousePosition);
        }
#else
        // ��ġ �Է� ó�� (����� ��)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleInput(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                DragObjectTo(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
#endif
    }

    private void HandleInput(Vector3 inputPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(inputPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                offset = hit.point - transform.position;
            }
            else
            {
                isDragging = false;
            }
        }
    }

    private void DragObjectTo(Vector3 inputPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(inputPosition);
        Plane dragPlane = new Plane(Vector3.forward, transform.position); // Z������ ������ ���
        float distance;

        if (dragPlane.Raycast(ray, out distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance) - offset;
            transform.position = new Vector3(targetPoint.x, targetPoint.y, transform.position.z); // Z�� ����
        }
    }
}
