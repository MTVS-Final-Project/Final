using UnityEngine;

public class ModifySetup : MonoBehaviour
{
    public ParentManager pm;

    public bool lineOn;

    public bool buttonOff;

    public GameObject RoomModify;//Ŭ���ϸ� drag object�� Ȱ��ȭ�ؼ� �����̵��� �����ϰ� �ϴ� ��ũ��Ʈ
    public GameObject lines; //���ڹ���
    public GameObject GaguCanvas;//���� ����â
    public GameObject PlaceCanvas;//���� ȸ�� ��ġ ��ư����

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pm = GameObject.Find("PopupCanvas").GetComponent<ParentManager>();
        RoomModify = GameObject.Find("RoomModify");
        GaguCanvas = GameObject.Find("GaguCanvas");
        PlaceCanvas = GameObject.Find("PlaceCanvas");
        RoomModify.SetActive(false);
        GaguCanvas.SetActive(false);
        PlaceCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
         if (!pm.allDeactivated || buttonOff)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
         else if(!buttonOff&&pm.allDeactivated)
        {
            transform.GetChild(0).gameObject.SetActive(true);

        }

        if (lines == null)
        {
            if (GameObject.Find("LineParent") != null)
            {
                lines = GameObject.Find("LineParent");
                lines.SetActive(false);
            }
        }
    }
    public void StartModify()
    {
        lines.SetActive(true);
        GaguCanvas.SetActive(true);
        RoomModify.SetActive(true);
        PlaceCanvas.SetActive(true);
    }
    public void EndModify()
    {
        lines.SetActive(false);
        GaguCanvas.SetActive(false);
        RoomModify.SetActive(false);
        PlaceCanvas.SetActive(false);

        // �� ���� ��� DragObject ��ũ��Ʈ�� ��Ȱ��ȭ (��Ȱ��ȭ�� ���� ������Ʈ ����)
        DragObject[] dragObjects = FindObjectsOfType<DragObject>(true);

        // �� DragObject ��ũ��Ʈ�� ��Ȱ��ȭ
        foreach (DragObject dragObject in dragObjects)
        {
            dragObject.enabled = false;
        }

        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ButtonSwitch()
    {
        buttonOff = !buttonOff;
    }
}
