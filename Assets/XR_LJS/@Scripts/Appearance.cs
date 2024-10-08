using UnityEngine;

public class Appearance : MonoBehaviour
{
    public SpriteRenderer part;
    public Sprite[] option;
    public int index;


    void Update()
    {
        if (index >= 0 && index < option.Length) // 인덱스 범위 체크
        {
            part.sprite = option[index]; // 현재 인덱스의 스프라이트로 설정
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
