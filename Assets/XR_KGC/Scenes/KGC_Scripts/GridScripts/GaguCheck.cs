using Unity.VisualScripting;
using UnityEngine;

public class GaguCheck : MonoBehaviour
{
    public bool onGagu;
    public bool onGround;

    public PlaceButtonScript buttonScript;
    public SpriteRenderer sr; //스프라이트 색 바꾸기

    public Vector3 snapPos;

   // public bool findButtonScript;

   // public DragObject dragObject;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // dragObject = transform.parent.GetComponent<DragObject>();
       buttonScript = GameObject.Find("PlaceCanvas").GetComponent<PlaceButtonScript>();
       sr = transform.parent.GetChild(1).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonScript == null)
        {
            buttonScript = GameObject.Find("PlaceCanvas").GetComponent<PlaceButtonScript>();

        }

        buttonScript.gaguOnGagu = onGagu;

        if (onGagu)
        {
            sr.color = new Color(255f / 255f, 0f / 255f, 0f / 255f, 150f / 255f);
            //sr.color = new Color32(255, 0, 0, 150);
        }
        else
        {
            sr.color = Color.white;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if ((other.CompareTag("Set")))
        {
                onGagu = true;
           // buttonScript.gaguOnGagu = true;
        }
        
        if (other.CompareTag("Ground"))
        {
            onGround = true;
            snapPos = other.gameObject.transform.position;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Set"))
        {
            onGagu = false;
            //buttonScript.gaguOnGagu = false;
        }
        if (other.CompareTag("Ground"))
        {
            onGround = false;
            //dragObject.isDragging = false;
        }
    }

}
