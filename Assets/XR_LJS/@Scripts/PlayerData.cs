using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }
    public Data data;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // �ߺ��� �ν��Ͻ� ����
            return;
        }
        Instance = this;
        data = new Data();
    }

    // JSON ��ȯ �޼����
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
    // Room ����
    public int myNumberInRoom;

    // �⺻ ����
    [Header("Basic Appearance")]
    public int skin;
    public Color skinColor;
    public int eyes;  // ������ �ҹ��ڷ� ����
    public int hair;

    // �ǻ�
    [Header("Clothing")]
    public int body;
    public int pant;

    // �ȴٸ�
    [Header("Limbs")]
    public int rightArm;
    public int leftArm;
    public int rightLeg;
    public int leftLeg;

    // �Ź�
    [Header("Shoes")]
    public int rightShoes;
    public int leftShoes;

    // �׼�����
    [Header("Accessories")]
    public int hat;

    // �⺻�� ������ ���� ������
    public Data()
    {
        // �⺻�� �ʱ�ȭ
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