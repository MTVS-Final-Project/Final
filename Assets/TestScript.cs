using Spine;
using Spine.Unity;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public SkeletonAnimation skeleton; // SkeletonAnimation 연결
    private Skeleton sk;

    void Start()
    {
        // Skeleton 객체 초기화
        InitializeSkeleton();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Switching to GrayNyang...");
            SetSkin("whiteCat/GrayNyang");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Switching to WhiteNyang...");
            SetSkin("whiteCat/WhiteNyang");
        }
    }

    public void SetSkin(string skinName)
    {
        if (sk == null)
        {
            Debug.LogError("Skeleton is not initialized! Reinitializing...");
            InitializeSkeleton(); // Skeleton을 재초기화
        }

        // 스킨 변경
        sk.SetSkin(skinName);
        sk.SetSlotsToSetupPose(); // 슬롯 초기화
        skeleton.AnimationState.Apply(sk); // 변경된 스킨을 애니메이션 상태에 적용

        Debug.Log($"Skin changed to: {skinName}");
    }

    private void InitializeSkeleton()
    {
        if (skeleton == null)
        {
            Debug.LogError("SkeletonAnimation is not assigned!");
            return;
        }

        // SkeletonAnimation에서 Skeleton 객체 가져오기
        sk = skeleton.skeleton;

        if (sk == null)
        {
            Debug.LogError("Failed to initialize Skeleton!");
        }
        else
        {
            Debug.Log("Skeleton initialized successfully.");
        }
    }
}
