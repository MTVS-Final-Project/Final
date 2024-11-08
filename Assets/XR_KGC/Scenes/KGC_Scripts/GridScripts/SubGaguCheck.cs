using System;
using UnityEngine;

public class SubGaguCheck : MonoBehaviour
{

    public GaguCheck gaguCheck;
    public DragObject dragObject;

    public bool outOfRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gaguCheck = transform.parent.GetChild(0).GetComponent<GaguCheck>();
        dragObject = transform.parent.GetComponent<DragObject>();   
    }

    // Update is called once per frame
   
    private void OnTriggerStay(Collider other)
    {
        if ((other.CompareTag("Set")))
        {
            gaguCheck.onGagu = true;
        }
        if (other.CompareTag("Ground"))
        {
            outOfRange = false;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Set"))
        {
            gaguCheck.onGagu = false;
        }
        if (other.CompareTag("Ground"))
        {
            gaguCheck.onGround = false;
            //dragObject.isDragging = false;
        }
        if (other.CompareTag("Ground"))
        {
            outOfRange = true;
        }
    }


    private void LateUpdate()
    {
        if (!dragObject.isDragging && outOfRange)
        {
            if (Mathf.Abs(transform.parent.transform.eulerAngles.y) < 0.1f)
            {
                transform.parent.transform.position = transform.parent.transform.position + new Vector3(-0.5f, -0.25f, 0);
            }
            else if (Mathf.Abs(transform.parent.transform.eulerAngles.y - 180f) < 0.1f)
            {
                transform.parent.transform.position = transform.parent.transform.position + new Vector3(0.5f, -0.25f, 0);

            }


        }

    }

}
