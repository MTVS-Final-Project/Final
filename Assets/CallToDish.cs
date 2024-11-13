using System.Collections;
using UnityEngine;

public class CallToDish : MonoBehaviour
{
    public DishState ds;
    public CatController cc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ds = GetComponent<DishState>();
        cc = GameObject.Find("Cat").GetComponent<CatController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator CallToMeal()
    {
        if (ds.mealCount > 0)
        {
            cc.CatMoveTo(transform);
        }
        yield return new WaitForSeconds(10f);
    }
}
