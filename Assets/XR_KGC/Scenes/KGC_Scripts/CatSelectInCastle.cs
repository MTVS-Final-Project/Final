using UnityEngine;

public class CatSelectInCastle : MonoBehaviour
{
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 확인
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 레이캐스트로 감지된 오브젝트 체크
            if (Physics.Raycast(ray, out hit))
            {
                // 감지된 오브젝트의 이름이 "Cat"인지 확인
                if (hit.collider.gameObject.name == "Cat")
                {
                    Debug.Log("Cat detected: " + hit.collider.gameObject.name);
                    HandleCatSelection(hit.collider.gameObject);
                }
            }
        }
    }

    // "Cat" 선택 후 처리할 로직
    void HandleCatSelection(GameObject catObject)
    {
        // 여기에 "Cat"을 선택했을 때의 행동 작성
        Debug.Log("Selected Cat: " + catObject.name);
    }
}
