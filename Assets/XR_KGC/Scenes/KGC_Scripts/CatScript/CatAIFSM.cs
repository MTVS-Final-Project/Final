using JetBrains.Annotations;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class CatAIFSM : MonoBehaviour
{
    public PetInteraction interaction;
    public CatController controller;
    public SkeletonAnimation anim;
    public GameObject player;
    public SkeletonAnimation sa;


    // ����� ���� ��ġ
    public Transform toilet;      // ȭ��� ��ġ
    public Transform dish;        // ��׸� ��ġ
    public Transform tower;       // Ÿ�� ����� ��ġ
    public Transform bed;         // ħ�� ��ġ
    public Transform towerBottom; // Ÿ�� �ö󰡱� �� ��ġ

    // ����� ���� ���� ��ġ
    public string personality;    //����� ����
    public float sleepy = 100;    // ����屸
    public float friendly = 100f; // ��ȣ��
    public float mood = 100;      // ���, ������ ��ȣ���� ���� ����
    public float hunger = 100f;   // ����� ��ġ
    public float moveTerm = 5;    // �󸶳� ���� �����̴��� (Ȱ������ ����)
    public float moveRange = 2;   // �� ���� �ִ� �󸶳� �ָ� ������
    public float discharge = 0;   // ȭ��� ��� �屸
    public float metabolism = 1;  // �������, �������� ������� ���� �ٰ� ������ ª�� �ص� ��
    public float weight = 3; //����� ������
    public float age = 3f; //����� ����
    public bool male = false; //��,�� ����
    

    // ����� �ӵ� ����
    public float speed = 1;       // ����� �ӵ�, ���/��� ���¿� ���� �ٸ� ���ƾ� ����
    public float eatSpeed = 1;    // �� �Դ� �ӵ�, ������ ���� ����
    public int CatIndex = 0; //0���� ��ο��, 1���� ����� 2���� �Ķ��� 3������ ���� ����ŷ�.

    // ����� ���� �÷���
    public bool rest;      // ����̰� ���� �ִ���
    public bool toSleep;   // ����̰� �ڰ� ������
    public bool toMeal;    // ����̰� �� �԰� ������
    public bool eating;    // ����̰� �� �԰� �ִ���
    public bool starving; //����µ� ��׸��� ���� ����.
    public bool isFat = false; //���ȴ��� �ƴ��� Ȯ�ο�.


    public DishState ds;          // ��׸� ���� Ȯ��
    public List<GameObject> tiles = new List<GameObject>();        // ��ȸ ������ �� �̵� ������ Ÿ��
    public List<GameObject> tilesInRange = new List<GameObject>(); // ����� �ֺ� ���� �Ÿ� �� Ÿ��

    [SerializeField]
    private List<string> CatType;

    [System.Serializable]
    public class CatStats
    {
        public string personality;
        public float sleepy;
        public float friendly;
        public float mood;
        public float hunger;
        public float moveTerm;
        public float moveRange;
        public float discharge;
        public float metabolism;
        public float weight;
        public float speed;
        public float eatSpeed;
        public int CatIndex;
        public float age;
        public bool male;

        public CatStats(string personality, float sleepy, float friendly, float mood, float hunger, float moveTerm,
            float moveRange, float discharge, float metabolism, float weight, float speed,
            float eatSpeed, int CatIndex, bool male,float age)
        {
            this.personality = personality;
            this.sleepy = sleepy;
            this.friendly = friendly;
            this.mood = mood;
            this.hunger = hunger;
            this.moveTerm = moveTerm;
            this.moveRange = moveRange;
            this.discharge = discharge;
            this.metabolism = metabolism;
            this.weight = weight;
            this.speed = speed;
            this.eatSpeed = eatSpeed;
            this.CatIndex = CatIndex;
            this.male = male;
            this.age = age;
        }
    }

    private CatData catData;
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
        catData = GameObject.Find("GameManager").GetComponent<CatData>();
        interaction = gameObject.GetComponentInChildren<PetInteraction>();
        player = GameObject.Find("Player");
        sa = gameObject.GetComponent<SkeletonAnimation>();
        SetCatSkin(CatIndex);
        state = CatState.Wandering; // �ʱ� ���� ����
        StartCoroutine(StateController());
        rest = false; // �ʱ⿡�� ���� ����
        StartCoroutine(SaveCatDataPeriodically());

        LoadCatStatsFromCatData();
    }

    public void SetCatSkin(int index)
    {
        Debug.Log("��Ų���� �����");
        sa.skeleton.SetSkin(CatType[index]);
        sa.skeleton.SetSlotsToSetupPose();
        sa.AnimationState.Apply(sa.skeleton);

        Debug.Log(CatType[index]);
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
        if (friendly > 120)
        {
            friendly = 100;
        }
        if (mood > 150)
        {
            mood = 150;
        }
        if (isFat)
        {

            speed = 1.3f;
            if (CatIndex == 0)
            {
                SetCatSkin(3);
            }
            else if (CatIndex == 1)
            {
                SetCatSkin(4);
            }
            else if (CatIndex == 2)
            {
                SetCatSkin(5);
            }
        }
        else
        {
            speed = 1f;

            if (CatIndex == 0)
            {
                SetCatSkin(0);
            }
            else if (CatIndex == 1)
            {
                SetCatSkin(1);
            }
            else if (CatIndex == 2)
            {
                SetCatSkin(2);
            }
        }

        if (weight >= 6)
        {
            isFat = true;
        }

        if (controller.isZoomedIn)
        {
            state = CatState.PlayerCalled;
        }


        if (player == null)
        {
            player = GameObject.Find("Player");
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
            sleepy -= Time.deltaTime * metabolism * 0.5f;   // Ȱ�� �� ����屸 ����
        }
        else
        {
            sleepy += Time.deltaTime * metabolism * 1.5f;  // ���� �� ����屸 ����
        }

        // ���� �屸 �ִ�ġ ���� �� ���� ����
        if (sleepy > 100)
        {
            // sleepy = 100;
            state = CatState.Wandering; // ��ȸ ���·� ����
            rest = false;
        }
        if (hunger > 20)
        {
            discharge += Time.deltaTime * metabolism * 0.01f;
        }

        if (!rest && starving)
        {
            mood -= Time.deltaTime;
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
                    yield return StartCoroutine(LetsPlay());
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

        int ran = Random.Range(0, 5);
        if (ran > 3 && mood > 80 && friendly > 80 && hunger > 70)
        {
            state = CatState.WantToPlay;
        }
        else
        {
            state = CatState.Wandering;
        }

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
        //StopAllCoroutines();

        if (!controller.isZoomedIn)//�ܾƿ��̶��
        {
            state = CatState.Idle; // ���� �� �⺻ ���·� ����
        }
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
    public IEnumerator LetsPlay()
    {
        Vector3 targetPosition = player.transform.position;
        float playerDistance = Vector3.Distance(targetPosition, transform.position);

        if (playerDistance > 1)
        {
            anim.AnimationName = "Walking";
            float duration = speed;
            float elapsed = 0f;
            Vector3 startingPosition = transform.position;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            anim.AnimationName = "Butt";//�����ϴ�ǥ��
            interaction.LetsPlay();
            yield return new WaitForSeconds(5);
            transform.position = targetPosition;
            state = CatState.Idle;

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
            anim.AnimationName = "Play"; //�չ� �����̴°ŷ� �ٲ�ߵ�.
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
        float duration = speed;
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
        anim.AnimationName = "Jump";

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        anim.AnimationName = "Idle";
        state = CatState.Sleeping; // Sleeping ���·� ��ȯ

        transform.position = targetPosition;
    }


    private IEnumerator SaveCatDataPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f); // 10�� ���
            SaveCatStatsToCatData();
        }
    }

    public void SaveCatStatsToCatData()
    {
        if (catData != null)
        {
            catData.catStats = new CatData.CatStats(
                personality,
                sleepy,
                friendly,
                mood,
                hunger,
                moveTerm,
                moveRange,
                discharge,
                metabolism,
                weight,
                speed,
                eatSpeed,
                CatIndex,
                male,
                age
            );

            Debug.Log("Cat stats have been saved to CatData.");
        }
    }

    public void LoadCatStatsFromCatData()
    {
        if (catData != null && catData.catStats != null)
        {
            CatData.CatStats stats = catData.catStats;

            // CatAIFSM�� ������ CatStats �� �Ҵ�
            personality = stats.personality;
            sleepy = stats.sleepy;
            friendly = stats.friendly;
            mood = stats.mood;
            hunger = stats.hunger;
            moveTerm = stats.moveTerm;
            moveRange = stats.moveRange;
            discharge = stats.discharge;
            metabolism = stats.metabolism;
            weight = stats.weight;
            speed = stats.speed;
            eatSpeed = stats.eatSpeed;
            CatIndex = stats.CatIndex;
            age = stats.age;
            male = stats.male;

            Debug.Log("Cat stats have been loaded from CatData.");
        }
    }



}
