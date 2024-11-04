using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AvatarManager avatarManager;

    private void Start()
    {
        // ���� ���� �� ������ �ε�
        avatarManager.LoadCharacterCustomizationData();
    }

    public void OnCustomizationComplete()
    {
        // Ŀ���͸���¡ �Ϸ� �� ������ ����
        avatarManager.SaveCharacterCustomizationData();
    }

    private void OnApplicationQuit()
    {
        // ���� ���� �� ������ ����
        avatarManager.SaveCharacterCustomizationData();
    }

    public void OnSwitchToGameScene()
    {
        // ���� ������ ��ȯ �� ������ �ε�
        avatarManager.LoadCharacterCustomizationData();
        // ���� �� ��ȯ �ڵ� �߰�
    }
}
