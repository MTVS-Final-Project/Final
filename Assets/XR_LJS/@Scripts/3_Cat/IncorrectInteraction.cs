using TMPro;
using UnityEngine;
using System.Collections;

public class IncorrectInteraction : MonoBehaviour
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

    void OnMouseDown()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == head)
            {
                ReactToIncorrectInteraction("head");
            }
            else if (hit.collider.gameObject == body)
            {
                ReactToIncorrectInteraction("body");
            }
        }
    }

    void ReactToIncorrectInteraction(string part)
    {
        CatBehavior catBehavior = GetComponentInParent<CatBehavior>();

        if (catBehavior.catPersonality == CatBehavior.CatPersonality.Picky)
        {
            if (part == "head")
            {
                text.text = "����̰� �Ӹ��� �ǵ帮�� ���� �Ⱦ��ϸ� �ָ� �������ϴ�!";
                whiteImageFriendly.SetActive(true); // Friendly �̹��� Ȱ��ȭ
                MoveCatUp(); // ����� �̵�
            }
            else if (part == "body")
            {
                text.text = "����̰� �����̸� �ǵ帮�� ���� �Ⱦ��ϸ� �������ϴ�!";
                whiteImagePicky.SetActive(true);
                MoveCatUp(); // ����� �̵�
            }
        }
        ShowWhiteImage(); // �Ͼ�� �̹��� Ȱ��ȭ �� 2�� �� ��Ȱ��ȭ
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

    private void MoveCatUp()
    {
        // �θ� ������Ʈ�� Y���� 5��ŭ �̵�
        Vector3 newPosition = transform.parent.position + new Vector3(-5, 5, 0);
        transform.parent.position = newPosition;
    }
}
