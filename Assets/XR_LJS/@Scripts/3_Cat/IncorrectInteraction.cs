using TMPro;
using UnityEngine;

public class IncorrectInteraction : MonoBehaviour
{
    public GameObject head; // �ڽ� ������Ʈ �Ӹ�
    public GameObject body; // �ڽ� ������Ʈ ����
    public TMP_Text text;

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
            }
            else if (part == "body")
            {
                text.text = "����̰� �����̸� �ǵ帮�� ���� �Ⱦ��ϸ� �������ϴ�!";
            }
        }
    }
}
