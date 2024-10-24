using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragMove : MonoBehaviour, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    private void Start()
    {
        // UI ��ư�� RectTransform ��������
        rectTransform = GetComponent<RectTransform>();
        // UI�� �׷����� Canvas ��������
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ĵ������ ������ ���� �巡�� ��� ����
        if (canvas == null)
            return;

        // ȭ�� ��ġ�� ĵ���� ��ǥ��� ��ȯ
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out canvasPosition
        );

        // ��ư�� ��ġ ������Ʈ
        rectTransform.anchoredPosition = canvasPosition;
    }
}
