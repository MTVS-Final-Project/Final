using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Vivox;
using UnityEditor;
using UnityEngine;

public class CatAI : MonoBehaviour
{
    public CatController controller;
    public SkeletonAnimation anim;

    public Transform toilet;
    public Transform dish;
    public Transform tower;
    public Transform bed;
    public Transform towerBottom; //타워 올라가기전 위치

    public float sleepy = 100; // 수면욕구
    public float friendly = 100f; // 우호도
    public float mood = 100; // 기분 ,높으면 우호도가 빨리오름
    public float hunger = 100f; // 배고픔 수치
    public float moveTerm = 5; // 얼마나 자주 움직이는지, 활동성과 엮어서
    public float moveRange = 2; // 한번에 최대 얼마나 멀리 가는지.
    public float discharge = 0; // 화장실 사용 욕구
    public float metabolism = 1;//신진대사, 높을수록 배고픔이 빨리 줄어들고 수면을 짧게해도 됨.

    public float speed = 1; //고양이 속도, 기분,허기상태에 따라 속도가 달라짐.낮아야 빠름
    public float eatSpeed = 1;//밥먹는속도.높으면 빨리먹음

    public DishState ds;//밥그릇 차있나 없나 확인
    public List<GameObject> tiles = new List<GameObject>(); // 배회하는 상태일 때 가장 가까운 타일 중 occupied가 아닌 곳으로 이동
    public List<GameObject> tilesInRange = new List<GameObject>(); // 고양이한테서 일정 이내 타일을 찾는 함수

    public bool rest; // 고양이 쉬고 있을 때.
    public bool toSleep; //고양이가 자고싶은지
    public bool toMeal; //고양이가 밥먹고싶은지


    public bool eating; //고양이가 밥먹고있는지
    


    public enum CatState
    {
        Normal,        // 기본상태
        Hungry,      // 배고플 때
        Sleepy,     // 자고 싶을 때
        Joyfull,     // 막 돌아다니는 상태
        PlayerCalled // 플레이어가 불렀을 때
    }
    public CatState state;

    void Start()
    {
        StartCoroutine(Wandering(moveTerm));
        rest = false;
    }

    void Update()
    {

        if (ds == null)
        {
            ds = GameObject.Find("Dish").GetComponent<DishState>();
        }
        if (controller == null)
        {
            GetComponent<CatController>();
        }

        if (tiles.Count <= 0) // 고양이 주변 타일 찾기
        {
            GameObject tParent = GameObject.Find("TileParent");
            foreach (Transform child in tParent.transform)
            {
                tiles.Add(child.gameObject);
            }
        }
        if (toilet == null) //가구들 위치 찾아오기
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

        hunger -= Time.deltaTime * metabolism; //허기수치 줄어듦

        if (!rest)//쉬고있으면 sleepy수치가 줄어듦
        {
            sleepy -= Time.deltaTime;   //안잘때
        }
        else
        {
            sleepy += Time.deltaTime * metabolism;  //잘때
        }

        if (hunger > 0) //화장실 욕구 충전
        {
            discharge += Time.deltaTime * metabolism * 0.01f;
        }


        if (hunger < 50&&!toMeal)
        {
            toMeal = true;
            GoMeal();
        }

        if (sleepy < 30 && !toSleep&&!eating) //30이 되면 자러감,밥은 다 먹고 감
        {
            toSleep = true;
            GoTower();
        }
        if (sleepy > 50)
        {
            toSleep = false;
        }
        if (sleepy > 100)
        {
            sleepy = 100;
            startWander();
            rest = false;
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            GoTower();
        }
    }

    public void startWander()
    {
        StopAllCoroutines();
        StartCoroutine(Wandering(moveTerm));
    }
    public void GoMeal()
    {
        StopAllCoroutines();
        StartCoroutine(ToMeal(dish.transform.position, speed));
        rest = false;
        toSleep = false;
    }
    public void GoTower()
    {
        StopAllCoroutines();
        StartCoroutine(ToTower(towerBottom.position, speed));
        rest = false;
    }

    public IEnumerator ToTower(Vector3 targetPosition, float duration) //타워 사용을 위해 올라가는 코루틴
    {
        anim.AnimationName = "Walking";
        //duration = 1.0f;
        float elapsed = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(JumpUp(tower.position));

        anim.AnimationName = "Sit";
        rest = true;
    }

    public IEnumerator ToMeal(Vector3 targetPosition, float duration) //밥먹으러 가는 코루틴
    {
        anim.AnimationName = "Walking";
        duration = 1.0f;
        float elapsed = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        if (ds.mealCount > 0)
        {
            StartCoroutine(Eat());
        }
        else
        {
            //밥이없으니 플레이어에게 조르러 감.
        }

    }

    public IEnumerator Eat() //밥 먹는 코루틴
    {
        eating = true;
        anim.AnimationName = "Food";
        while (0 < ds.mealCount)
        {
            ds.mealCount -= Time.deltaTime * eatSpeed * metabolism*0.3f;
            hunger += Time.deltaTime * 10;
            if (ds.mealCount < 0.01)
            {
                ds.mealCount = 0;
            }
            yield return null;
        }
        toMeal = false;
        eating = false;
        anim.AnimationName = "Idle";
        startWander();

    }

    public IEnumerator Wandering(float Term) // 플레이어가 부를 때는 중단시켜야 되는 코루틴
    {
        tilesInRange.Clear();
        for (int i = 0; i < tiles.Count; i++)
        {
            if (Vector3.Distance(transform.position, tiles[i].transform.position) < moveRange && !tiles[i].GetComponent<FloorScript>().occupied)
            {
                tilesInRange.Add(tiles[i]); // 타일 추가
            }
        }
        if (tilesInRange.Count > 0)
        {
            yield return StartCoroutine(MoveTowards(tilesInRange[Random.Range(0, tilesInRange.Count)].transform.position));
        }
        yield return new WaitForSeconds(Term);
        StartCoroutine(Wandering(moveTerm));
    }


    public IEnumerator MoveTowards(Vector3 targetPosition) //특정목표 위치로 이동하는 스크립트
    {
        anim.AnimationName = "Walking";
        float duration = 1.0f;
        float elapsed = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        anim.AnimationName = "Idle";
    }


    public IEnumerator JumpUp(Vector3 targetPosition) //점프할?때 쓰이는 스크립트. 그냥 이동이 빠름
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }


}
