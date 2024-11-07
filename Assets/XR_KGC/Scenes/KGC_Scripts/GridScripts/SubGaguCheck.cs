using UnityEngine;

public class SubGaguCheck : MonoBehaviour
{

    public GaguCheck gaguCheck;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gaguCheck = transform.parent.GetChild(0).GetComponent<GaguCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.CompareTag("Set")))
        {
            gaguCheck.onGagu = true;
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
    }
}
