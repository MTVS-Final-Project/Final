using UnityEngine;

public class RoomModify : MonoBehaviour
{
    public PlaceButtonScript PlaceButtonScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, 1 << 10))
            {
               PlaceButtonScript.selected = hit.collider.gameObject;
                hit.collider.gameObject.GetComponent<DragObject>().enabled = true;
            }
        }
    }
}
