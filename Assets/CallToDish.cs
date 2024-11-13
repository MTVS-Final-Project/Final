using System.Collections;
using UnityEngine;

public class CallToDish : MonoBehaviour
{
    public DishState ds;
    public CatController cc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (ds == null)
        {
            ds = GetComponent<DishState>();

        }
        if (cc == null)
        {
            cc = GameObject.Find("Cat").GetComponent<CatController>();

        }
    }

     
}
