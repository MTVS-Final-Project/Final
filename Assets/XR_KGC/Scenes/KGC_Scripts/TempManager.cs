using System.Collections.Generic;
using UnityEngine;

public class TempManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static TempManager Instance;
    //플레이어가 가진 모든 정보들을 관리해야되는 스크립트
    //

    //플레이어의 커스텀 값  
    //플레이어의 고양이 종류,그 고양이가 가진 값들.고양이 성격,플레이어에 대한 호감도 등
    //플레이어가 가진 토큰,플레이어가 구입한 가구,플레이어가 가진 아이템
    //플레이어 가구들의 상태 ,고양이 밥그릇이 차있나 줄어들었나, 화장실이 비워져있나 등등
    //플레이어가 수락한 퀘스트들.

    //플레이어의 재산
    public int tokne;

    // 가구 데이터 리스트 (주석 제거),플레이어의 집에 배치된 가구들의 정보
    public List<GaguSave.GaguData> gaguDataList = new List<GaguSave.GaguData>();

    void Awake()
    {
        // 이미 인스턴스가 존재하는 경우 이 오브젝트를 파괴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 인스턴스가 없는 경우 이 오브젝트를 인스턴스로 설정하고 파괴되지 않도록 설정
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
