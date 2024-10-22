using UnityEngine;
using UnityEngine.UI;

public class ChangeSkinColor : MonoBehaviour
{
    public GameObject panel;
    public SpriteRenderer skin;
    public Image squareSkinDisplay;

    public Color[] colors;
    public int whatColor = 1;

    private void Update()
    {
        squareSkinDisplay.color = skin.color;
        for(int i = 0; i < colors.Length; i++)
        {
            if(i == whatColor)
            {
                skin.color = colors[i];
            }
        }
    }


    public void OpenPanel()
    {
        panel.SetActive(true);
    }
    public void ClosePanel()
    {
        panel.SetActive(false);
    }
    public void ChangesinColor(int idex)
    {
        whatColor = idex;
    }
}
