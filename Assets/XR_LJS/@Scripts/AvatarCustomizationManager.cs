using UnityEngine;
using System.IO;

public class AvatarManager : MonoBehaviour
{
    public CharacterCustomizationData characterData; // Ŀ���͸���¡ ������

    private void Awake()
    {
    }

    // �����͸� Resources/Player ������ �����ϴ� �޼ҵ�
    public void SaveCharacterCustomizationData()
    {
        string directoryPath = Application.dataPath + "/Resources/Player";
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath); // ������ �������� ������ ����
        }

        string path = directoryPath + "/characterData.json";
        string json = characterData.ToJson(); // CharacterCustomizationData���� JSON���� ��ȯ
        File.WriteAllText(path, json);
        Debug.Log("Character customization data saved to " + path);
    }


    // Resources���� �����͸� �ε��ϴ� �޼ҵ�
    public void LoadCharacterCustomizationData()
    {
        string path = Application.dataPath + "/Resources/Player/characterData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            characterData = CharacterCustomizationData.FromJson(json); // JSON���� CharacterCustomizationData ��ü�� ��ȯ
            Debug.Log("Character customization data loaded from " + path);
        }
        else
        {
            Debug.LogError("Character customization data file not found!");
        }
    }


}
