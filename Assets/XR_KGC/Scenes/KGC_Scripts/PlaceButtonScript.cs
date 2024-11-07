using UnityEngine;

public class PlaceButtonScript : MonoBehaviour
{
    DragObject dragObject;

    public GameObject selected;

    public bool itemOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (selected == null)
        {
            itemOn = false;
        }
        else if (selected != null)
        {
            itemOn = true;
        }
    }
    public void PlaceOBJ()
    {
        //GameObject go = GameObject.Find("OBJPreview");
        //dragObject = go.GetComponent<DragObject>();
        // go.name = "OBJ";
        if (selected.transform.GetChild(0).GetComponent<GaguCheck>().onGagu)
        {
           //다른 가구랑 겹쳐있으면 그냥 제거해버리로 변경,안됨
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
            // y축 회전 값을 0으로 설정
            Vector3 newRotation = selected.transform.eulerAngles;
            newRotation.y = 0f;
            selected.transform.eulerAngles = newRotation;
        }
        else
        {
            // y축 회전 값을 180도로 설정
            Vector3 newRotation = selected.transform.eulerAngles;
            newRotation.y = 180f;
            selected.transform.eulerAngles = newRotation;
        }
    }


}

