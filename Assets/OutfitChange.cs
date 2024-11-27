using Spine.Unity;
using UnityEngine;

public class OutfitChange : MonoBehaviour
{
    private SkeletonAnimation sa;

    void Start()
    {
        // SkeletonAnimation ������Ʈ�� �����ɴϴ�.
        sa = GetComponent<SkeletonAnimation>();

        // �ʱ� ���� ���� (�ʿ� ��)
        sa.initialSkinName = "1";
        sa.Initialize(true); // Skeleton �����͸� �ʱ�ȭ
    }

    void Update()
    {
        // Ű �Է¿� ���� ������ ����
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeSkin("1"); // ���� 1�� ����
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangeSkin("2"); // ���� 2�� ����
        }
    }

    /// <summary>
    /// ����(Skin)�� �����ϴ� �޼���
    /// </summary>
    /// <param name="skinName">������ Skin �̸�</param>
    private void ChangeSkin(string skinName)
    {
        if (sa.Skeleton != null)
        {
            sa.Skeleton.SetSkin(skinName); // ���ο� Skin ����
            sa.Skeleton.SetSlotsToSetupPose(); // ������ ��� �ʱ�ȭ
            sa.AnimationState.Apply(sa.Skeleton); // ���� ���� ����
        }
        else
        {
            Debug.LogWarning("Skeleton�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
        }
    }
}
