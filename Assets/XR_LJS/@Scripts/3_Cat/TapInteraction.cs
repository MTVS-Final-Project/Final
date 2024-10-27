using TMPro;
using UnityEngine;
using System.Collections;

public class TapInteraction : MonoBehaviour
{
    public GameObject head; // �ڽ� ������Ʈ �Ӹ�
    public GameObject body; // �ڽ� ������Ʈ ����
    public TMP_Text text;
    public GameObject whiteImage; // Canvas�� �Ͼ�� �̹��� ������Ʈ

    private void Start()
    {
        whiteImage.SetActive(false); // ó���� �Ͼ�� �̹��� ��Ȱ��ȭ
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ȭ�� �ε帮��
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == head)
                {
                    text.text = "����� �Ӹ��� ���� �ƽ��ϴ�.";
                    ShowWhiteImage(); // �Ͼ�� �̹��� Ȱ��ȭ �� 2�� �ڿ� ��Ȱ��ȭ
                    ReactToTapping("head");
                }
                else if (hit.collider.gameObject == body)
                {
                    text.text = "����� �����̸� ���� �ƽ��ϴ�.";
                    ShowWhiteImage(); // �Ͼ�� �̹��� Ȱ��ȭ �� 2�� �ڿ� ��Ȱ��ȭ
                    ReactToTapping("body");
                }
            }
        }
    }

    void ReactToTapping(string part)
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
                text.text = "����̰� �Ӹ��� ���� ġ�� ���� �Ⱦ��ϸ� �������ϴ�!";
                ShowWhiteImage(); // �Ͼ�� �̹��� Ȱ��ȭ �� 2�� �ڿ� ��Ȱ��ȭ
                // ��ü ����̸� Y������ 5��ŭ �̵�
                Vector3 newPosition = transform.parent.position + new Vector3(0, 5, 0);
                transform.parent.position = newPosition;
            }
            else if (part == "body")
            {
                text.text = "����̰� �����̸� ���� ġ�� ���� ���ϴ�!";
                ShowWhiteImage(); // �Ͼ�� �̹��� Ȱ��ȭ �� 2�� �ڿ� ��Ȱ��ȭ
            }
        }
    }

    private void ShowWhiteImage()
    {
        whiteImage.SetActive(true); // �Ͼ�� �̹��� Ȱ��ȭ
        StartCoroutine(HideImage()); // 2�� �Ŀ� ��Ȱ��ȭ
    }

    private IEnumerator HideImage()
    {
        yield return new WaitForSeconds(2); // 2�� ���
        whiteImage.SetActive(false); // �Ͼ�� �̹��� ��Ȱ��ȭ
    }
}
