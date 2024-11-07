using UnityEngine;

public class FloorScript : MonoBehaviour
{
    Collider col;

    public bool occupied;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
          if (other.CompareTag("Set"))
        {
            occupied = true;
            other.gameObject.transform.parent.transform.position = transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Set"))
        {
            occupied = false;
        }
    }
}
