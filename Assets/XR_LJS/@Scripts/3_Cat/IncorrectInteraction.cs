using TMPro;
using UnityEngine;

public class IncorrectInteraction : MonoBehaviour
{
    public GameObject head; // 자식 오브젝트 머리
    public GameObject body; // 자식 오브젝트 몸통
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
                text.text = "고양이가 머리를 건드리는 것을 싫어하며 멀리 도망갑니다!";
            }
            else if (part == "body")
            {
                text.text = "고양이가 엉덩이를 건드리는 것을 싫어하며 도망갑니다!";
            }
        }
    }
}
