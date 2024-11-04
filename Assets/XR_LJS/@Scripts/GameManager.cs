using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AvatarManager avatarManager;

    private void Start()
    {
        // 게임 시작 시 데이터 로드
        avatarManager.LoadCharacterCustomizationData();
    }

    public void OnCustomizationComplete()
    {
        // 커스터마이징 완료 시 데이터 저장
        avatarManager.SaveCharacterCustomizationData();
    }

    private void OnApplicationQuit()
    {
        // 게임 종료 시 데이터 저장
        avatarManager.SaveCharacterCustomizationData();
    }

    public void OnSwitchToGameScene()
    {
        // 게임 씬으로 전환 시 데이터 로드
        avatarManager.LoadCharacterCustomizationData();
        // 이후 씬 전환 코드 추가
    }
}
