using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class CatAIFSM : MonoBehaviour
{
    public PetInteraction interaction;
    public CatController controller;
    public SkeletonAnimation anim;
    public GameObject player;

    // 고양이 가구 위치
    public Transform toilet;      // 화장실 위치
    public Transform dish;        // 밥그릇 위치
    public Transform tower;       // 타워 꼭대기 위치
    public Transform bed;         // 침대 위치
    public Transform towerBottom; // 타워 올라가기 전 위치

    // 고양이 상태 관련 수치
    public float sleepy = 100;    // 수면욕구
    public float friendly = 100f; // 우호도
    public float mood = 100;      // 기분, 높으면 우호도가 빨리 오름
    public float hunger = 100f;   // 배고픔 수치
    public float moveTerm = 5;    // 얼마나 자주 움직이는지 (활동성과 연관)
    public float moveRange = 2;   // 한 번에 최대 얼마나 멀리 가는지
    public float discharge = 0;   // 화장실 사용 욕구
    public float metabolism = 1;  // 신진대사, 높을수록 배고픔이 빨리 줄고 수면을 짧게 해도 됨

    // 고양이 속도 관련
    public float speed = 1;       // 고양이 속도, 기분/허기 상태에 따라 다름
    public float eatSpeed = 1;    // 밥 먹는 속도, 높으면 빨리 먹음

    public DishState ds;          // 밥그릇 상태 확인
    public List<GameObject> tiles = new List<GameObject>();        // 배회 상태일 때 이동 가능한 타일
    public List<GameObject> tilesInRange = new List<GameObject>(); // 고양이 주변 일정 거리 내 타일

    // 고양이 상태 플래그
    public bool rest;      // 고양이가 쉬고 있는지
    public bool toSleep;   // 고양이가 자고 싶은지
    public bool toMeal;    // 고양이가 밥 먹고 싶은지
    public bool eating;    // 고양이가 밥 먹고 있는지
    public bool starving; //배고픈데 밥그릇에 밥이 없다.

    // 고양이 상태
    public enum CatState
    {
        Idle,           // 기본 상태
        Wandering,      // 배회
        Eating,         // 밥 먹는 중
        Sleeping,       // 자는 중
        MovingToMeal,   // 밥 먹으러 이동 중
        MovingToTower,  // 타워로 이동 중
        JumpToSleep,
        BegForFood,     //밥달라.
        WantToPlay,     //놀아달라
        PlayerCalled    // 플레이어가 부른 상태
    }
    public CatState state;

    void Start()
    {
        interaction = gameObject.GetComponentInChildren<PetInteraction>();
        player = GameObject.Find("Player");
        state = CatState.Wandering; // 초기 상태 설정
        StartCoroutine(StateController());
        rest = false; // 초기에는 쉬지 않음
    }

    public void GetGaguPosition()
    {
        //이 스크립트가 진짜 써야되는거임 밑에거는 테스트용임.

        toilet = GameObject.Find("CatToilet(Clone)").transform;
        dish = GameObject.Find("Dish(Clone)").transform;
        tower = GameObject.Find("TowerPosition").transform;
        bed = GameObject.Find("Bed(Clone)").transform;
        towerBottom = GameObject.Find("CatTower(Clone)").transform;
        ds = GameObject.Find("Dish(Clone)").GetComponent<DishState>();

        //---------------------------------------------------- 고양이 테스트하는용
        //toilet = GameObject.Find("CatToilet").transform;
        //dish = GameObject.Find("Dish").transform;
        //tower = GameObject.Find("TowerPosition").transform;
        //bed = GameObject.Find("Bed").transform;
        //towerBottom = GameObject.Find("CatTower").transform;
        //ds = GameObject.Find("Dish").GetComponent<DishState>();

    }

    void LateUpdate()
    {
        if (player == null)
        {
             player =  GameObject.Find("Player");
        }
        print("여기까지됨");
        GetGaguPosition();

        // 초기화: 게임 오브젝트 연결
        if (ds == null)
        {
        }

        UpdateBasicNeeds(); // 기본 욕구 업데이트

        // 상태 변경 조건 확인
        if (hunger < 50 && !toMeal && !starving)
        {
            state = CatState.MovingToMeal; // 배고프면 밥 먹으러 이동
        }



        if (sleepy < 30 && !toSleep && !toMeal && !rest && !starving)
        {
            state = CatState.MovingToTower; // 졸리면 자러 이동
        }

        if (state == CatState.MovingToMeal)
        {
            toMeal = true;
        }
        else
        { toMeal = false; }

        if (state == CatState.MovingToTower)
        {
            toSleep = true;
        }
        else
        { toSleep = false; }

        if (tiles.Count <= 0) // 고양이 주변 타일 찾기
        {
            GameObject tParent = GameObject.Find("TileParent");
            foreach (Transform child in tParent.transform)
            {
                tiles.Add(child.gameObject);
            }
        }
    }

    private void UpdateBasicNeeds()
    {
        // 배고픔 수치 감소
        hunger -= Time.deltaTime * metabolism;

        // 쉬고 있을 때와 아닐 때의 수면욕구 변화
        if (!rest)
        {
            sleepy -= Time.deltaTime * metabolism*0.5f;   // 활동 중 수면욕구 감소
        }
        else
        {
            sleepy += Time.deltaTime * metabolism * 1.5f;  // 쉬는 중 수면욕구 증가
        }

        // 수면 욕구 최대치 제한 및 상태 복귀
        if (sleepy > 100)
        {
            sleepy = 100;
            state = CatState.Wandering; // 배회 상태로 복귀
            rest = false;
        }
        if (hunger > 20)
        {
            discharge += Time.deltaTime * metabolism * 0.01f;
        }

    }

    private IEnumerator StateController()
    {
        // 상태를 지속적으로 확인하며 동작 수행
        while (true)
        {
            switch (state)
            {
                case CatState.Idle:
                    IdleBehavior();
                    break;

                case CatState.Wandering:
                    yield return StartCoroutine(Wandering(moveTerm));
                    break;

                case CatState.Eating:
                    yield return StartCoroutine(Eat());
                    break;

                case CatState.Sleeping:
                    yield return StartCoroutine(SleepBehavior());
                    break;

                case CatState.MovingToMeal:
                    yield return StartCoroutine(ToMeal(dish.position, speed));
                    break;

                case CatState.MovingToTower:
                    yield return StartCoroutine(ToTower(towerBottom.position, speed));
                    break;

                case CatState.PlayerCalled:
                    RespondToPlayer();
                    break;

                case CatState.BegForFood:
                    if (ds.mealCount > 0)
                    {
                        state = CatState.MovingToMeal;
                    }
                    break;
                case CatState.WantToPlay:

                    break;

                default:
                    Debug.LogWarning("Unknown state!");
                    break;
            }

            yield return null;
        }
    }
    public void CallCat()
    {
        StartCoroutine(MoveTowards(player.transform.position - new Vector3(0.2f, 0.2f, 0)));
    }

    private void IdleBehavior()
    {
        anim.AnimationName = "Idle"; //확률에 따라 돌아다니거나 놀거나 하는

        state = CatState.Wandering;

    }
    public void Ignore()
    {
        interaction.Ignore();
    }

    private IEnumerator SleepBehavior()
    {
        // 자는 중
        anim.AnimationName = "Sit";
        rest = true; // 쉬는 상태로 변경
        while (sleepy < 80)
        {
            if (state == CatState.MovingToMeal && sleepy > 60)//배고프면 일찍일어남.
            {
                break;
            }
            yield return null;
        }
        rest = false;
        state = CatState.Idle; // 수면욕구 충족 시 배회 상태로 복귀
    }

    private void RespondToPlayer()
    {
        // 플레이어 호출에 응답
        StopAllCoroutines();
        state = CatState.Idle; // 응답 후 기본 상태로 복귀
    }

    public IEnumerator ToMeal(Vector3 targetPosition, float duration)
    {
        // 밥 먹으러 이동
        anim.AnimationName = "Walking";
        yield return StartCoroutine(MoveTowards(targetPosition));

        if (ds.mealCount > 0)
        {
            state = CatState.Eating; // 도착 후 Eating 상태로 전환
        }
        else
        {
            state = CatState.BegForFood; // 임시
            StartCoroutine(GiveMeFood());
            starving = true;
        }
    }

    public IEnumerator GiveMeFood()
    {
        Vector3 targetPosition = player.transform.position;
        float playerDistance = Vector3.Distance(targetPosition, transform.position);

        if (playerDistance > 1)
        {
            anim.AnimationName = "Walking";
            float duration = playerDistance;
            float elapsed = 0f;
            Vector3 startingPosition = transform.position;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            anim.AnimationName = "idle"; //앞발 휘적이는거로 바꿔야됨.
            interaction.GiveMeFood();
            transform.position = targetPosition;

        }
        else
        {
            interaction.GiveMeFood();
        }
        yield return new WaitForSeconds(1);

        if (ds.mealCount <= 0)
        {
            StartCoroutine(GiveMeFood());
        }

    }
    public IEnumerator ToTower(Vector3 targetPosition, float duration)
    {
        // 타워로 이동
        anim.AnimationName = "Walking";
        yield return StartCoroutine(MoveTowards(targetPosition));

        yield return StartCoroutine(JumpUpToSleep(tower.position)); // 점프 후 타워로 이동
    }

    public IEnumerator Eat()
    {
        starving = false;
        // 밥 먹는 중
        anim.AnimationName = "Food";
        while (ds.mealCount > 0)
        {
            ds.mealCount -= Time.deltaTime * eatSpeed * metabolism * 0.3f;
            hunger += Time.deltaTime * 15;

            if (ds.mealCount < 0.01f)
            {
                ds.mealCount = 0;
            }

            yield return null;
        }

        toMeal = false;
        state = CatState.Idle; //  
    }

    public IEnumerator Wandering(float term)
    {
        // 고양이 배회 상태
        tilesInRange.Clear();
        foreach (var tile in tiles)
        {
            if (Vector3.Distance(transform.position, tile.transform.position) < moveRange &&
                !tile.GetComponent<FloorScript>().occupied)
            {
                tilesInRange.Add(tile); // 이동 가능한 타일 추가
            }
        }

        if (tilesInRange.Count > 0)
        {
            yield return StartCoroutine(MoveTowards(tilesInRange[Random.Range(0, tilesInRange.Count)].transform.position));
        }

        yield return new WaitForSeconds(term);
        state = CatState.Idle; // 배회 후 Idle 상태로 복귀
    }

    public IEnumerator MoveTowards(Vector3 targetPosition)
    {
        // 특정 목표 위치로 이동
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

    public IEnumerator JumpUp(Vector3 targetPosition)
    {
        // 타워로 점프
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

    public IEnumerator JumpUpToSleep(Vector3 targetPosition)
    {
        // 타워로 점프
        float duration = 0.3f;
        float elapsed = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        state = CatState.Sleeping; // Sleeping 상태로 전환

        transform.position = targetPosition;
    }
}
