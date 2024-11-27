using Spine;
using Spine.Unity;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public SkeletonAnimation skeleton; // SkeletonAnimation ����
    private Skeleton sk;

    void Start()
    {
        // Skeleton ��ü �ʱ�ȭ
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
            InitializeSkeleton(); // Skeleton�� ���ʱ�ȭ
        }

        // ��Ų ����
        sk.SetSkin(skinName);
        sk.SetSlotsToSetupPose(); // ���� �ʱ�ȭ
        skeleton.AnimationState.Apply(sk); // ����� ��Ų�� �ִϸ��̼� ���¿� ����

        Debug.Log($"Skin changed to: {skinName}");
    }

    private void InitializeSkeleton()
    {
        if (skeleton == null)
        {
            Debug.LogError("SkeletonAnimation is not assigned!");
            return;
        }

        // SkeletonAnimation���� Skeleton ��ü ��������
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
