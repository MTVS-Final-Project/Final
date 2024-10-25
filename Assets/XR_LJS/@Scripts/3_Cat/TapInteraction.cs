using TMPro;
using UnityEngine;

public class TapInteraction : MonoBehaviour
{
    public GameObject head; // 자식 오브젝트 머리
    public GameObject body; // 자식 오브젝트 몸통
    public TMP_Text text;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 화면 두드리기
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == head)
                {
                    text.text = "고양이 머리를 툭툭 쳤습니다.";
                    ReactToTapping("head");
                }
                else if (hit.collider.gameObject == body)
                {
                    text.text = "고양이 엉덩이를 툭툭 쳤습니다.";
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
            text.text = "고양이가 긍정적으로 반응합니다!";
        }
        else if (catBehavior.catPersonality == CatBehavior.CatPersonality.Picky)
        {
            if (part == "head")
            {
                text.text = "고양이가 머리를 툭툭 치는 것을 싫어하며 도망갑니다!";
                // 전체 고양이를 Y축으로 5만큼 이동
                Vector3 newPosition = transform.parent.position + new Vector3(0, 5, 0);
            }
            else if (part == "body")
            {
                text.text = "고양이가 엉덩이를 툭툭 치는 것을 즐깁니다!";
            }
        }
    }
}

