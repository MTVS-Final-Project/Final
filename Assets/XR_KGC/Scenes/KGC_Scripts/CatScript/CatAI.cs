using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
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
    public Transform towerBottom; //Ÿ�� �ö󰡱��� ��ġ

    public float sleepy = 100; // ����屸
    public float friendly = 100f; // ��ȣ��
    public float mood = 100; // ���
    public float hunger = 100f; // ����� ��ġ
    public float moveTerm = 5; // �󸶳� ���� �����̴���, Ȱ������ ���
    public float moveRange = 2; // �ѹ��� �ִ� �󸶳� �ָ� ������.
    public float discharge = 0; // ȭ��� ��� �屸

    public float speed = 1; //����� �ӵ�, ���,�����¿� ���� �ӵ��� �޶���.���ƾ� ����
    public float eatSpeed = 1;//��Դ¼ӵ�.������ ��������

    public DishState dishState;//��׸� ���ֳ� ���� Ȯ��
    public List<GameObject> tiles = new List<GameObject>(); // ��ȸ�ϴ� ������ �� ���� ����� Ÿ�� �� occupied�� �ƴ� ������ �̵�
    public List<GameObject> tilesInRange = new List<GameObject>(); // ��������׼� ���� �̳� Ÿ���� ã�� �Լ�

    public bool rest; // ����� ���� ���� ��.
    

    public enum CatState
    {
        Normal,        // �⺻����
        Hungry,      // ����� ��
        Sleepy,     // �ڰ� ���� ��
        Joyfull,     // �� ���ƴٴϴ� ����
        PlayerCalled // �÷��̾ �ҷ��� ��
    }
    public CatState state;

    void Start()
    {
        StartCoroutine(Wandering(moveTerm));
    }

    void Update()
    {
        if (controller == null)
        {
            GetComponent<CatController>();
        }

        if (tiles.Count <= 0) // ����� �ֺ� Ÿ�� ã��
        {
            GameObject tParent = GameObject.Find("TileParent");
            foreach (Transform child in tParent.transform)
            {
                tiles.Add(child.gameObject);
            }
        }

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

        hunger -= Time.deltaTime;
        if (!rest)
        {
        sleepy -= Time.deltaTime;
        }
        else
        {sleepy += Time.deltaTime; }



        if (Input.GetKeyDown(KeyCode.Y))
        {
            GoTower();
        }
    }

    public void GoMeal()
    {
        StopAllCoroutines();
        StartCoroutine(ToMeal(dish.transform.position, speed));
    }
    public void GoTower()
    {
        StopAllCoroutines();
        StartCoroutine(ToTower(towerBottom.position,speed));
    }

    public IEnumerator ToTower(Vector3 targetPosition,float duration) //Ÿ�� ����� ���� �ö󰡴� �ڷ�ƾ
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
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(JumpUp(tower.position));

        anim.AnimationName = "Sit";
        rest = true;
    }
    public IEnumerator ToMeal(Vector3 targetPosition,float duration) //������� ���� �ڷ�ƾ
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
        if (dishState.mealCount > 0)
        {

        }

    }
    //public IEnumerator Eat()
    //{
        
    //}

    public IEnumerator Wandering(float Term) // �÷��̾ �θ� ���� �ߴܽ��Ѿ� �Ǵ� �ڷ�ƾ
    {
        yield return new WaitForSeconds(Term);
        tilesInRange.Clear();
        for (int i = 0; i < tiles.Count; i++)
        {
            if (Vector3.Distance(transform.position, tiles[i].transform.position) < moveRange && !tiles[i].GetComponent<FloorScript>().occupied)
            {
                tilesInRange.Add(tiles[i]); // Ÿ�� �߰�
            }
        }
        if (tilesInRange.Count > 0)
        {
            yield return StartCoroutine(MoveTowards(tilesInRange[Random.Range(0, tilesInRange.Count)].transform.position));
        }
        StartCoroutine(Wandering(moveTerm));
    }

    public IEnumerator MoveTowards(Vector3 targetPosition) //Ư����ǥ ��ġ�� �̵��ϴ� ��ũ��Ʈ
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

    public IEnumerator JumpUp(Vector3 targetPosition) //������?�� ���̴� ��ũ��Ʈ. �׳� �̵��� ����
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
