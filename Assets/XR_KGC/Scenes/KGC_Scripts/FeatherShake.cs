using UnityEngine;
using UnityEngine.EventSystems;

public class FeatherShake : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private float initialX; // �巡�� ���� �� X ��ǥ
    private float rotationFactor = 0.1f; // ȸ�� �ΰ���
    public float totalRotationZ = 0f; // ������ Z �� ȸ�� ���� ���밪
    private float currentRotationZ = 0f; // ���� Z �� ȸ�� �� (ȭ�� �󿡼� ����Ǵ� ȸ�� ��)

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // �巡�� ���� �� X ��ġ �ʱ�ȭ
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
        // X �̵� �Ÿ��� ����Ͽ� ȸ�� ������ ��ȯ
        float deltaX = eventData.position.x - initialX;

        // ���� �����ӿ����� ȸ���� ���
        currentRotationZ = deltaX * rotationFactor;

        // ������Ʈ�� Z�� ȸ�� ����
        rectTransform.localRotation = Quaternion.Euler(0, 0, -currentRotationZ);

        // ������ ȸ�� ������ ���밪���� ����
        totalRotationZ += Mathf.Abs(currentRotationZ);

        // �ʱ� X ��ġ ����
        initialX = eventData.position.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�� ���� �� ȸ�� �ʱ�ȭ
        currentRotationZ = 0f;
        rectTransform.localRotation = Quaternion.Euler(0, 0, 0);

        // �巡�װ� ���� �� ���� ȸ������ Ȯ��
        Debug.Log("�� ���� ȸ�� ����: " + totalRotationZ);
        totalRotationZ = 0f; // ���� ȸ�� �ʱ�ȭ
    }
}
