using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CatAI : MonoBehaviour
{
    //����� �ൿ ������ ��ũ��Ʈ ���⼭ ������.

    //�÷��̾ ���� ȣ����
    //������� ���� �Ķ���͵�.
    //����� �����, ����� ���. ������� ��а� �÷��̾��� ȣ������ ���� ��ȣ�ۿ��ҿ���.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //cat pos manager�� �����ؼ� ���⼭ �ൿ�� ���ϸ� �� ��ũ��Ʈ���� ��ġ�����ҵ�
    public CatPosManager cpm;
    public CatController controller;
    public SkeletonAnimation anim;

    public float friendly = 100f;//��ȣ��
    public float mood = 100; //���
    public float hunger = 100f; //����� ��ġ
    public float moveTerm = 5; //�󸶳� ���� �����̴���,Ȱ������ ���
    public float moveRange = 2; //�ѹ��� �ִ� �󸶳� �ָ� ������.

    //����� ��׸� ���� Ȯ��
    public DishState dish;

    public List<GameObject> tiles = new List<GameObject>();//��ȸ�ϴ� �����϶��� ���� ����� Ÿ���� occupied�� �ƴѰ����� �̵�
    public List<GameObject> tilesInRange = new List<GameObject>();//��������׼� �����̳� Ÿ���� ã���Լ�



    void Start()
    {
        
        StartCoroutine(Wandering(moveTerm)); 
    }

    // Update is called once per frame
    void Update()
    {
        if (cpm == null)
        {
            GetComponent<CatPosManager>();
        }
        if (controller == null)
        {
            GetComponent<CatController>();
        }

        if (tiles.Count <= 0)
        {
            GameObject tParent = GameObject.Find("TileParent");
            foreach (Transform child in tParent.transform)
            {
                tiles.Add(child.gameObject);
            }

        }

        hunger -= Time.deltaTime;
        //���ƴٴϴٰ� ��������� �����������
    }

    public IEnumerator Wandering(float Term)  //�÷��̾ �θ����� �ߴܽ��ѾߵǴ� �ڷ�ƾ
    {
        //��������׼� �����Ÿ� �̳��� Ÿ�ϵ��� ã�Ƽ� �� ��ġ�� �̵��ϴ� �Լ�
        yield return new WaitForSeconds(Term);
        tilesInRange.Clear();
        for (int i = 0; i < tiles.Count; i++)
        {
            //print(Vector3.Distance(transform.position, tiles[i].transform.position));
            if (Vector3.Distance(transform.position, tiles[i].transform.position) < moveRange && !tiles[i].GetComponent<FloorScript>().occupied)//Ÿ���� moverange�̳��� �ִٸ�
            {
                tilesInRange.Add(tiles[i]); //Ÿ���߰�
            }
        }
        controller.CatGo(tilesInRange[Random.Range(0, tilesInRange.Count)].transform);

        StartCoroutine(Wandering(moveTerm));

    }
}
