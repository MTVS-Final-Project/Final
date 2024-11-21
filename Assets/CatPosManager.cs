using Spine.Unity;
using System.Collections;
using TMPro;
using UnityEngine;

public class CatPosManager : MonoBehaviour
{
    public CatController cc;

    public SkeletonAnimation sa;
    public CatAI catAI;

    public Transform toilet;
    public Transform dish;
    public Transform tower;
    public Transform bed;
    public Transform towerBottom; //타워 올라가기전 위치
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
        if (towerBottom == null)
        {
            towerBottom = tower.parent.transform;
        }
    }
    public void CatMove(Vector3 positioin)
    {
        StartCoroutine(MoveTowards(positioin));
    }

    public void Jump(Vector3 positioin)
    {
        StartCoroutine(JumpUp(positioin));
    }
    public void GoTower()
    {
        StartCoroutine(ToTower(towerBottom.position));
    }

    public IEnumerator ToTower(Vector3 targetPosition)
    {
        sa.AnimationName = "Walking";
        float duration = 1.0f;
        float elapsed = 0f;
        Vector3 startingPosition = transform.position;
        Vector3 direction = (targetPosition - startingPosition).normalized;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(JumpUp(tower.position));
        
        sa.AnimationName = "Sit";
    }




    public IEnumerator MoveTowards(Vector3 targetPosition)
    {
        sa.AnimationName = "Walking";
        float duration = 1.0f;
        float elapsed = 0f;
        Vector3 startingPosition = transform.position;
        Vector3 direction = (targetPosition - startingPosition).normalized;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        sa.AnimationName = "Idle";
    }

    public IEnumerator JumpUp(Vector3 targetPosition)
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Vector3 startingPosition = transform.position;
        Vector3 direction = (targetPosition - startingPosition).normalized;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

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
