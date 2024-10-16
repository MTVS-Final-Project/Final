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

    // Unity�� Color ����ü�� �⺻������ JSON ����ȭ�� ���� �����Ƿ�,
    // ������ ���ڿ��� �����մϴ�.
    public string skinColorHex;
    public string eyesColorHex;
    public string pantColorHex;
    public string rightArmColorHex;
    public string leftArmColorHex;
    public string rightLegColorHex;
    public string leftLegColorHex;
    public string rightShoesColorHex;
    public string leftShoesColorHex;

    // JSON ���ڿ��� ��ȯ
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    // JSON ���ڿ����� AvatarData ��ü�� ��ȯ
    public static AvatarData FromJson(string json)
    {
        return JsonUtility.FromJson<AvatarData>(json);
    }

    // Color�� HEX ���ڿ��� ��ȯ
    public static string ColorToHex(Color color)
    {
        return ColorUtility.ToHtmlStringRGBA(color);
    }

    // HEX ���ڿ��� Color�� ��ȯ
    public static Color HexToColor(string hex)
    {
        ColorUtility.TryParseHtmlString("#" + hex, out Color color);
        return color;
    }
}