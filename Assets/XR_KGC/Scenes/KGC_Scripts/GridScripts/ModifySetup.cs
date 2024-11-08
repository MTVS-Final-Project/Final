using UnityEngine;

public class ModifySetup : MonoBehaviour
{

    public bool lineOn;

    public GameObject RoomModify;//Ŭ���ϸ� drag object�� Ȱ��ȭ�ؼ� �����̵��� �����ϰ� �ϴ� ��ũ��Ʈ
    public GameObject lines; //���ڹ���
    public GameObject GaguCanvas;//���� ����â
    public GameObject PlaceCanvas;//���� ȸ�� ��ġ ��ư����

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

}
