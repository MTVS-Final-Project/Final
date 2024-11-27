using UnityEngine;

public class CatRotation : MonoBehaviour
{
    private Vector3 lastpos; // 이전 위치를 저장하는 Vector3

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 초기화: 현재 위치를 이전 위치로 설정
        lastpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 이동 벡터 계산
        Vector3 nowpos = transform.position; // 현재 위치
        Vector3 moveVector = nowpos - lastpos;

        // X축의 값이 양수인지 음수인지 확인
        if (moveVector.x > 0)
        {
            //Debug.Log("X축 이동: 양수 (오른쪽으로 이동)");
            transform.rotation = Quaternion.Euler(0, 180, 0); // 오른쪽을 보는 방향
        }
        else if (moveVector.x < 0)
        {
           // Debug.Log("X축 이동: 음수 (왼쪽으로 이동)");
            transform.rotation = Quaternion.Euler(0, 0, 0); // 왼쪽을 보는 방향
        }
        else
        {
           // Debug.Log("X축 이동 없음");
        }

        // 현재 위치를 이전 위치로 갱신
        lastpos = nowpos;
    }
}
