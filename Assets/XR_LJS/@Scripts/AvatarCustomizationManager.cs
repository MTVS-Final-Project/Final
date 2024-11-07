using UnityEngine;
using System.IO;

public class AvatarManager : MonoBehaviour
{
    public CharacterCustomizationData characterData; // 커스터마이징 데이터

    private void Awake()
    {
    }

    // 데이터를 Resources/Player 폴더에 저장하는 메소드
    public void SaveCharacterCustomizationData()
    {
        string directoryPath = Application.dataPath + "/Resources/Player";
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath); // 폴더가 존재하지 않으면 생성
        }

        string path = directoryPath + "/characterData.json";
        string json = characterData.ToJson(); // CharacterCustomizationData에서 JSON으로 변환
        File.WriteAllText(path, json);
        Debug.Log("Character customization data saved to " + path);
    }


    // Resources에서 데이터를 로드하는 메소드
    public void LoadCharacterCustomizationData()
    {
        string path = Application.dataPath + "/Resources/Player/characterData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            characterData = CharacterCustomizationData.FromJson(json); // JSON에서 CharacterCustomizationData 객체로 변환
            Debug.Log("Character customization data loaded from " + path);
        }
        else
        {
            Debug.LogError("Character customization data file not found!");
        }
    }


}
