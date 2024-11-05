using UnityEngine;

public class StreetClick : MonoBehaviour
{
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

            if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000, 1 << 11))
            {
                Debug.Log(hit.collider.name);
                hit.collider.gameObject.GetComponent<StreetToOther>().Go();
            }

        }
    }

    

}
