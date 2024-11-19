using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    //바닥 게임오브젝트
    public GameObject floor;
    public GameObject TileParent;

    public List<float> Xpos = new List<float>();
    public List<float> Ypos = new List<float>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TileParent가 null이라면 Find로 검색
        if (TileParent == null)
        {
            TileParent = GameObject.Find("TileParent");
            FloorPosCheck();
        }

        // TileParent가 존재할 경우 자식 순회
      
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadFloor();
        }
    }

    public void FloorPosCheck()
    {
        if (TileParent != null)
        {
            // 리스트 초기화 (중복 추가 방지)
            Xpos.Clear();
            Ypos.Clear();

            // 자식 순회하며 X, Y 값 추가
            foreach (Transform child in TileParent.transform)
            {
                // 자식의 position 값 가져오기
                Vector3 childPosition = child.position;

                // X와 Y 값을 리스트에 추가 (소수점 제거를 위해 int 변환)
                Xpos.Add(childPosition.x);
                Ypos.Add(childPosition.y);
            }
        }
    }

    public void LoadFloor()
    {
        // Xpos 리스트의 데이터 수만큼 순환
        for (int i = 0; i < Xpos.Count; i++)
        {
            // 새로운 floor 게임 오브젝트 생성
            GameObject go = Instantiate(floor);

            // 이름 설정 (optional)
            go.name = $"floor_{i}";
            go.transform.parent = TileParent.transform;

            // 생성된 게임 오브젝트의 위치 설정
            go.transform.position = new Vector3(Xpos[i], Ypos[i], 0f);
        }
    }

}
