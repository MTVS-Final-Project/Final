using Photon.Pun;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelNext : MonoBehaviourPunCallbacks
{
    private int selectedSprite = 0;  // 기본으로 선택된 스프라이트 인덱스
    public List<Sprite> playerSprites = new List<Sprite>();  // 플레이어 스프라이트 리스트
    public Button saveButton;  // 세이브 버튼
    public Button loadButton;  // 로드 버튼

    // Start 메서드: 버튼 클릭 이벤트 연결
    void Start()
    {
        // 세이브 버튼 클릭 시 Save 메서드 호출
        saveButton.onClick.AddListener(SaveAndPrepareForSceneChange);

        // 로드 버튼 클릭 시 Load 메서드 호출
        loadButton.onClick.AddListener(LoadAndApplySprite);

        // 디버그 로그: 게임 시작 시
        Debug.Log("LevelNext 스크립트가 시작되었습니다. 초기 선택된 스프라이트 인덱스: " + selectedSprite);
    }

    // 세이브 버튼을 클릭하면 호출되는 메서드
    public void SaveAndPrepareForSceneChange()
    {
        // 디버그 로그: 세이브 버튼 클릭 시
        Debug.Log("세이브 버튼이 클릭되었습니다. 선택된 스프라이트 인덱스: " + selectedSprite);

        // 세이브 경로
        string filePath = Application.persistentDataPath + "/PlayerData.txt";  // 저장 경로 (쓰기 가능한 경로로 수정)
        Debug.Log("선택된 스프라이트 인덱스(" + selectedSprite + ")를 파일에 저장합니다. 경로: " + filePath);

        // 선택된 스프라이트 인덱스를 파일에 저장
        File.WriteAllText(filePath, selectedSprite.ToString());

        // 선택된 스프라이트를 현재 게임 오브젝트에 적용 (SpriteRenderer에 적용)
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null && playerSprites.Count > selectedSprite)
        {
            spriteRenderer.sprite = playerSprites[selectedSprite];
            Debug.Log("선택된 스프라이트가 적용되었습니다: " + playerSprites[selectedSprite].name);
        }
        else
        {
            Debug.LogWarning("SpriteRenderer가 없거나 선택된 스프라이트가 유효하지 않습니다.");
        }

        // 세이브 완료 후 씬 전환 준비
        Debug.Log("세이브가 완료되었습니다. '다음 씬' 버튼을 클릭하여 씬을 전환하세요.");
    }

    // 로드 버튼을 클릭하면 호출되는 메서드
    public void LoadAndApplySprite()
    {
        // 로드 후 씬 전환
        Debug.Log("'Room_KGC' 씬으로 이동합니다.");
        SceneManager.LoadScene("Room_KGC");  // "Room_KGC" 씬으로 이동
    }
}
