using UnityEngine;

public class ToyDrag : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 initialPosition;
    private float offsetX;
    public float totalDistanceMoved = 0f; // �� �̵� �Ÿ� ����� ���� ����

    public Transform spawnpoint;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            DragObjectTo(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            // �巡�װ� ������ �� �� �̵� �Ÿ� ���
            Debug.Log("�� �̵� �Ÿ�: " + totalDistanceMoved);
            totalDistanceMoved = 0f; // �̵� �Ÿ� �ʱ�ȭ
            transform.position = spawnpoint.position;
        }
#else
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

                // �巡�װ� ������ �� �� �̵� �Ÿ� ���
                Debug.Log("�� �̵� �Ÿ�: " + totalDistanceMoved);
                totalDistanceMoved = 0f; // �̵� �Ÿ� �ʱ�ȭ
                transform.position = spawnpoint.position;
            }
        }
#endif


        if (totalDistanceMoved > 1)
        {
            GameObject cat = GameObject.Find("Cat");
            if (cat != null)
            {
                cat.GetComponent<CatTemp>().CatAction();
            }
        }
    }

    private void HandleInput(Vector3 inputPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(inputPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider != null && hit.collider.gameObject == gameObject)
        {
            isDragging = true;
            initialPosition = transform.position; // ������Ʈ�� ���� ��ġ ����
            Vector3 hitPoint = mainCamera.WorldToScreenPoint(transform.position);
            offsetX = inputPosition.x - hitPoint.x; // Ŭ�� ������ ������Ʈ �� X�� ������ ����
        }
    }

    private void DragObjectTo(Vector3 inputPosition)
    {
        Vector3 screenPoint = new Vector3(inputPosition.x - offsetX, 0, mainCamera.WorldToScreenPoint(transform.position).z);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPoint);

        // ī�޶��� ������ ������ ���� �̵�
        Vector3 targetPosition = initialPosition + mainCamera.transform.right * (worldPosition.x - initialPosition.x);

        // X���� ȭ�� �������θ� �̵��ϰ� Y�� Z�� ����
        Vector3 previousPosition = transform.position;
        transform.position = new Vector3(targetPosition.x, initialPosition.y, targetPosition.z);

        // �̵� �Ÿ��� �����Ͽ� ���
        totalDistanceMoved += Vector3.Distance(previousPosition, transform.position);
    }
}
