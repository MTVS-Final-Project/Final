using TMPro;
using UnityEngine;
using System.Collections;

public class PetInteraction : MonoBehaviour
{
    private Vector3 initialMousePosition;
    public GameObject head; // �ڽ� ������Ʈ �Ӹ�
    public GameObject body; // �ڽ� ������Ʈ ����
    public TMP_Text text;
    public GameObject whiteImageFriendly; // Friendly �̹���
    public GameObject whiteImagePicky; // Picky �̹���

    void Start()
    {
        whiteImageFriendly.SetActive(false); // ó���� Friendly �̹��� ��Ȱ��ȭ
        whiteImagePicky.SetActive(false); // ó���� Picky �̹��� ��Ȱ��ȭ
    }

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
            ShowWhiteImage(); // �Ͼ�� �̹��� Ȱ��ȭ �� 2�� �ڿ� ��Ȱ��ȭ
        }
    }

    void ReactToPetting(string part)
    {
        CatBehavior catBehavior = GetComponentInParent<CatBehavior>();

        // ��� �̹��� ��Ȱ��ȭ
        whiteImageFriendly.SetActive(false);
        whiteImagePicky.SetActive(false);

        if (catBehavior.catPersonality == CatBehavior.CatPersonality.Friendly)
        {
            text.text = "����̰� ���������� �����մϴ�!";
            whiteImageFriendly.SetActive(true); // Friendly �̹��� Ȱ��ȭ
        }
        else if (catBehavior.catPersonality == CatBehavior.CatPersonality.Picky)
        {
            if (part == "head")
            {
                text.text = "����̰� �Ӹ��� ���ٵ�� ���������� �����մϴ�.";
                whiteImagePicky.SetActive(true); // Picky �̹��� Ȱ��ȭ
            }
            else if (part == "body")
            {
                text.text = "����̰� �����̸� ���ٵ�� ���� �Ⱦ��ϸ� �������ϴ�.";
                whiteImagePicky.SetActive(true);
                // ��ü ����̸� Y������ 1��ŭ �̵�
                Vector3 newPosition = transform.parent.position + new Vector3(1, 1, 0);
                StartCoroutine(MoveCat(newPosition));
            }
            ShowWhiteImage(); // �Ͼ�� �̹��� Ȱ��ȭ �� 2�� �ڿ� ��Ȱ��ȭ
        }
    }

    private void ShowWhiteImage()
    {
        // �̰��� ���� �Ͼ�� �̹����� �����ִ� ����Դϴ�.
        StartCoroutine(HideImage()); // 2�� �Ŀ� ��Ȱ��ȭ
    }

    private IEnumerator HideImage()
    {
        yield return new WaitForSeconds(2); // 2�� ���
        whiteImageFriendly.SetActive(false); // Friendly �̹��� ��Ȱ��ȭ
        whiteImagePicky.SetActive(false); // Picky �̹��� ��Ȱ��ȭ
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
