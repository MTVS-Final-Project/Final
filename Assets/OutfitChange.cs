using Spine.Unity;
using UnityEngine;

public class OutfitChange : MonoBehaviour
{
    private SkeletonAnimation sa;

    void Start()
    {
        // SkeletonAnimation 컴포넌트를 가져옵니다.
        sa = GetComponent<SkeletonAnimation>();

        // 초기 외형 설정 (필요 시)
        sa.initialSkinName = "1";
        sa.Initialize(true); // Skeleton 데이터를 초기화
    }

    void Update()
    {
        // 키 입력에 따라 외형을 변경
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeSkin("1"); // 외형 1로 변경
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangeSkin("2"); // 외형 2로 변경
        }
    }

    /// <summary>
    /// 외형(Skin)을 변경하는 메서드
    /// </summary>
    /// <param name="skinName">변경할 Skin 이름</param>
    private void ChangeSkin(string skinName)
    {
        if (sa.Skeleton != null)
        {
            sa.Skeleton.SetSkin(skinName); // 새로운 Skin 설정
            sa.Skeleton.SetSlotsToSetupPose(); // 슬롯의 포즈를 초기화
            sa.AnimationState.Apply(sa.Skeleton); // 변경 사항 적용
        }
        else
        {
            Debug.LogWarning("Skeleton이 초기화되지 않았습니다.");
        }
    }
}
