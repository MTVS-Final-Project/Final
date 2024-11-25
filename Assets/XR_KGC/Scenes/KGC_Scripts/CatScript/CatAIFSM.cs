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

    // ����� ���� ��ġ
    public Transform toilet;      // ȭ��� ��ġ
    public Transform dish;        // ��׸� ��ġ
    public Transform tower;       // Ÿ�� ����� ��ġ
    public Transform bed;         // ħ�� ��ġ
    public Transform towerBottom; // Ÿ�� �ö󰡱� �� ��ġ

    // ����� ���� ���� ��ġ
    public float sleepy = 100;    // ����屸
    public float friendly = 100f; // ��ȣ��
    public float mood = 100;      // ���, ������ ��ȣ���� ���� ����
    public float hunger = 100f;   // ����� ��ġ
    public float moveTerm = 5;    // �󸶳� ���� �����̴��� (Ȱ������ ����)
    public float moveRange = 2;   // �� ���� �ִ� �󸶳� �ָ� ������
    public float discharge = 0;   // ȭ��� ��� �屸
    public float metabolism = 1;  // �������, �������� ������� ���� �ٰ� ������ ª�� �ص� ��

    // ����� �ӵ� ����
    public float speed = 1;       // ����� �ӵ�, ���/��� ���¿� ���� �ٸ�
    public float eatSpeed = 1;    // �� �Դ� �ӵ�, ������ ���� ����

    public DishState ds;          // ��׸� ���� Ȯ��
    public List<GameObject> tiles = new List<GameObject>();        // ��ȸ ������ �� �̵� ������ Ÿ��
    public List<GameObject> tilesInRange = new List<GameObject>(); // ����� �ֺ� ���� �Ÿ� �� Ÿ��

    // ����� ���� �÷���
    public bool rest;      // ����̰� ���� �ִ���
    public bool toSleep;   // ����̰� �ڰ� ������
    public bool toMeal;    // ����̰� �� �԰� ������
    public bool eating;    // ����̰� �� �԰� �ִ���
    public bool starving; //����µ� ��׸��� ���� ����.

    // ����� ����
    public enum CatState
    {
        Idle,           // �⺻ ����
        Wandering,      // ��ȸ
        Eating,         // �� �Դ� ��
        Sleeping,       // �ڴ� ��
        MovingToMeal,   // �� ������ �̵� ��
        MovingToTower,  // Ÿ���� �̵� ��
        JumpToSleep,
        BegForFood,     //��޶�.
        WantToPlay,     //��ƴ޶�
        PlayerCalled    // �÷��̾ �θ� ����
    }
    public CatState state;

    void Start()
    {
        interaction = gameObject.GetComponentInChildren<PetInteraction>();
        player = GameObject.Find("Player");
        state = CatState.Wandering; // �ʱ� ���� ����
        StartCoroutine(StateController());
        rest = false; // �ʱ⿡�� ���� ����
    }

    public void GetGaguPosition()
    {
        //�� ��ũ��Ʈ�� ��¥ ��ߵǴ°��� �ؿ��Ŵ� �׽�Ʈ����.

        toilet = GameObject.Find("CatToilet(Clone)").transform;
        dish = GameObject.Find("Dish(Clone)").transform;
        tower = GameObject.Find("TowerPosition").transform;
        bed = GameObject.Find("Bed(Clone)").transform;
        towerBottom = GameObject.Find("CatTower(Clone)").transform;
        ds = GameObject.Find("Dish(Clone)").GetComponent<DishState>();

        //---------------------------------------------------- ����� �׽�Ʈ�ϴ¿�
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
        print("���������");
        GetGaguPosition();

        // �ʱ�ȭ: ���� ������Ʈ ����
        if (ds == null)
        {
        }

        UpdateBasicNeeds(); // �⺻ �屸 ������Ʈ

        // ���� ���� ���� Ȯ��
        if (hunger < 50 && !toMeal && !starving)
        {
            state = CatState.MovingToMeal; // ������� �� ������ �̵�
        }



        if (sleepy < 30 && !toSleep && !toMeal && !rest && !starving)
        {
            state = CatState.MovingToTower; // ������ �ڷ� �̵�
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

        if (tiles.Count <= 0) // ����� �ֺ� Ÿ�� ã��
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
        // ����� ��ġ ����
        hunger -= Time.deltaTime * metabolism;

        // ���� ���� ���� �ƴ� ���� ����屸 ��ȭ
        if (!rest)
        {
            sleepy -= Time.deltaTime * metabolism*0.5f;   // Ȱ�� �� ����屸 ����
        }
        else
        {
            sleepy += Time.deltaTime * metabolism * 1.5f;  // ���� �� ����屸 ����
        }

        // ���� �屸 �ִ�ġ ���� �� ���� ����
        if (sleepy > 100)
        {
            sleepy = 100;
            state = CatState.Wandering; // ��ȸ ���·� ����
            rest = false;
        }
        if (hunger > 20)
        {
            discharge += Time.deltaTime * metabolism * 0.01f;
        }

    }

    private IEnumerator StateController()
    {
        // ���¸� ���������� Ȯ���ϸ� ���� ����
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
        anim.AnimationName = "Idle"; //Ȯ���� ���� ���ƴٴϰų� ��ų� �ϴ�

        state = CatState.Wandering;

    }
    public void Ignore()
    {
        interaction.Ignore();
    }

    private IEnumerator SleepBehavior()
    {
        // �ڴ� ��
        anim.AnimationName = "Sit";
        rest = true; // ���� ���·� ����
        while (sleepy < 80)
        {
            if (state == CatState.MovingToMeal && sleepy > 60)//������� �����Ͼ.
            {
                break;
            }
            yield return null;
        }
        rest = false;
        state = CatState.Idle; // ����屸 ���� �� ��ȸ ���·� ����
    }

    private void RespondToPlayer()
    {
        // �÷��̾� ȣ�⿡ ����
        StopAllCoroutines();
        state = CatState.Idle; // ���� �� �⺻ ���·� ����
    }

    public IEnumerator ToMeal(Vector3 targetPosition, float duration)
    {
        // �� ������ �̵�
        anim.AnimationName = "Walking";
        yield return StartCoroutine(MoveTowards(targetPosition));

        if (ds.mealCount > 0)
        {
            state = CatState.Eating; // ���� �� Eating ���·� ��ȯ
        }
        else
        {
            state = CatState.BegForFood; // �ӽ�
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
            anim.AnimationName = "idle"; //�չ� �����̴°ŷ� �ٲ�ߵ�.
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
        // Ÿ���� �̵�
        anim.AnimationName = "Walking";
        yield return StartCoroutine(MoveTowards(targetPosition));

        yield return StartCoroutine(JumpUpToSleep(tower.position)); // ���� �� Ÿ���� �̵�
    }

    public IEnumerator Eat()
    {
        starving = false;
        // �� �Դ� ��
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
        // ����� ��ȸ ����
        tilesInRange.Clear();
        foreach (var tile in tiles)
        {
            if (Vector3.Distance(transform.position, tile.transform.position) < moveRange &&
                !tile.GetComponent<FloorScript>().occupied)
            {
                tilesInRange.Add(tile); // �̵� ������ Ÿ�� �߰�
            }
        }

        if (tilesInRange.Count > 0)
        {
            yield return StartCoroutine(MoveTowards(tilesInRange[Random.Range(0, tilesInRange.Count)].transform.position));
        }

        yield return new WaitForSeconds(term);
        state = CatState.Idle; // ��ȸ �� Idle ���·� ����
    }

    public IEnumerator MoveTowards(Vector3 targetPosition)
    {
        // Ư�� ��ǥ ��ġ�� �̵�
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
        // Ÿ���� ����
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
        // Ÿ���� ����
        float duration = 0.3f;
        float elapsed = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        state = CatState.Sleeping; // Sleeping ���·� ��ȯ

        transform.position = targetPosition;
    }
}
