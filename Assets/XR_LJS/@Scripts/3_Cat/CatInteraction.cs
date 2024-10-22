using UnityEngine;
using TMPro; // TextMeshPro ���ӽ����̽� �߰�

public class CatInteraction : MonoBehaviour
{
    public Transform player; // ĳ������ Transform
    public Transform cat; // ������� Transform
    public GameObject catHead; // ����� �Ӹ� �κ�
    public GameObject catButt; // ����� ������ �κ�

    // TextMeshProUGUI ������Ʈ ���� (UI �ؽ�Ʈ�� ����� TMP �ؽ�Ʈ ����)
    public TMP_Text interactionText;

    private float originalCatScale = 1f; // ���� ����� ũ��
    private float enlargedCatScale = 1.5f; // Ȯ��� ����� ũ��
    private float originalCameraSize = 5f; // ���� ī�޶� ������
    private float enlargedCameraSize = 3f; // Ȯ��� ī�޶� ������

    // ĳ���Ϳ� ����� ���� ��ȣ�ۿ� ��� �Ÿ�
    private float interactionDistance = 1f;

    void Update()
    {
        // ĳ���Ϳ� ����� �� �Ÿ� ���
        float distance = Vector2.Distance(cat.position, player.position);

        // ����̿� ĳ���Ͱ� ����� ���
        if (distance <= interactionDistance)
        {
            // ����̿� ī�޶� ���ÿ� Ȯ��
            cat.localScale = Vector3.Lerp(cat.localScale, new Vector3(enlargedCatScale, enlargedCatScale, 1f), Time.deltaTime * 2f);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, enlargedCameraSize, Time.deltaTime * 2f);

            // ����̿� ����� ���� ��ġ ��ȣ�ۿ� ���
            HandleTouchInput();
        }
        else
        {
            // ����̿� ī�޶� ���� ũ��� ����
            cat.localScale = Vector3.Lerp(cat.localScale, new Vector3(originalCatScale, originalCatScale, 1f), Time.deltaTime * 2f);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, originalCameraSize, Time.deltaTime * 2f);

            // ����̿� ����� ���� ��ġ ��ȣ�ۿ� ���
            ShowTextBox("");  // ��ġ ���� �ؽ�Ʈ �ʱ�ȭ
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0) // �ϳ� �̻��� ��ġ�� ���� ��
        {
            Touch touch = Input.GetTouch(0); // ù ��° ��ġ �Է� ��������

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position); // ��ġ ��ġ�� ���� ��ǥ�� ��ȯ
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject == catHead) // ����� �Ӹ��� �浹 ����
                    {
                        ShowTextBox("�����: �Ӹ��� ������!");
                    }
                    else if (hit.collider.gameObject == catButt) // ����� �����̿� �浹 ����
                    {
                        ShowTextBox("�����: �����̸� ������!");
                        ChangeCatPose(); // ����� �ڼ� ����
                    }
                }
            }
        }
    }

    private void ShowTextBox(string message)
    {
        // TextMeshPro �ؽ�Ʈ ������Ʈ
        if (interactionText != null)
        {
            interactionText.text = message; // TMP �ؽ�Ʈ �ʵ带 ������Ʈ
        }
        else
        {
            Debug.LogWarning("interactionText�� �������� �ʾҽ��ϴ�!"); // TMP �ؽ�Ʈ�� null�� ��� ��� ���
        }
    }

    private void ChangeCatPose()
    {
        // ����� �ڼ� ���� ����
        Debug.Log("����� �ڼ��� �ٲ����!"); // �ڼ� ������ ���� (����� ����� �޽���)
    }
}
