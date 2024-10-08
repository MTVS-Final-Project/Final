using UnityEngine;

public class Appearance : MonoBehaviour
{
    public SpriteRenderer part;
    public Sprite[] option;
    public int index;


    void Update()
    {
        for (int i = 0; i < option.Length; i++)
        {
            if (i == index)
            {
                part.sprite = option[i];
            }
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
