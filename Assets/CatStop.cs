using UnityEngine;

public class CatStop : MonoBehaviour
{
    public GameObject roomModify;
    public CatController controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomModify = GameObject.Find("RoomModify");
        controller = GameObject.Find("Cat").GetComponent<CatController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (roomModify == null)
        {
            roomModify = GameObject.Find("RoomModify");


        }
        if (roomModify.activeInHierarchy)
        {
            controller.modifying = true;
        }
        else
        {
            controller.modifying = false;
        }
    }
}
