using Spine.Unity;
using System.Collections;
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
    public void GetGaguPosition()
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


        //가라코드 싹 다 쳐내
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    cc.CatGo(toilet);
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    StartCoroutine(ToMeal());
        //    cc.CatGo(dish);
        //    sa.AnimationName = "Food";
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    StartCoroutine(StartGara2());
        //    cc.CatGo(tower);
        //    // sa.AnimationName = "Sit";
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    StartCoroutine(StartGara2());

        //    cc.CatGo(bed);
        //    // sa.AnimationName = "Sit";

        //}
    }
    public IEnumerator ToMeal()
    {
        sa.AnimationName = "Walking";
        yield return new WaitForSeconds(1);
        sa.AnimationName = "Food";
    }
    public IEnumerator StartGara2()
    {
        sa.AnimationName = "Walking";
        yield return new WaitForSeconds(1);
        sa.AnimationName = "Sit";
    }
    public IEnumerator StartGara3()
    {
        sa.AnimationName = "Walking";
        yield return new WaitForSeconds(1);
        sa.AnimationName = "Food";
    }
}
