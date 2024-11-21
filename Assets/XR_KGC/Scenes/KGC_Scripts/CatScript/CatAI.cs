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

    public float friendly = 100f;//우호도
    public float mood = 100; //기분
    public float hunger = 100f; //배고픔 수치
    public float moveTerm = 5; //얼마나 자주 움직이는지


    public List<GameObject> tiles = new List<GameObject>();//배회하는 상태일때는 가장 가까운 타일중 occupied가 아닌곳으로 이동

    //여기서 움직일때는 catcontroller의 모든 코루틴을 정지시킨후 실행하는게 좋을듯

    void Start()
    {
        if (cpm == null)
        {
            GetComponent<CatPosManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
       if (tiles.Count <= 0)
        {
            GameObject tParent = GameObject.Find("TileParent");
            foreach (Transform child in tParent.transform)
            {
                tiles.Add(child.gameObject);
            }

        }
    }

    public IEnumerator Wandering(float Term)
    {
        
        yield return new WaitForSeconds(Term);
    }
}
