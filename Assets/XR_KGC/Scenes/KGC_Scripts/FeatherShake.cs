using UnityEngine;
using UnityEngine.EventSystems;

public class FeatherShake : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private float initialX; // 드래그 시작 시 X 좌표
    private float rotationFactor = 0.1f; // 회전 민감도
    public float totalRotationZ = 0f; // 누적된 Z 축 회전 값의 절대값
    private float currentRotationZ = 0f; // 현재 Z 축 회전 값 (화면 상에서 적용되는 회전 값)

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 시작 시 X 위치 초기화
        initialX = eventData.position.x;
    }

    private void Update()
    {
        if (totalRotationZ > 360)
        {
            GameObject.Find("Cat").GetComponent<CatTemp>().CatAction();
            totalRotationZ = 0;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // X 이동 거리를 계산하여 회전 값으로 변환
        float deltaX = eventData.position.x - initialX;

        // 현재 프레임에서의 회전값 계산
        currentRotationZ = deltaX * rotationFactor;

        // 오브젝트의 Z축 회전 적용
        rectTransform.localRotation = Quaternion.Euler(0, 0, -currentRotationZ);

        // 누적된 회전 각도는 절대값으로 누적
        totalRotationZ += Mathf.Abs(currentRotationZ);

        // 초기 X 위치 갱신
        initialX = eventData.position.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 종료 시 회전 초기화
        currentRotationZ = 0f;
        rectTransform.localRotation = Quaternion.Euler(0, 0, 0);

        // 드래그가 끝난 후 누적 회전각도 확인
        Debug.Log("총 누적 회전 각도: " + totalRotationZ);
        totalRotationZ = 0f; // 누적 회전 초기화
    }
}
