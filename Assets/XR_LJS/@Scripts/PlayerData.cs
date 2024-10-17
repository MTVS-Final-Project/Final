using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance; // Singleton �ν��Ͻ�
    public Data data; // �÷��̾� ������

    private void Awake()
    {
        // �̹� �ν��Ͻ��� �����ϴ��� Ȯ��
        if (Instance == null)
        {
            Instance = this; // �ν��Ͻ� ����
            data = new Data(); // ������ �ʱ�ȭ
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // ���� �ν��Ͻ� ����
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

    // �ƹ�Ÿ
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

    // �Ǽ����� 
    public int hat;
}
