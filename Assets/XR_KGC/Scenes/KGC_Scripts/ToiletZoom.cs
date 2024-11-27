using UnityEngine;

public class ToiletZoom : MonoBehaviour
{
    public GameObject RoomModify; //�� �������� �ƴҶ��� �۵��ϰ�
    public GameObject toilet;

    public GameObject player;

    public ParentManager pm;

    public int quantity = 0;//�󸶳� ���� �׿�����.
    //�� ĵ�������� ��� ���ӿ�����Ʈ�� ��Ȱ��ȭ�϶��� �۵��ϰ�.
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
            // Ŭ���� ��ġ���� ȭ�� ��ǥ�� ���� ���� ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ����ĳ��Ʈ�� ���� 3D �ݶ��̴��� ����
            if (Physics.Raycast(ray, out hit))
            {
                // �浹�� ������Ʈ�� �ڽ����� Ȯ��
                if (hit.collider.gameObject == this.gameObject&&Vector3.Distance(transform.position,player.transform.position)<1)
                {
                    // ������Ʈ�� Ŭ���� ��� Ȯ�� ��� ����
                    ZoomIn();
                }
            }
        }
        }
    }

    // Ȯ�� ��� ���� �Լ�
    void ZoomIn()
    {
        toilet.SetActive(true);
    }
}
