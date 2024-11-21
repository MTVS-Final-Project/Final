using UnityEngine;
using UnityEngine.UI;

public class DishState : MonoBehaviour
{
    public GameObject RoomModify; //방 수정중이 아닐때만 작동하게

    public GameObject DishUI;
    public Image catDish;

    public SpriteRenderer sr;
    public bool clearDish;

    public int mealCount; //밥 양

    public Sprite[] dish = new Sprite[2];

    public ParentManager pm;

    public GameObject player; //가구 상호작용할때 플래이어가 일정거리 이하일때만


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pm = GameObject.Find("PopupCanvas").GetComponent<ParentManager>();
       sr = transform.GetChild(1).GetComponent<SpriteRenderer>();
        DishUI = GameObject.Find("PopupCanvas").transform.GetChild(1).gameObject;
        catDish = DishUI.GetComponent<Image>();
        RoomModify = GameObject.Find("RoomModify");

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
        if (mealCount <= 0)
        {
            clearDish = true;
        }
        else
        {
            clearDish = false;
        }


        if (clearDish)
        {
            sr.sprite = dish[0];
            catDish.sprite = dish[0];
        }
        else if (!clearDish)
        {
            sr.sprite = dish[1];
            catDish.sprite = dish[1];

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
                    if (hit.collider.gameObject == gameObject && Vector3.Distance(transform.position, player.transform.position)<1)
                    {
                        // 오브젝트가 클릭된 경우 확대 기능 실행
                        ZoomInDish();
                    }
                }
            }
        }


    }
    public void ZoomInDish()
    {
        DishUI.SetActive(true);

    }

    public void Consume()
    {
        if (mealCount <= 1)
        {
            mealCount -= 1;
        }
        else
        {
            //고양이가 배고파함
        }
    }

    public void ChargeMeal()
    {
        if (mealCount < 1)
        {
        mealCount += 1;
        }
        else
        {
            //밥이 가득차있다고 알려주기
        }
    }

}
