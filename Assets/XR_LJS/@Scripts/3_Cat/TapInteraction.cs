using TMPro;
using UnityEngine;
using System.Collections;

public class TapInteraction : MonoBehaviour
{
    public GameObject head; // �ڽ� ������Ʈ �Ӹ�
    public GameObject body; // �ڽ� ������Ʈ ����
    public TMP_Text text;
    public GameObject whiteImageFriendly; // Friendly �̹���
    public GameObject whiteImagePicky; // Picky �̹���
    private void Start()
    {
        whiteImageFriendly.SetActive(false); // ó���� Friendly �̹��� ��Ȱ��ȭ
        whiteImagePicky.SetActive(false); // ó���� Picky �̹��� ��Ȱ��ȭ
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
                    whiteImagePicky.SetActive(true);
                    ReactToTapping("head");
                }
                else if (hit.collider.gameObject == body)
                {
                    text.text = "����� �����̸� ���� �ƽ��ϴ�.";
                    whiteImageFriendly.SetActive(true); // Friendly �̹��� Ȱ��ȭ
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
            whiteImageFriendly.SetActive(true); // Friendly �̹��� Ȱ��ȭ
        }
        else if (catBehavior.catPersonality == CatBehavior.CatPersonality.Picky)
        {
            if (part == "head")
            {
                text.text = "����̰� �Ӹ��� ���� ġ�� ���� �Ⱦ��ϸ� �������ϴ�!";
                whiteImagePicky.SetActive(true);
                // ��ü ����̸� Y������ 5��ŭ �̵�
                Vector3 newPosition = transform.parent.position + new Vector3(-5, 5, 0);
                transform.parent.position = newPosition;
            }
            else if (part == "body")
            {
                text.text = "����̰� �����̸� ���� ġ�� ���� ���ϴ�!";
                ShowWhiteImage(); // �Ͼ�� �̹��� Ȱ��ȭ �� 2�� �ڿ� ��Ȱ��ȭ
            }
            ShowWhiteImage();
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
}
