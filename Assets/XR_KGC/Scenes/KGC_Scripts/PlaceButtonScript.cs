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
            selected.transform.position = Vector3.zero;
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
       SpriteRenderer sr = selected.GetComponent<SpriteRenderer>();
        if (sr.flipX)
        {
            sr.flipX = false;
        }
        else if (!sr.flipX)
        {
            sr.flipX=true;
        }
    }

}

