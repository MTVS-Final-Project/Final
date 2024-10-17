using UnityEngine;

[System.Serializable]
public class CharacterCustomizationData
{
    public int hairStyle;
    public int eyeStyle;
    public int mouthStyle;
    public int bodyStyle;
    public int pantStyle;
    public int leftArmStyle;
    public int rightArmStyle;
    public int leftLegStyle;
    public int rightLegStyle;


    // 각 부위의 가능한 스타일 개수
    private const int HairStylesCount = 2; // 예: 0, 1, 2
    private const int EyeStylesCount = 2;  // 예: 0, 1, 2
    private const int MouthStylesCount = 2; // 예: 0, 1, 2
    private const int BodyStylesCount = 2;
    private const int PantStylesCount = 2;
    private const int LeftArmStylesCount = 2;
    private const int RightArmStylesCount = 2;
    private const int leftLegStylesCount = 2;
    private const int rightLegStylesCount = 2;


    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public static CharacterCustomizationData FromJson(string json)
    {
        return JsonUtility.FromJson<CharacterCustomizationData>(json);
    }

    public static CharacterCustomizationData CreateRandom()
    {
        return new CharacterCustomizationData
        {
            hairStyle = Random.Range(0, HairStylesCount),
            eyeStyle = Random.Range(0, EyeStylesCount),
            mouthStyle = Random.Range(0, MouthStylesCount),
            bodyStyle = Random.Range(0, BodyStylesCount),
            pantStyle = Random.Range(0, PantStylesCount),
            leftArmStyle = Random.Range(0, LeftArmStylesCount),
            rightArmStyle = Random.Range(0, RightArmStylesCount),
            leftLegStyle = Random.Range(0, leftLegStylesCount),
            rightLegStyle = Random.Range(0, rightLegStylesCount),
};
    }
}
