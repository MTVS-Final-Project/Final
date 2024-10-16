using UnityEngine;
using System;

[Serializable]
public class AvatarData
{
    public int bodyId;
    public int eyesId;
    public int hairId;
    public int pantId;
    public int rightArmId;
    public int leftArmId;
    public int rightLegId;
    public int leftLegId;
    public int rightShoesId;
    public int leftShoesId;

    // Unity의 Color 구조체는 기본적으로 JSON 직렬화가 되지 않으므로,
    // 색상값을 문자열로 저장합니다.
    public string skinColorHex;
    public string eyesColorHex;
    public string pantColorHex;
    public string rightArmColorHex;
    public string leftArmColorHex;
    public string rightLegColorHex;
    public string leftLegColorHex;
    public string rightShoesColorHex;
    public string leftShoesColorHex;

    // JSON 문자열로 변환
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    // JSON 문자열에서 AvatarData 객체로 변환
    public static AvatarData FromJson(string json)
    {
        return JsonUtility.FromJson<AvatarData>(json);
    }

    // Color를 HEX 문자열로 변환
    public static string ColorToHex(Color color)
    {
        return ColorUtility.ToHtmlStringRGBA(color);
    }

    // HEX 문자열을 Color로 변환
    public static Color HexToColor(string hex)
    {
        ColorUtility.TryParseHtmlString("#" + hex, out Color color);
        return color;
    }
}