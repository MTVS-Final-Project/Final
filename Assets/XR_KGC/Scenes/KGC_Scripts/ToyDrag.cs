using UnityEngine;

public class ToyDrag : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 initialPosition;
    private float offsetX;
    public float totalDistanceMoved = 0f; // 총 이동 거리 계산을 위한 변수

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

            // 드래그가 끝났을 때 총 이동 거리 출력
            Debug.Log("총 이동 거리: " + totalDistanceMoved);
            totalDistanceMoved = 0f; // 이동 거리 초기화
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

                // 드래그가 끝났을 때 총 이동 거리 출력
                Debug.Log("총 이동 거리: " + totalDistanceMoved);
                totalDistanceMoved = 0f; // 이동 거리 초기화
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
            initialPosition = transform.position; // 오브젝트의 현재 위치 저장
            Vector3 hitPoint = mainCamera.WorldToScreenPoint(transform.position);
            offsetX = inputPosition.x - hitPoint.x; // 클릭 지점과 오브젝트 간 X축 오프셋 저장
        }
    }

    private void DragObjectTo(Vector3 inputPosition)
    {
        Vector3 screenPoint = new Vector3(inputPosition.x - offsetX, 0, mainCamera.WorldToScreenPoint(transform.position).z);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPoint);

        // 카메라의 오른쪽 방향을 따라 이동
        Vector3 targetPosition = initialPosition + mainCamera.transform.right * (worldPosition.x - initialPosition.x);

        // X축을 화면 기준으로만 이동하고 Y와 Z는 고정
        Vector3 previousPosition = transform.position;
        transform.position = new Vector3(targetPosition.x, initialPosition.y, targetPosition.z);

        // 이동 거리를 누적하여 계산
        totalDistanceMoved += Vector3.Distance(previousPosition, transform.position);
    }
}
