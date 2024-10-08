using UnityEngine;

public class Appearance : MonoBehaviour
{
    public SpriteRenderer part;
    public Sprite[] option;
    public int index;


    void Update()
    {
        if (index >= 0 && index < option.Length) // �ε��� ���� üũ
        {
            part.sprite = option[index]; // ���� �ε����� ��������Ʈ�� ����
        }
    }

    public void Swap()
    {
        if (index < option.Length - 1)
        {
            index++;
        }
        else
        {
            index = 0;
        }
    }
}
