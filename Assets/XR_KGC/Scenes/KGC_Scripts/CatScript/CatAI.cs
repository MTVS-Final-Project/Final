using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CatAI : MonoBehaviour
{
    //고양이 행동 전반적 스크립트 여기서 쓸거임.

    //플레이어에 대한 호감도
    //고양이의 성격 파라미터들.
    //고양이 배고픔, 고양이 기분. 고양이의 기분과 플레이어의 호감도는 서로 상호작용할예정.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //cat pos manager와 연동해서 여기서 행동을 정하면 그 스크립트에서 위치제어할듯
    public CatPosManager cpm;
    public CatController controller;
    public SkeletonAnimation anim;

    public float friendly = 100f;//우호도
    public float mood = 100; //기분
    public float hunger = 100f; //배고픔 수치
    public float moveTerm = 5; //얼마나 자주 움직이는지,활동성과 엮어서
    public float moveRange = 2; //한번에 최대 얼마나 멀리 가는지.

    //고양이 밥그릇 상태 확인
    public DishState dish;

    public List<GameObject> tiles = new List<GameObject>();//배회하는 상태일때는 가장 가까운 타일중 occupied가 아닌곳으로 이동
    public List<GameObject> tilesInRange = new List<GameObject>();//고양이한테서 일정이내 타일을 찾는함수



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
        //돌아다니다가 배고파지면 밥먹으러가기
    }

    public IEnumerator Wandering(float Term)  //플레이어가 부를때는 중단시켜야되는 코루틴
    {
        //고양이한테서 일정거리 이내의 타일들을 찾아서 그 위치로 이동하는 함수
        yield return new WaitForSeconds(Term);
        tilesInRange.Clear();
        for (int i = 0; i < tiles.Count; i++)
        {
            //print(Vector3.Distance(transform.position, tiles[i].transform.position));
            if (Vector3.Distance(transform.position, tiles[i].transform.position) < moveRange && !tiles[i].GetComponent<FloorScript>().occupied)//타일이 moverange이내에 있다면
            {
                tilesInRange.Add(tiles[i]); //타일추가
            }
        }
        controller.CatGo(tilesInRange[Random.Range(0, tilesInRange.Count)].transform);

        StartCoroutine(Wandering(moveTerm));

    }
}
