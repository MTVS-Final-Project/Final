using UnityEngine;

public class ToiletZoom : MonoBehaviour
{
    public GameObject RoomModify; //방 수정중이 아닐때만 작동하게
    public GameObject toilet;

    public GameObject player;

    public ParentManager pm;

    public int quantity = 0;//얼마나 많이 쌓였는지.
    //이 캔버스안의 모든 게임오브잭트가 비활성화일때만 작동하게.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pm = GameObject.Find("PopupCanvas").GetComponent<ParentManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
           player = GameObject.Find("Player");
        }

        if (RoomModify == null)
        {
            RoomModify = GameObject.Find("RoomModify");
        }
        if (toilet == null)
        {
            toilet = GameObject.Find("PopupCanvas").transform.GetChild(0).gameObject;
        }


        if (!RoomModify.activeInHierarchy&&pm.allDeactivated)
        {

        if (Input.GetMouseButtonDown(0))
        {
            // 클릭한 위치에서 화면 좌표를 통해 레이 생성
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 레이캐스트를 통해 3D 콜라이더를 감지
            if (Physics.Raycast(ray, out hit))
            {
                // 충돌한 오브젝트가 자신인지 확인
                if (hit.collider.gameObject == this.gameObject&&Vector3.Distance(transform.position,player.transform.position)<1)
                {
                    // 오브젝트가 클릭된 경우 확대 기능 실행
                    ZoomIn();
                }
            }
        }
        }
    }

    // 확대 기능 예제 함수
    void ZoomIn()
    {
        toilet.SetActive(true);
    }
}
