using Photon.Pun;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelNext : MonoBehaviourPunCallbacks
{
    private int selectedSprite = 0;  // 기본으로 선택된 스프라이트 인덱스
    public Button saveButton;  // 세이브 버튼
    public Button loadButton;  // 로드 버튼

    // Start 메서드: 버튼 클릭 이벤트 연결
    void Start()
    {

        // 로드 버튼 클릭 시 Load 메서드 호출
        loadButton.onClick.AddListener(LoadAndApplySprite);

        // 디버그 로그: 게임 시작 시
        Debug.Log("LevelNext 스크립트가 시작되었습니다. 초기 선택된 스프라이트 인덱스: " + selectedSprite);
    }

   

    // 로드 버튼을 클릭하면 호출되는 메서드
    public void LoadAndApplySprite()
    {
        // 로드 후 씬 전환
        Debug.Log("'Room_KGC' 씬으로 이동합니다.");
        SceneManager.LoadScene("Room_KGC");  // "Room_KGC" 씬으로 이동
    }
}
