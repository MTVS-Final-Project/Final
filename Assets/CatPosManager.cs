using Spine.Unity;
using UnityEngine;

public class CatPosManager : MonoBehaviour
{
    public CatController cc;

    public SkeletonAnimation sa;

    public Transform toilet;
    public Transform dish;
    public Transform tower;
    public Transform bed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (toilet == null)
        {
            toilet = GameObject.Find("ToiletPosition").transform;
        }
            if (dish == null)
        {
            dish = GameObject.Find("CatDishPoint").transform;
        }
                if (tower == null)
        {
            tower = GameObject.Find("TowerPosition").transform;
        }
                    if (bed == null)
        {
            bed = GameObject.Find("CatBedPosition").transform;
        }



                    if (Input.GetKeyDown(KeyCode.Q))
        {
            cc.CatGo(toilet);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            cc.CatGo(dish);
            sa.AnimationName = "Food";
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            cc.CatGo(tower);
            sa.AnimationName = "Sit";
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            cc.CatGo(bed);
            sa.AnimationName = "Sit";

        }


    }
}
