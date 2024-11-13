using UnityEngine;

public class ModifySetup : MonoBehaviour
{
    public ParentManager pm;

    public bool lineOn;

    public bool buttonOff;

    public GameObject RoomModify;//클릭하면 drag object를 활성화해서 가구이동을 가능하게 하는 스크립트
    public GameObject lines; //격자무늬
    public GameObject GaguCanvas;//가구 선택창
    public GameObject PlaceCanvas;//가구 회전 배치 버튼모음

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

        // 씬 내의 모든 DragObject 스크립트를 비활성화 (비활성화된 게임 오브젝트 포함)
        DragObject[] dragObjects = FindObjectsOfType<DragObject>(true);

        // 각 DragObject 스크립트를 비활성화
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
