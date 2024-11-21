using UnityEngine;
using UnityEngine.UI;

public class DishState : MonoBehaviour
{
    public GameObject RoomModify; //�� �������� �ƴҶ��� �۵��ϰ�

    public GameObject DishUI;
    public Image catDish;

    public SpriteRenderer sr;
    public bool clearDish;

    public int mealCount; //�� ��

    public Sprite[] dish = new Sprite[2];

    public ParentManager pm;

    public GameObject player; //���� ��ȣ�ۿ��Ҷ� �÷��̾ �����Ÿ� �����϶���


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
                // Ŭ���� ��ġ���� ȭ�� ��ǥ�� ���� ���� ����
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // ����ĳ��Ʈ�� ���� 3D �ݶ��̴��� ����
                if (Physics.Raycast(ray, out hit))
                {
                    // �浹�� ������Ʈ�� �ڽ����� Ȯ��
                    if (hit.collider.gameObject == gameObject && Vector3.Distance(transform.position, player.transform.position)<1)
                    {
                        // ������Ʈ�� Ŭ���� ��� Ȯ�� ��� ����
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
            //����̰� �������
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
            //���� �������ִٰ� �˷��ֱ�
        }
    }

}
