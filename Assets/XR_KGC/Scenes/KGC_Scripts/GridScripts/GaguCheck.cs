using UnityEngine;

public class GaguCheck : MonoBehaviour
{
    public bool onGagu;
    public bool onGround;

    public Vector3 snapPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if ((other.CompareTag("Set")))
        {
                onGagu = true;
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
        }
        if (other.CompareTag("Ground"))
        {
            onGround = false;
        }
    }

}
