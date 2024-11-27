using Spine.Unity;
using UnityEngine;

public class prefsCheck : MonoBehaviour
{
    public GameObject player;           // 플레이어 GameObject
    public SkeletonAnimation sk;       // SkeletonAnimation 컴포넌트
    public string skinnum;
    // Start is called before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // SkeletonAnimation 컴포넌트 가져오기
        sk = player.GetComponent<SkeletonAnimation>();

        // "selectedCharacter" 키 값이 있는지 확인
        if (PlayerPrefs.HasKey("selectedCharacter"))
        {
            // 저장된 값 가져오기
            int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
            Debug.Log("Selected Character: " + selectedCharacter);

            // Spine의 스킨 변경
            SetInitialSkin(selectedCharacter);
        }
        else
        {
            Debug.Log("No character selected yet!");
        }
    }

    /// <summary>
    /// Spine 스켈레톤 애니메이션의 스킨을 설정하는 함수
    /// </summary>
    /// <param name="characterIndex">선택된 캐릭터의 인덱스</param>
    void SetInitialSkin(int characterIndex)
    {
        // 예: 캐릭터 이름을 스킨에 매핑 (숫자를 문자열로 변환)
        string skinName = characterIndex.ToString();

        // Spine의 SetSkin을 이용해 스킨 설정
        
            sk.Skeleton.SetSkin(skinName);
            sk.Skeleton.SetSlotsToSetupPose(); // 스킨 변경 후 슬롯 업데이트
            Debug.Log("Skin changed to: " + skinName);
       
    }
}
