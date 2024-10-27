using TMPro;
using UnityEngine;
using System.Collections;

public class TapInteraction : MonoBehaviour
{
    public GameObject head; // 자식 오브젝트 머리
    public GameObject body; // 자식 오브젝트 몸통
    public TMP_Text text;
    public GameObject whiteImage; // Canvas의 하얀색 이미지 오브젝트

    private void Start()
    {
        whiteImage.SetActive(false); // 처음에 하얀색 이미지 비활성화
    }

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
                    ShowWhiteImage(); // 하얀색 이미지 활성화 및 2초 뒤에 비활성화
                    ReactToTapping("head");
                }
                else if (hit.collider.gameObject == body)
                {
                    text.text = "고양이 엉덩이를 툭툭 쳤습니다.";
                    ShowWhiteImage(); // 하얀색 이미지 활성화 및 2초 뒤에 비활성화
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
                ShowWhiteImage(); // 하얀색 이미지 활성화 및 2초 뒤에 비활성화
                // 전체 고양이를 Y축으로 5만큼 이동
                Vector3 newPosition = transform.parent.position + new Vector3(0, 5, 0);
                transform.parent.position = newPosition;
            }
            else if (part == "body")
            {
                text.text = "고양이가 엉덩이를 툭툭 치는 것을 즐깁니다!";
                ShowWhiteImage(); // 하얀색 이미지 활성화 및 2초 뒤에 비활성화
            }
        }
    }

    private void ShowWhiteImage()
    {
        whiteImage.SetActive(true); // 하얀색 이미지 활성화
        StartCoroutine(HideImage()); // 2초 후에 비활성화
    }

    private IEnumerator HideImage()
    {
        yield return new WaitForSeconds(2); // 2초 대기
        whiteImage.SetActive(false); // 하얀색 이미지 비활성화
    }
}
