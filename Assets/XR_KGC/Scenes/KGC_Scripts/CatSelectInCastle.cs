using Unity.VisualScripting;
using UnityEngine;

public class CatSelectInCastle : MonoBehaviour
{
    public Camera cam;

    public GameObject catCanvas;
    public bool zoom = false;
    private float zoomSpeed = 5f; // 줌 애니메이션 속도
    public GameObject selectedCat; // 선택된 Cat 오브젝트를 저장
    public GameObject backButton;
    public Vector3 initPos =new Vector3(0,1,-10);
    public CatData catData;

    public int count = 1; //한번은 무조건 도망감.

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        backButton.SetActive(false);
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

        // 줌 동작이 활성화되었을 때
        if (zoom && selectedCat != null)
        {
            // 카메라 사이즈를 2로 점진적으로 줄이기
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 2f, Time.deltaTime * zoomSpeed);

            // 선택된 Cat 오브젝트의 X, Y를 따라가게 설정
            Vector3 targetPosition = new Vector3(selectedCat.transform.position.x, selectedCat.transform.position.y, cam.transform.position.z);
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, Time.deltaTime * zoomSpeed);
        }
        if (!zoom)
        {
            catCanvas.SetActive(false);
        }
        else
        {
            catCanvas.SetActive(true);
        }
    }

    // "Cat" 선택 후 처리할 로직
    void HandleCatSelection(GameObject catObject)
    {
        Debug.Log("Selected Cat: " + catObject.name);

        // 선택된 Cat 오브젝트 저장 및 줌 활성화
        selectedCat = catObject;
        zoom = true;
        backButton.SetActive(true);
    }

    public void ExitZoom()
    {
        zoom = false;
        cam.orthographicSize =5;
        backButton.SetActive(false);
        cam.transform.position = initPos;
    }

    public void Interaction()
    {
        if (count > 0)
        {
            ExitZoom();
        }
        else
        {
            //catData
        }
    }
}
