using TMPro;
using UnityEngine;
using System.Collections;

public class IncorrectInteraction : MonoBehaviour
{
    public GameObject head; // 자식 오브젝트 머리
    public GameObject body; // 자식 오브젝트 몸통
    public TMP_Text text;
    public GameObject whiteImageFriendly; // Friendly 이미지
    public GameObject whiteImagePicky; // Picky 이미지
    private void Start()
    {
        whiteImageFriendly.SetActive(false); // 처음에 Friendly 이미지 비활성화
        whiteImagePicky.SetActive(false); // 처음에 Picky 이미지 비활성화
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
                text.text = "고양이가 머리를 건드리는 것을 싫어하며 멀리 도망갑니다!";
                whiteImageFriendly.SetActive(true); // Friendly 이미지 활성화
                MoveCatUp(); // 고양이 이동
            }
            else if (part == "body")
            {
                text.text = "고양이가 엉덩이를 건드리는 것을 싫어하며 도망갑니다!";
                whiteImagePicky.SetActive(true);
                MoveCatUp(); // 고양이 이동
            }
        }
        ShowWhiteImage(); // 하얀색 이미지 활성화 및 2초 후 비활성화
    }

    private void ShowWhiteImage()
    {
        // 이곳은 현재 하얀색 이미지를 보여주는 기능입니다.
        StartCoroutine(HideImage()); // 2초 후에 비활성화
    }

    private IEnumerator HideImage()
    {
        yield return new WaitForSeconds(2); // 2초 대기
        whiteImageFriendly.SetActive(false); // Friendly 이미지 비활성화
        whiteImagePicky.SetActive(false); // Picky 이미지 비활성화
    }

    private void MoveCatUp()
    {
        // 부모 오브젝트의 Y축을 5만큼 이동
        Vector3 newPosition = transform.parent.position + new Vector3(-5, 5, 0);
        transform.parent.position = newPosition;
    }
}
