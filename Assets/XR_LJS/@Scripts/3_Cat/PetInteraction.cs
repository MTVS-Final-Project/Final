using TMPro;
using UnityEngine;
using System.Collections;

public class PetInteraction : MonoBehaviour
{
    private Vector3 initialMousePosition;
    public GameObject head; // 자식 오브젝트 머리
    public GameObject body; // 자식 오브젝트 몸통
    public TMP_Text text;
    public GameObject whiteImage; // Canvas의 하얀색 이미지 오브젝트

    void Start()
    {
        whiteImage.SetActive(false); // 처음에 하얀색 이미지 비활성화
    }

    void OnMouseDown() // 고양이 클릭 시
    {
        initialMousePosition = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        float dragDistance = Vector3.Distance(initialMousePosition, currentMousePosition);

        if (dragDistance > 50.0f) // 쓰다듬기 거리 기준
        {
            if (gameObject == head)
            {
                text.text = "고양이 머리를 쓰다듬었습니다.";
                ReactToPetting("head");
            }
            else if (gameObject == body)
            {
                text.text = "고양이 엉덩이를 쓰다듬었습니다.";
                ReactToPetting("body");
            }
            ShowWhiteImage(); // 하얀색 이미지 활성화 및 2초 뒤에 비활성화
        }
    }

    void ReactToPetting(string part)
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
                text.text = "고양이가 머리를 쓰다듬어 긍정적으로 반응합니다.";
            }
            else if (part == "body")
            {
                text.text = "고양이가 엉덩이를 쓰다듬는 것을 싫어하며 도망갑니다.";

                // 전체 고양이를 Y축으로 1만큼 이동
                Vector3 newPosition = transform.parent.position + new Vector3(0, 1, 0);
                StartCoroutine(MoveCat(newPosition));
            }
            ShowWhiteImage(); // 하얀색 이미지 활성화 및 2초 뒤에 비활성화
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

    private IEnumerator MoveCat(Vector3 targetPosition)
    {
        float duration = 0.5f; // 이동하는 데 걸리는 시간
        float elapsed = 0f;
        Vector3 startingPosition = transform.parent.position; // 부모 오브젝트 (고양이 전체)의 현재 위치 저장

        while (elapsed < duration)
        {
            transform.parent.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime; // 경과 시간 업데이트
            yield return null; // 다음 프레임까지 대기
        }

        transform.parent.position = targetPosition; // 최종 위치 설정
    }
}
