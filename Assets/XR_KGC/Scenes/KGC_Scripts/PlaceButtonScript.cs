using System.Collections;
using UnityEngine;

public class PlaceButtonScript : MonoBehaviour
{
    DragObject dragObject;

    public GameObject selected;
    public GameObject AlertText;

    public CatPosManager catPM;

    //public GameObject lines; //�����Ϸ��Ҷ� �����ֵ�
    //public GameObject placeCanvas;
    //public GameObject GaguCanvas;
    //public GameObject RoomModify;
    public ModifySetup ms; //�����ؼ� �����Ϸ��Ҷ� ���� �Լ� ����

    public bool gaguOnGagu;
    public bool itemOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AlertText = transform.GetChild(4).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (catPM != null)
        {
            GameObject.Find("Cat").GetComponent<CatPosManager>();
        }
        //if (lines == null)
        //{
        //    lines = GameObject.Find("LineParent");
        //}
        //if (placeCanvas == null)
        //{
        //    placeCanvas = GameObject.Find("PlaceCanvas");
        //}
        //if (GaguCanvas == null)
        //{
        //    GaguCanvas = GameObject.Find("GaguCanvas");
        //}
        //if (RoomModify == null)
        //{
        //    RoomModify = GameObject.Find("RoomModify");
        //}
        if (ms == null)
        {
            ms = GameObject.Find("ModifyCanvas").GetComponent<ModifySetup>();
        }

        if (selected == null)
        {
            itemOn = false;
        }
        else if (selected != null)
        {
            itemOn = true;
        }
    }
    //����̰� ���������� ��ġ ã�ư��� �ڵ�
    public void PosUpdate()
    {
        catPM.GetGaguPosition();
    }


    public void PlaceOBJ()
    {
        //GameObject go = GameObject.Find("OBJPreview");
        //dragObject = go.GetComponent<DragObject>();
        // go.name = "OBJ";
        if (selected.transform.GetChild(0).GetComponent<GaguCheck>().onGagu)
        {
           //�ٸ� ������ ���������� �׳� �����ع����� ����,�ȵ�
            selected.transform.position = Vector3.zero; 
           //Cancel();
        }

        dragObject = selected.GetComponent<DragObject>();
        dragObject.enabled = false;
        selected = null;
    }
    public void Cancel()
    {
       // GameObject go = GameObject.Find("OBJPreview");
        Destroy(selected);
        selected = null;
    }

    public void Rotate()
    {
        if (Mathf.Abs(selected.transform.eulerAngles.y - 180f) < 0.1f)
        {
            // y�� ȸ�� ���� 0���� ����
            Vector3 newRotation = selected.transform.eulerAngles;
            newRotation.y = 0f;
            selected.transform.eulerAngles = newRotation;
        }
        else
        {
            // y�� ȸ�� ���� 180���� ����
            Vector3 newRotation = selected.transform.eulerAngles;
            newRotation.y = 180f;
            selected.transform.eulerAngles = newRotation;
        }
    }
    public void EndModify()
    {
        if (gaguOnGagu)
        {
            StartCoroutine(GOGAlert());
        }
        else
        {
            ms.EndModify();
        }
    }
    public IEnumerator GOGAlert()
    {
        AlertText.SetActive(true);

        yield return new WaitForSeconds(2);

        AlertText.SetActive(false);
    }
}

