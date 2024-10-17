using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance; // Singleton 인스턴스
    public Data data; // 플레이어 데이터

    private void Awake()
    {
        // 이미 인스턴스가 존재하는지 확인
        if (Instance == null)
        {
            Instance = this; // 인스턴스 설정
            data = new Data(); // 데이터 초기화
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 기존 인스턴스 삭제
        }
    }

    public string AvatarToString()
    {
        return JsonUtility.ToJson(data);
    }
}

[System.Serializable]
public class Data
{
    // Room 
    public int myNumberInRoom;

    // 아바타
    public int skin;
    public Color skinColor;
    public int Eyes;
    public int hair;
    public int body;
    public int pant;
    public int rightArm;
    public int leftArm;
    public int rightleg;
    public int leftLeg;
    public int rightShoes;
    public int leftShoes;

    // 악세서리 
    public int hat;
}
