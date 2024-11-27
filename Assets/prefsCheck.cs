using Spine.Unity;
using UnityEngine;

public class prefsCheck : MonoBehaviour
{
    public GameObject player;           // �÷��̾� GameObject
    public SkeletonAnimation sk;       // SkeletonAnimation ������Ʈ
    public string skinnum;
    // Start is called before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // SkeletonAnimation ������Ʈ ��������
        sk = player.GetComponent<SkeletonAnimation>();

        // "selectedCharacter" Ű ���� �ִ��� Ȯ��
        if (PlayerPrefs.HasKey("selectedCharacter"))
        {
            // ����� �� ��������
            int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
            Debug.Log("Selected Character: " + selectedCharacter);

            // Spine�� ��Ų ����
            SetInitialSkin(selectedCharacter);
        }
        else
        {
            Debug.Log("No character selected yet!");
        }
    }

    /// <summary>
    /// Spine ���̷��� �ִϸ��̼��� ��Ų�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="characterIndex">���õ� ĳ������ �ε���</param>
    void SetInitialSkin(int characterIndex)
    {
        // ��: ĳ���� �̸��� ��Ų�� ���� (���ڸ� ���ڿ��� ��ȯ)
        string skinName = characterIndex.ToString();

        // Spine�� SetSkin�� �̿��� ��Ų ����
        
            sk.Skeleton.SetSkin(skinName);
            sk.Skeleton.SetSlotsToSetupPose(); // ��Ų ���� �� ���� ������Ʈ
            Debug.Log("Skin changed to: " + skinName);
       
    }
}
