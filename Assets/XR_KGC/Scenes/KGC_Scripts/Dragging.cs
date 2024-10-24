using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragMove : MonoBehaviour, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    private void Start()
    {
        // UI 버튼의 RectTransform 가져오기
        rectTransform = GetComponent<RectTransform>();
        // UI가 그려지는 Canvas 가져오기
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 캔버스가 존재할 때만 드래그 기능 실행
        if (canvas == null)
            return;

        // 화면 위치를 캔버스 좌표계로 변환
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out canvasPosition
        );

        // 버튼의 위치 업데이트
        rectTransform.anchoredPosition = canvasPosition;
    }
}
