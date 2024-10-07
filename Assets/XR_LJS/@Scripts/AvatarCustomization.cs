using UnityEngine;
using UnityEngine.UI;

public class AvatarCustomization : MonoBehaviour
{
    public GameObject panel;
    public SpriteRenderer body;
    public Image spuareHeadDisplay;
    public Color[] colors;
    public int WhatColor = 1;

    void Update()
    {
        spuareHeadDisplay.color = body.color;
        for (int i = 0; i < colors.Length; i++)
        {
            if(i == WhatColor)
            {
                body.color = colors[i];
            }
        }
    }


    public void ChangePanelState(bool state)
    {
        panel.SetActive(state);
    }

    public void ChangeHeadColor(int index)
    {
        WhatColor = index;
    }
}
