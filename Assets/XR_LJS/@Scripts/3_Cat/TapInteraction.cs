using TMPro;
using UnityEngine;

public class TapInteraction : MonoBehaviour
{
    public GameObject head; // �ڽ� ������Ʈ �Ӹ�
    public GameObject body; // �ڽ� ������Ʈ ����
    public TMP_Text text;

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
                    ReactToTapping("head");
                }
                else if (hit.collider.gameObject == body)
                {
                    text.text = "����� �����̸� ���� �ƽ��ϴ�.";
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
                // ��ü ����̸� Y������ 5��ŭ �̵�
                Vector3 newPosition = transform.parent.position + new Vector3(0, 5, 0);
            }
            else if (part == "body")
            {
                text.text = "����̰� �����̸� ���� ġ�� ���� ���ϴ�!";
            }
        }
    }
}

