using TMPro;
using UnityEngine;
using System.Collections;
public class PetInteraction : MonoBehaviour
{
    private Vector3 initialMousePosition;
    public GameObject head; // �ڽ� ������Ʈ �Ӹ�
    public GameObject body; // �ڽ� ������Ʈ ����
    public TMP_Text text;

    void OnMouseDown() // ����� Ŭ�� ��
    {
        initialMousePosition = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        float dragDistance = Vector3.Distance(initialMousePosition, currentMousePosition);

        if (dragDistance > 50.0f) // ���ٵ�� �Ÿ� ����
        {
            if (gameObject == head)
            {
                text.text = "����� �Ӹ��� ���ٵ�����ϴ�.";
                ReactToPetting("head");
            }
            else if (gameObject == body)
            {
                text.text = "����� �����̸� ���ٵ�����ϴ�.";
                ReactToPetting("body");
            }
        }
    }

    void ReactToPetting(string part)
    {
        CatBehavior catBehavior = GetComponentInParent<CatBehavior>();

        if (catBehavior.catPersonality == CatBehavior.CatPersonality.Friendly)
        {
            text.text = "����̰� ���������� �����մϴ�!";
        }
        else if (catBehavior.catPersonality == CatBehavior.CatPersonality.Picky)
        {
            if (part == "head")
            {
                text.text = "����̰� �Ӹ��� ���ٵ�� ���������� �����մϴ�.";
            }
            else if (part == "body")
            {
                text.text = "����̰� �������� ���ٵ�� ���� �Ⱦ��ϸ� �������ϴ�.";

                // ��ü ����̸� Y������ 1��ŭ �̵�
                Vector3 newPosition = transform.parent.position + new Vector3(0, 1, 0);
                StartCoroutine(MoveCat(newPosition));
            }
        }
    }

    private IEnumerator MoveCat(Vector3 targetPosition)
    {
        float duration = 0.5f; // �̵��ϴ� �� �ɸ��� �ð�
        float elapsed = 0f;
        Vector3 startingPosition = transform.parent.position; // �θ� ������Ʈ (����� ��ü)�� ���� ��ġ ����

        while (elapsed < duration)
        {
            transform.parent.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime; // ��� �ð� ������Ʈ
            yield return null; // ���� �����ӱ��� ���
        }

        transform.parent.position = targetPosition; // ���� ��ġ ����
    }
}
