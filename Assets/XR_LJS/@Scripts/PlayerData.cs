
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    public Data data;

    private void OnEnable()
    {
        data = new Data();
        PlayerData.Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public string AvatarToString()
    {
        string returnString = JsonUtility.ToJson(PlayerData.Instance.data);
        return returnString;
    }
}

public class Data
{
    // Room 
    public int myNumberInRoom;

    // 아바타
    public int skin;
    public Color skinColor;
    public int Eyes;
    public Color EyesColor;
    public int hair;
    public Color hairColor;
    public int body;
    public Color bodyColor;
    public int pant;
    public Color pantColor;
    public int rightArm;
    public Color rightArmColor;
    public int leftArm;
    public Color leftArmColor;
    public int rightleg;
    public Color rightLegColor;
    public int leftLeg;
    public Color leftLegColor;
    public int rightShoes;
    public Color rightShoesColor;
    public int leftShoes;
    public Color leftShoesColor;


    // 악세서리 
    public int hat;
    public Color hatColor;

}
