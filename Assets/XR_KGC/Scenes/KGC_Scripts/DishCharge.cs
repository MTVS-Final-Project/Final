using UnityEngine;

public class DishCharge : MonoBehaviour
{
    public DishState ds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ds == null)
        {
            ds = GameObject.Find("Dish(Clone)").GetComponent<DishState>();
        }
    }

    public void CallCharge()
    {
        ds.ChargeMeal();
    }
}
