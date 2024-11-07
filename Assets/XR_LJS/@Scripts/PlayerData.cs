using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }
    public Data data;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // 중복된 인스턴스 제거
            return;
        }
        Instance = this;
        data = new Data();
    }

    // JSON 변환 메서드들
    public string AvatarToString()
    {
        return JsonUtility.ToJson(data);
    }

    public void LoadFromJson(string jsonData)
    {
        data = JsonUtility.FromJson<Data>(jsonData);
    }
}

[System.Serializable]
public class Data
{
    // Room 정보
    public int myNumberInRoom;

    // 기본 외형
    [Header("Basic Appearance")]
    public int skin;
    public Color skinColor;
    public int eyes;  // 변수명 소문자로 시작
    public int hair;

    // 의상
    [Header("Clothing")]
    public int body;
    public int pant;

    // 팔다리
    [Header("Limbs")]
    public int rightArm;
    public int leftArm;
    public int rightLeg;
    public int leftLeg;

    // 신발
    [Header("Shoes")]
    public int rightShoes;
    public int leftShoes;

    // 액세서리
    [Header("Accessories")]
    public int hat;

    // 기본값 설정을 위한 생성자
    public Data()
    {
        // 기본값 초기화
        myNumberInRoom = -1;
        skin = 0;
        skinColor = Color.white;
        eyes = 0;
        hair = 0;
        body = 0;
        pant = 0;
        rightArm = 0;
        leftArm = 0;
        rightLeg = 0;
        leftLeg = 0;
        rightShoes = 0;
        leftShoes = 0;
        hat = 0;
    }
}