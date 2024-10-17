using UnityEngine;

public class MakePlane : MonoBehaviour
{
    public GameObject quad;

    public int Xaxis = 5;
    public int Yaxis = 5;


    void Start()
    {
        //가로세로 1인 쿼드를 가지고 바닥을 깔고싶다.
        //0,0,0부터 1씩 증가시키면서 함
        for (int i = 0; i < Xaxis; i++)
        {
            GameObject go = Instantiate(quad);
            go.transform.position = new Vector3(i, 0, 0);

            for (int j = 0; j < Yaxis; j++)
            {
                GameObject go1 = Instantiate(quad);
                go1.transform.position = new Vector3(i, 0, j);
            }
        }

        //쿼드 피벗 중심으로부터 X축으로 1씩 가면서 쿼드 깔기
        //정해진 수 만큼 쿼드가 깔렸으면 Z축으로 1 올라간다음 반복
        //그렇게 가로 세로 쿼드가 다 깔릴때까지 반복

        //나중에는 서버에서 가로세로 몇칸 어디에 뭐가 깔렸는지 받아와야함
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
